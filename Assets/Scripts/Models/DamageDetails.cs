using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class DamageDetails
    {
        public DamageDetails() { }
        public DamageDetails(float damage, bool isCriticalHit)
        {
            Damage = damage;
            IsCriticalHit = isCriticalHit;
        }

        public float Damage;
        public bool IsCriticalHit;
    }
}
