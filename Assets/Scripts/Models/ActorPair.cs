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
    public Axis axis;
    public List<TileBehavior> gaps = new List<TileBehavior>();
    public List<ActorBehavior> enemies = new List<ActorBehavior>();
    public List<ActorBehavior> players = new List<ActorBehavior>();

}
