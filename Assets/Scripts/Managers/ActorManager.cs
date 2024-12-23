using System.Collections.Generic;
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

        //public void SetAttacking(List<ActorInstance> x, bool state)
        //{
        //    x.ForEach(x => x.SetAttacking(state));
        //}

        //public void SetDefending(List<ActorInstance> x, bool state)
        //{
        //    x.ForEach(x => x.SetDefending(state));
        //}

        //public void SetSupporting(List<ActorInstance> x, bool state)
        //{
        //    x.ForEach(x => x.SetSupporting(state));
        //}




        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
            actors.Clear();
        }
    }
}