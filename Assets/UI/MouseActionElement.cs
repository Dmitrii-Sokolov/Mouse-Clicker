using UnityEngine;
using UnityEngine.UI;

public class MouseActionElement : MonoBehaviour
{
    [SerializeField] private Text mType = default;
    [SerializeField] private Text mPosition = default;

    public void Init(MouseAction mouseAction)
    {
        mType.text = mouseAction.Type.ToString();
        mPosition.text = mouseAction.Position.ToString();
    }
}
