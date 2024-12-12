using System;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public class Enemy : ActorStats
    {
        public int Id;
        public string Name;
        public string Variant;
        public string Description;
        public float MaxHp;
    }
}
