using System.Collections;
using System.Linq;
using UnityEngine;

public class WaitAll : WaitBase
{
    public override bool keepWaiting => waitState.Any(t => t);

    public WaitAll(ExtendedMonoBehavior monoBehaviour, params IEnumerator[] coroutines) : base(monoBehaviour, coroutines)
    {

    }
}

public class WaitAny : WaitBase
{
    public override bool keepWaiting => waitState.All(t => t);

    public WaitAny(ExtendedMonoBehavior monoBehaviour, params IEnumerator[] coroutines) : base(monoBehaviour, coroutines)
    {

    }
}

public abstract class WaitBase : CustomYieldInstruction
{
    protected readonly bool[] waitState;

    protected WaitBase(ExtendedMonoBehavior monoBehaviour, params IEnumerator[] coroutines)
    {
        waitState = new bool[coroutines.Length];
        for (int i = 0; i < coroutines.Length; i++)
        {
            monoBehaviour.StartCoroutine(Wrapper(coroutines[i], i));
        }
    }

    private IEnumerator Wrapper(IEnumerator e, int index)
    {
        while (true)
        {
            if (e != null && e.MoveNext())
            {
                waitState[index] = true;
                yield return e.Current;
            }
            else
            {
                waitState[index] = false;
                break;
            }
        }
    }
}
