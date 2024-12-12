using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class ActorManager : ExtendedMonoBehavior
    {

        public void AccumulateAP()
        {
            var waitingEnemies = enemies.Where(x => x.IsPlaying && !x.IsReady).ToList();
            foreach (var enemy in enemies)
            {
                enemy.GainAPAsync();
            }
        }

        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
            actors.Clear();
        }
    }
}