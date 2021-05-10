using UnityEngine;

public class MouseActionList : MonoBehaviour
{
    [SerializeField] private MouseActionElement mPrefab;

    public void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public void Add(MouseAction mouseAction)
    {
        var element = Instantiate(mPrefab, transform);
        element.Init(mouseAction);
    }
}
