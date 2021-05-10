using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private List<MouseAction> mMouseActions = new List<MouseAction>();
    private bool mIsRecording = false;
    private IEnumerator mReplaying = default;

    [SerializeField] private float mStepPeriod = 0.05f;
    [SerializeField] private MouseActionList mMouseActionList;

    public void ClearSequence()
    {
        mMouseActions.Clear();
        mMouseActionList.Clear();
    }

    private void Start()
    {
        KeyboardOperations.SubscribeKey(VirtualKeyCode.F3, StartRecord);
        KeyboardOperations.SubscribeKey(VirtualKeyCode.F4, StopRecord);

        KeyboardOperations.SubscribeKey(VirtualKeyCode.F5, StartReplay);
        KeyboardOperations.SubscribeKey(VirtualKeyCode.F6, StopReplay);

        KeyboardOperations.SubscribeKey(VirtualKeyCode.MouseLeftButton, OnMouseLeftButtonDown, OnMouseLeftButtonUp);
    }

    private void StartRecord()
    {
        StopReplay();
        StopRecord();

        mIsRecording = true;
    }

    private void OnMouseLeftButtonDown()
    {
        if (!mIsRecording)
            return;

        var element = new MouseAction(MouseEventFlags.LeftDown, MouseOperations.GetCursorPosition());
        mMouseActions.Add(element);
        mMouseActionList.Add(element);
    }

    private void OnMouseLeftButtonUp()
    {
        if (!mIsRecording)
            return;

        var element = new MouseAction(MouseEventFlags.LeftUp, MouseOperations.GetCursorPosition());
        mMouseActions.Add(element);
        mMouseActionList.Add(element);
    }

    private void StopRecord()
    {
        mIsRecording = false;
    }


    private void StartReplay()
    {
        StopReplay();
        StopRecord();

        mReplaying = PerformReplay();
        StartCoroutine(mReplaying);
    }

    private IEnumerator PerformReplay()
    {
        while (true)
        {
            yield return new WaitForSeconds(mStepPeriod);

            foreach (var action in mMouseActions)
            {
                MouseOperations.PerformMouseEvent(action.Type, action.Position);
                yield return new WaitForSeconds(mStepPeriod);
            }
        }
    }

    private void StopReplay()
    {
        if (mReplaying != null)
            StopCoroutine(mReplaying);
    }
}
