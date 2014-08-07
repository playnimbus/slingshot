using System;
using UnityEngine;

public static class Utils
{
    public static Vector2 Vector2FromDegrees(float degrees)
    {
        return Vector2FromRadians(Mathf.Deg2Rad * degrees);
    }

    public static Vector2 Vector2FromRadians(float radians)
    {
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);
        return new Vector2(x, y);
    }

    public static Vector2 PerpindicularVector(Vector2 direction, bool clockwise = true)
    {
        if (clockwise) return new Vector2(direction.y, -direction.x);
        else return new Vector2(-direction.y, direction.x);
    }

    /// <summary>
    /// Easing equation function for a quadratic (t^2) easing in/out: 
    /// acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="t">Current time in seconds.</param>
    /// <param name="b">Starting value.</param>
    /// <param name="c">Final value.</param>
    /// <param name="d">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static double CubicEaseIn(double t, double b, double c, double d)
    {
        return c * (t /= d) * t * t + b;
    }

    /// <summary>
    /// Easing equation function for a cubic (t^3) easing out: 
    /// decelerating from zero velocity.
    /// </summary>
    /// <param name="t">Current time in seconds.</param>
    /// <param name="b">Starting value.</param>
    /// <param name="c">Final value.</param>
    /// <param name="d">Duration of animation.</param>
    /// <returns>The correct value.</returns>
    public static double CubicEaseOut(double t, double b, double c, double d)
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }
}
