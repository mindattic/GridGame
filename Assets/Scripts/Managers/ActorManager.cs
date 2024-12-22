using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class ActorManager : ExtendedMonoBehavior
    {

        public void AccumulateAP()
        {
            var notReadyEnemies = enemies.Where(x => !x.IsReady).ToList();
            notReadyEnemies.ForEach(x => x.GainAPAsync());
        }


        public void CheckEnemyReadiness()
        {
            var notReadyEnemies = enemies.Where(x => !x.IsReady).ToList();
            notReadyEnemies.ForEach(x => x.CheckReady());
        }



        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
            actors.Clear();
        }
    }
}