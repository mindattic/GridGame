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



    public bool HasAlignedPair(ActorBehavior actor1, ActorBehavior actor2)
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
