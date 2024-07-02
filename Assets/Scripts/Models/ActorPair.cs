using Game.Behaviors.Actor;
using System.Collections.Generic;

public class ActorPair
{

    public ActorBehavior actor1;
    public ActorBehavior actor2;

    public ActorBehavior highestActor;
    public ActorBehavior lowestActor;

    public Axis axis;
    public List<TileBehavior> gaps;
    public List<ActorBehavior> enemies;
    public List<ActorBehavior> players;

    public ActorPair() { }
    public ActorPair(ActorBehavior actor1, ActorBehavior actor2, Axis axis)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.axis = axis;
    }

    public float Ceiling => axis == Axis.Vertical ? highestActor.location.y : highestActor.location.x;
    public float Floor => axis == Axis.Vertical ? lowestActor.location.y : lowestActor.location.x;

   
    public bool HasMatch(ActorBehavior actor1, ActorBehavior actor2)
    {
        return (this.actor1 == actor1 && this.actor2 == actor2) || (this.actor1 == actor2 && this.actor2 == actor1);
    }
}
