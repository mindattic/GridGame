using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class Trigger
    {

        //public Trigger nestedTrigger = null;

        //Variables
        public IEnumerator Coroutine = null;
        public bool IsAsync = true;
        public bool HasTriggered = false;
        private Dictionary<string, object> attributes = new Dictionary<string, object>();

        //Properties
        public bool IsValid => Coroutine != null && !HasTriggered;

        //Constructors
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

        public IEnumerator Start(ActorInstance instance)
        {
            HasTriggered = true;

            if (!instance.isActive || !instance.isAlive || !IsValid)
                yield break;

            if (!IsAsync)
                yield return Coroutine;

            instance.StartCoroutine(Coroutine);
            yield break;
        }

        public bool HasAttribute(string key)
        {
            return attributes.ContainsKey(key);
        }

        public void AddAttribute(string key, object value)
        {
            attributes[key] = value;
        }

        public T GetAttribute<T>(string key, T defaultValue = default)
        {
            return attributes.TryGetValue(key, out var value) ? (T)value : defaultValue;
        }



    }
}
