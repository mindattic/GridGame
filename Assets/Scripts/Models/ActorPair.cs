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
    public List<TileBehavior> gaps;
    public List<ActorBehavior> enemies;
    public List<ActorBehavior> players;

    public bool HasMatch(ActorBehavior actor1, ActorBehavior actor2)
    {
        return (this.actor1 == actor1 && this.actor2 == actor2) || (this.actor1 == actor2 && this.actor2 == actor1);
    }
}
