using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger
{
    // Variables
    public IEnumerator Coroutine = null;
    public bool IsAsync = true;
    public bool HasTriggered = false;
    public float Delay = 0f;
    private Dictionary<string, object> attributes = new Dictionary<string, object>();

    // Properties
    public bool IsValid => Coroutine != null && !HasTriggered;

    // Constructors
    public Trigger() { }
    public Trigger(IEnumerator coroutine)
    {
        Coroutine = coroutine;
    }
    public Trigger(IEnumerator coroutine, bool isAsync)
    {
        Coroutine = coroutine;
        IsAsync = isAsync;
    }

    public IEnumerator Start(MonoBehaviour context)
    {
        if (!IsValid)
            yield break;

        HasTriggered = true;

        if (!IsAsync)
        {
            yield return Coroutine;
        }
        else
        {
            context.StartCoroutine(Coroutine);
            yield break;
        }
    }

    public void AddAttribute(string key, object value)
    {
        attributes[key] = value;
    }

    public T GetAttribute<T>(string key, T defaultValue = default)
    {
        return attributes.TryGetValue(key, out var value) ? (T)value : defaultValue;
    }

    public bool HasAttribute(string key)
    {
        return attributes.ContainsKey(key);
    }
}
