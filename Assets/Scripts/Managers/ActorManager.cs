using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class ActorManager : MonoBehaviour
    {
        protected List<ActorInstance> actors => GameManager.instance.actors;
        protected IQueryable<ActorInstance> players => GameManager.instance.players;
        protected IQueryable<ActorInstance> enemies => GameManager.instance.enemies;



        public void CheckEnemyAP()
        {
            var notReadyEnemies = enemies.Where(x => !x.hasMaxAP).ToList();
            notReadyEnemies.ForEach(x => x.actionBar.TriggerFill());
        }


        public void CheckEnemyAngry()
        {
            var notAngryEnemies = enemies.Where(x => !x.flags.isAngry).ToList();
            notAngryEnemies.ForEach(x => x.ExecuteAngry());
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