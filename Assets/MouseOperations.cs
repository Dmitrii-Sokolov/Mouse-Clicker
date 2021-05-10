using System.Runtime.InteropServices;
using UnityEngine;

public static class MouseOperations
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static implicit operator Vector2Int(MousePoint point) => new Vector2Int(point.X, point.Y);
        public static implicit operator MousePoint(Vector2Int vector) => new MousePoint(vector.x, vector.y);
    }

    public static Vector2Int GetCursorPosition()
    {
        return GetCursorPosition(out var currentMousePoint)
            ? (Vector2Int) currentMousePoint
            : new Vector2Int(0, 0);
    }

    public static void SetCursorPosition(Vector2Int point)
    {
        SetCursorPosition(point.x, point.y);
    }

    public static void PerformMouseEvent(MouseEventFlags action)
    {
        MouseEvent((int)action, 0, 0, 0, 0);
    }

    public static void PerformMouseEvent(MouseEventFlags action, Vector2Int position)
    {
        SetCursorPosition(position);
        MouseEvent((int)action, 0, 0, 0, 0);
    }

    [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPosition(out MousePoint lpMousePoint);

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPosition(int x, int y);

    [DllImport("user32.dll", EntryPoint = "mouse_event")]
    private static extern void MouseEvent(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
}