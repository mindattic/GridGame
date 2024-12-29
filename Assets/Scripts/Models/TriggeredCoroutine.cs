using System.Collections;

namespace Assets.Scripts.Models
{
    public class TriggeredCoroutine
    {
        public IEnumerator Coroutine;
        public bool isAsync = true;
    }
}
