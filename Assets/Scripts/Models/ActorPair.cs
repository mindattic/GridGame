using System.Collections.Generic;

public class ActorPair
{
    public ActorPair() { }
    public ActorPair(ActorBehavior actor1, ActorBehavior actor2, Axis axis)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.axis = axis;
    }

    public ActorBehavior actor1;
    public ActorBehavior actor2;

    public ActorBehavior highest;
    public ActorBehavior lowest;

    public float ceiling => axis == Axis.Vertical ? highest.location.y : highest.location.x;
    public float floor => axis == Axis.Vertical ? lowest.location.y : lowest.location.x;

    public Axis axis;
    public IEnumerable<TileBehavior> gaps;
    public IEnumerable<ActorBehavior> enemies;
    public IEnumerable<ActorBehavior> players;

    public bool Matches(ActorBehavior actor1, ActorBehavior actor2)
    {
        return (this.actor1 == actor1 && this.actor2 == actor2) || (this.actor1 == actor2 && this.actor2 == actor1);
    }
}
