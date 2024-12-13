using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class ActorManager : ExtendedMonoBehavior
    {

        public void CheckAccumulateAP()
        {
            var notReadyEnemies = enemies.Where(x => x.IsPlaying && !x.IsReady).ToList();
            notReadyEnemies.ForEach(x => x.GainAPAsync());
        }


        public void CheckEnemyReadiness()
        {
            var notReadyEnemies = enemies.Where(x => x.IsPlaying && !x.IsReady).ToList();
            notReadyEnemies.ForEach(x => x.CheckReady());
        }



        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
            actors.Clear();
        }
    }
}