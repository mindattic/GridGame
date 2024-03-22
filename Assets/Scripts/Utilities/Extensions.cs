using System;
using System.Collections;
using System.Collections.Generic;
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

public static class Vector3Extensions
{
    public static void SetX(this Vector3 v3, float value)
    {
        v3 = new Vector3(value, v3.y, v3.z);
    }

    public static void AddX(this Vector3 v3, float value)
    {
        v3 = new Vector3(v3.x + value, v3.y, v3.z);
    }

    public static void SetY(this Vector3 v3, float value)
    {
        v3 = new Vector3(v3.x, value, v3.z);
    }

    public static void AddY(this Vector3 v3, float value)
    {
        v3 = new Vector3(v3.x , v3.y + value, v3.z);
    }

    public static void SetZ(this Vector3 v3, float value)
    {
        v3 = new Vector3(v3.x, v3.y, value);
    }

    public static void AddZ(this Vector3 v3, float value)
    {
        v3 = new Vector3(v3.x, v3.y , v3.z + value);
    }


}


public static class IntExtensions
{
    public static float ToFloat(this int i)
    {
        return (float)i;
    }
}