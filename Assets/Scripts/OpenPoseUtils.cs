using UnityEngine;

public static class OpenPoseUtils
{
    public static float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }

    public static float Vec2Angle(Vector2 from, Vector2 to)
    {
        var dot = Vector2.Dot(from.normalized, to.normalized);
        dot = Mathf.Clamp(dot, -1.0f, 1.0f);
        var acos = Mathf.Acos(dot) / Mathf.PI * 180.0f;
        var cross = Vec2Cross(from, to);
        if (cross < 0) acos *= -1;
        return acos;
    }
}