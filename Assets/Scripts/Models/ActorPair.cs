using System.Collections.Generic;

public class ActorPair
{

    public ActorBehavior Actor1;
    public ActorBehavior Actor2;

    public ActorBehavior HighestActor;
    public ActorBehavior LowestActor;

    public Axis Axis;
    public List<TileBehavior> Gaps;
    public List<ActorBehavior> Enemies;
    public List<ActorBehavior> Players;

    public ActorPair() { }
    public ActorPair(ActorBehavior actor1, ActorBehavior actor2, Axis axis)
    {
        this.Actor1 = actor1;
        this.Actor2 = actor2;
        this.Axis = axis;
    }

    public float Ceiling => Axis == Axis.Vertical ? HighestActor.location.y : HighestActor.location.x;
    public float Floor => Axis == Axis.Vertical ? LowestActor.location.y : LowestActor.location.x;

   
    public bool HasMatch(ActorBehavior actor1, ActorBehavior actor2)
    {
        return (this.Actor1 == actor1 && this.Actor2 == actor2) || (this.Actor1 == actor2 && this.Actor2 == actor1);
    }
}
