using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    [System.Serializable]
    public class Enemy
    {
        public int Id;
        public string Name;
        public string Description;
        public float Level;
        public float MaxHp;
        public float Strength;
        public float Vitality;
        public float Agility;
        public float Speed;
        public float Luck;
    }
}
