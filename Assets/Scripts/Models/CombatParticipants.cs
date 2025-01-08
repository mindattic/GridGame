using Game.Behaviors.Actor;
using System.Collections.Generic;
using System.Linq;

public class CombatParticipants
{
    public HashSet<ActorPair> alignedPairs = new HashSet<ActorPair>();
    public HashSet<ActorPair> attackingPairs = new HashSet<ActorPair>();
    public HashSet<ActorPair> supportingPairs = new HashSet<ActorPair>();

    public CombatParticipants() { }


    public void Clear()
    {
        //DespawnAll pairs
        alignedPairs.Clear();
        attackingPairs.Clear();
        supportingPairs.Clear();
    }

    /// <summary>
    /// Method which is used to select all actor instances participating in combat,
    /// if a pair is specified it selects all actors participating in an attack 
    /// between a pair of attackers
    /// </summary>
    /// <param name="pair">A pair of attackers</param>
    /// <returns></returns>
    public List<ActorInstance> SelectAll(ActorPair pair = null)
    {
        if (pair != null)
        {
            // If a specific pair is provided, include actor1, actor2, and alignment opponents
            return new[] { pair.actor1, pair.actor2 }
                .Concat(pair.alignment.opponents)
                .Distinct()
                .ToList();
        }

        // If no pair is specified, gather actors and alignment opponents from all collections
        return alignedPairs
            .SelectMany(x => new[] { x.actor1, x.actor2 }.Concat(x.alignment.opponents)) // Include alignment opponents
            .Concat(attackingPairs.SelectMany(x => new[] { x.actor1, x.actor2 }.Concat(x.alignment.opponents)))
            .Concat(supportingPairs.SelectMany(x => new[] { x.actor1, x.actor2 }.Concat(x.alignment.opponents)))
            .Distinct()
            .ToList();
    }

    public bool HasAlignedPair(ActorInstance actor1, ActorInstance actor2)
    {
        return alignedPairs.Count > 0 && alignedPairs.Any(x => x.HasPair(actor1, actor2));
    }


    public bool HasAttackingPair(ActorPair pair)
    {
        return attackingPairs.Count > 0 && attackingPairs.Any(x => x.HasPair(pair.actor1, pair.actor2));
    }

    public bool HasSupportingPair(ActorPair pair)
    {
        return supportingPairs.Count > 0 && supportingPairs.Any(x => x.HasPair(pair.actor1, pair.actor2));
    }

}
