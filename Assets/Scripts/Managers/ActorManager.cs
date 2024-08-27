using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class ActorManager : ExtendedMonoBehavior
    {

        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
            actors.Clear();
        }
    }
}