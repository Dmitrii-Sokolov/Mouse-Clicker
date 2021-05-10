using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class KeyboardOperations : MonoBehaviour
{
    private static KeyboardOperations mInstance;

    private List<ActionContainer> mSubscriptions = new List<ActionContainer>();

    private class ActionContainer
    {
        public VirtualKeyCode Code { get; }

        public bool Pressed { get; set; }
        public Action OnDown { get; set; }
        public Action OnUp { get; set; }

        public ActionContainer(VirtualKeyCode code)
        {
            Code = code;
        }
    }

    private static KeyboardOperations Instance
    {
        get
        {
            if (mInstance == null)
            {
                var go = new GameObject("KeyboardOperations");
                DontDestroyOnLoad(go);
                mInstance = go.AddComponent<KeyboardOperations>();
            }

            return mInstance;
        }
    }

    public static float RefreshPeriod { get; set; } = 0.001f;

    private void Awake()
    {
        StartCoroutine(CheckKeyboard());
    }

    public static IDisposable SubscribeKey(VirtualKeyCode keyCode, Action onDown, Action onUp = null)
    {
        var container = Instance.mSubscriptions.FirstOrDefault(s => s.Code == keyCode);
        if (container == null)
        {
            container = new ActionContainer(keyCode);
            container.Pressed = GetKeyPressed(keyCode);
            Instance.mSubscriptions.Add(container);
        }

        container.OnDown += onDown;
        container.OnUp += onUp;

        return new ActionDisposable(() =>
        {
            container.OnDown -= onDown;
            container.OnUp -= onUp;

            if (container.OnUp == null && container.OnDown == null)
                Instance.mSubscriptions.Remove(container);
        });
    }

    public static bool GetKeyPressed(VirtualKeyCode keyCode)
    {
        return GetKeyPressed((int)keyCode);
    }

    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeystate
    [DllImport("user32.dll")]
    private static extern short GetKeyState(int keyCode);

    private IEnumerator CheckKeyboard()
    {
        while (true)
        {
            yield return new WaitForSeconds(RefreshPeriod);

            foreach (var subscription in mSubscriptions)
            {
                var pressed = GetKeyPressed(subscription.Code);
                if (subscription.Pressed != pressed)
                {
                    if (pressed)
                        subscription.OnDown?.Invoke();

                    if (!pressed)
                        subscription.OnUp?.Invoke();

                    subscription.Pressed = pressed;
                }
            }
        }
    }

    private static bool GetKeyPressed(int keyCode)
    {
        return ((GetKeyState(keyCode)) & 0x8000) != 0;
    }
}