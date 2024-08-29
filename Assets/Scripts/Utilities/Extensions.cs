using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public static class EnumExtensions
{
    public static T Next<T>(this T src) where T : struct
    {
        var values = (T[])Enum.GetValues(src.GetType());
        int index = Array.IndexOf<T>(values, src) + 1;
        return (values.Length == index) ? values[0] : values[index];
    }
}

public static class IEnumeratorExtensions
{
    public static IEnumerator WaitUntil(this ExtendedMonoBehavior monoBehaviour, IEnumerator coroutine)
    {
        return new WaitAll(monoBehaviour, coroutine);
    }

    public static IEnumerator WaitAll(this ExtendedMonoBehavior monoBehaviour, params IEnumerator[] coroutines)
    {
        return new WaitAll(monoBehaviour, coroutines);
    }

    public static IEnumerator WaitAny(this ExtendedMonoBehavior monoBehaviour, params IEnumerator[] coroutines)
    {
        return new WaitAny(monoBehaviour, coroutines);
    }
}


public static class ListExtensions
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        return list.OrderBy(x => Guid.NewGuid()).ToList();
    }
}

public static class Vector3Extensions
{
    public static Vector3 SetX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 AddX(this Vector3 v, float x)
    {
        return new Vector3(v.x + x, v.y, v.z);
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 AddY(this Vector3 v, float y)
    {
        return new Vector3(v.x, v.y + y, v.z);
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 AddZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, v.z + z);
    }

    public static bool HasNaN(this Vector3 v)
    {
        return v.x == float.NaN || v.y == float.NaN || v.z == float.NaN;
    }

}

public static class ColorExtensions
{

    public static Color SetA(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, c.a);
    }

    public static Color AddA(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, c.a + a);
    }
}

public static class IntExtensions
{
    public static float ToFloat(this int i)
    {
        return (float)i;
    }
}

public static class FloatExtensions
{
    public static int ToInt(this float f)
    {
        return (int)f;
    }
}

static class StopwatchExtensions
{
    /// <summary>
    /// Gets estimated time on compleation. 
    /// </summary>
    /// <param name="sw"></param>
    /// <param name="counter"></param>
    /// <param name="counterGoal"></param>
    /// <returns></returns>
    public static TimeSpan GetEta(this Stopwatch sw, int counter, int counterGoal)
    {
        /* this is based off of:
         * (TimeTaken / linesProcessed) * linesLeft=timeLeft
         * so we have
         * (10/100) * 200 = 20 Seconds now 10 seconds go past
         * (20/100) * 200 = 40 Seconds left now 10 more seconds and we process 100 more lines
         * (30/200) * 100 = 15 Seconds and now we all see why the copy file dialog jumps from 3 hours to 30 minutes :-)
         * 
         * pulled from http://stackoverflow.com/questions/473355/calculate-time-remaining/473369#473369
         */
        if (counter == 0) return TimeSpan.Zero;
        float elapsedMin = ((float)sw.ElapsedMilliseconds / 1000) / 60;
        float minLeft = (elapsedMin / counter) * (counterGoal - counter); //see comment a
        TimeSpan ret = TimeSpan.FromMinutes(minLeft);
        return ret;
    }
}




