using System.Collections.Generic;

public class AttackParticipants
{
    public HashSet<ActorPair> alignedPairs = new HashSet<ActorPair>();
    public HashSet<ActorPair> attackingPairs = new HashSet<ActorPair>();

    public HashSet<ActorBehavior> attackers = new HashSet<ActorBehavior>();
    public HashSet<ActorBehavior> supporters = new HashSet<ActorBehavior>();
    public HashSet<ActorBehavior> defenders = new HashSet<ActorBehavior>();

    public AttackParticipants() { }


    public void Reset()
    {
        //Clear pairs
        alignedPairs.Clear();
        attackingPairs.Clear();

        //Clear participants
        attackers.Clear();
        supporters.Clear();
        defenders.Clear();
    }
}
