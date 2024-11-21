using System;
using System.Linq;

public class ActorPair
{
    //Variables
    public ActorInstance actor1;
    public ActorInstance actor2;
    public Axis axis;
    public Alignment alignment = new Alignment();
 
    //Properties
    public ActorInstance highestActor
    {
        get
        {
            return (axis == Axis.Vertical)
                ? actor1.location.y > actor2.location.y ? actor1 : actor2
                : actor1.location.x > actor2.location.x ? actor1 : actor2;
        }
    }

    public ActorInstance lowestActor
    {
        get
        {
            return (axis == Axis.Vertical)
                ? actor1.location.y < actor2.location.y ? actor1 : actor2
                : actor1.location.x < actor2.location.x ? actor1 : actor2;
        }
    }

    public int sortingOrder
    {
        get
        {
            return actor1.sortingOrder;
        }
        set
        {
            actor1.sortingOrder = value;
            actor2.sortingOrder = value;
            alignment.enemies.ForEach(x => x.sortingOrder = value);
        }
    }

    public float ceiling => axis == Axis.Vertical ? highestActor.location.y : highestActor.location.x;
    public float floor => axis == Axis.Vertical ? lowestActor.location.y : lowestActor.location.x;



    public ActorPair(ActorInstance actor1, ActorInstance actor2, Axis axis)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.axis = axis;
        this.alignment = Shared.AssignAlignment(actor1, actor2, axis);
    }

    public bool HasPair(ActorInstance actor1, ActorInstance actor2)
    {
        return (this.actor1 == actor1 && this.actor2 == actor2) || (this.actor1 == actor2 && this.actor2 == actor1);
    }


}
