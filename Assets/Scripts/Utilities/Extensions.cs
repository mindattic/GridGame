using System;
using System.Collections;
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