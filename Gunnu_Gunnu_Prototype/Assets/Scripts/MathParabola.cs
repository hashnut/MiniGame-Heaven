using UnityEngine;
using System;

public class MathParabola
{
    // Works, but teleport when double jump
    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t, float jumpSpeed)
    {
        Func<float, float> f = x => -1.0f * height * (x - start.x) * (x - end.x) + start.y;

        float midX = Mathf.Lerp(start.x, end.x, ((t - start.x) / (end.x - start.x)) * jumpSpeed);

        return new Vector2(midX, f(midX));
    }

    // Precise parabola for double jump (no teleport)
    public static Vector2 ParabolaDouble(Vector2 start, Vector2 end, float height, float t, float jumpSpeed)
    {
        // Define Quadratic Function "f" : a*x^2 + b*x + c;
        float a = -1.0f * height;
        float b = -1.0f * a * (start.x + end.x) + (end.y - start.y) / (end.x - start.x);
        float c = a * start.x * end.x - 0.5f * ((start.x + end.x) * ((end.y - start.y) / (end.x - start.x)))  + 0.5f * (start.y + end.y);

        Func<float, float> f = x => a * x * x + b * x + c;
        float midX = Mathf.Lerp(start.x, end.x, ((t - start.x) / (end.x - start.x)) * jumpSpeed);

        return new Vector2(midX, f(midX));
    }



}