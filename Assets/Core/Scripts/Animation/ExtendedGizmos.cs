using UnityEngine;

public class ExtendedGizmos
{
    public enum Plane { XY, XZ, YZ }
    
    public static void DrawCircle(Vector3 center, float radius, int segmentsPerUnit = 32, Plane plane = Plane.XZ)
    {
        var segments = Mathf.Max(4, (int)(radius * segmentsPerUnit));
        var angle = 0f;
        var oldPoint = center + GetCircleOffset(angle, radius, plane);

        for (var i = 1; i <= segments; i++)
        {
            angle = i * Mathf.PI * 2 / segments;
            var newPoint = center + GetCircleOffset(angle, radius, plane);
            Gizmos.DrawLine(oldPoint, newPoint);
            oldPoint = newPoint;
        }
    }
    
    public static void DrawEllipse(Vector3 center, float width, float height, int segmentsPerUnit = 32, Plane plane = Plane.XZ)
    {
        var radiusMax = Mathf.Max(width, height) * 0.5f;
        var segments = Mathf.Max(4, (int)(radiusMax * segmentsPerUnit)); 
        var angle = 0f;
        var oldPoint = center + GetEllipseOffset(angle, width, height, plane);

        for (var i = 1; i <= segments; i++)
        {
            angle = i * Mathf.PI * 2 / segments;
            var newPoint = center + GetEllipseOffset(angle, width, height, plane);
            Gizmos.DrawLine(oldPoint, newPoint);
            oldPoint = newPoint;
        }
    }

    private static Vector3 GetEllipseOffset(float angle, float width, float height, Plane plane)
    {
        var a = width * 0.5f; 
        var b = height * 0.5f;

        var x = Mathf.Cos(angle) * a;
        var y = Mathf.Sin(angle) * b;

        return plane switch
        {
            Plane.XY => new Vector3(x, y, 0),
            Plane.XZ => new Vector3(x, 0, y),
            Plane.YZ => new Vector3(0, x, y),
            _ => Vector3.zero
        };
    }

    private static Vector3 GetCircleOffset(float angle, float radius, Plane plane)
    {
        var cos = Mathf.Cos(angle) * radius;
        var sin = Mathf.Sin(angle) * radius;

        return plane switch
        {
            Plane.XY => new Vector3(cos, sin, 0),
            Plane.XZ => new Vector3(cos, 0, sin),
            Plane.YZ => new Vector3(0, cos, sin),
            _ => Vector3.zero
        };
    }
}
