using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class ActorManager : ExtendedMonoBehavior
    {

        public void CheckEnemyAP()
        {
            var notReadyEnemies = enemies.Where(x => !x.HasMaxAP).ToList();
            notReadyEnemies.ForEach(x => x.CheckAP());
        }


        public void CheckEnemyAngry()
        {
            var notAngryEnemies = enemies.Where(x => !x.flags.isAngry).ToList();
            notAngryEnemies.ForEach(x => x.CheckAngry());
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