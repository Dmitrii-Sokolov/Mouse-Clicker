using UnityEngine;

public class MouseAction
{
    public MouseEventFlags Type;
    public Vector2Int Position;

    public MouseAction(MouseEventFlags type, Vector2Int position)
    {
        Type = type;
        Position = position;
    }
}
