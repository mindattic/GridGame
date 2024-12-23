using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class DamageResult
    {
        public ActorInstance Enemy { get; set; } // The enemy actor involved in the attack.
        public bool IsHit { get; set; }         // Indicates if the attack hit the enemy.
        public int Damage { get; set; }         // The calculated damage dealt to the enemy.
        public bool WillDie { get; set; }       // Indicates if the enemy will die as a result of the damage.
    }

}
