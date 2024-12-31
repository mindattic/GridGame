using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Instances.Actor
{
    public class ActorGlow
    {
        protected TurnManager turnManager => GameManager.instance.turnManager;
        protected ActorRenderers render => instance.render;
        private bool isActive => instance.isActive;
        private bool isAlive => instance.isAlive;
        private bool isPlayer => instance.isPlayer;
        private bool isEnemy => instance.isEnemy;
        protected float gameSpeed => GameManager.instance.gameSpeed;
        protected AnimationCurve glowCurve => instance.glowCurve;

        //glowCurve = new AnimationCurve(
        //    new Keyframe(0f, 0f, 0f, 0f),      // First keyframe at time 0, value 0
        //    new Keyframe(1f, 0.25f, 0f, 0f)    // Second keyframe at time 1, value 0.25
        //);
        //glowCurve.preWrapMode = WrapMode.Loop;
        //glowCurve.postWrapMode = WrapMode.Loop;

        public float glowIntensity = 1.3333f;


        private ActorInstance instance;
        public void Initialize(ActorInstance parentInstance)
        {
            this.instance = parentInstance;
        }



        public void Update()
        {
            //Check abort conditions
            if (!isActive || !isAlive || !turnManager.isStartPhase || (turnManager.isPlayerTurn && !isPlayer) || (turnManager.isEnemyTurn && !isEnemy))
                return;

            //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
            var scale = new Vector3(
                glowIntensity + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
                glowIntensity + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
                1.0f);
            render.SetGlowScale(scale);
        }

    }
}
