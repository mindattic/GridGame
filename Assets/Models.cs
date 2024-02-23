using System.Collections.Generic;
using UnityEngine;








public class Destination
{

    public Destination() { }

    public Vector2Int? location { get; set; }
    public Vector3? position { get; set; }
    public Direction direction { get; set; } 


    //public bool IsValid => location != null && position != null && direction != Direction.None;

    public void Clear()
    {
        location = null;
        position = null;
        direction = Direction.None;
    }

}



public class RectVector3
{
    public RectVector3() { }
}


public class RectFloat
{
    public RectFloat() { }
}


public class ActorInit
{
    public string name { get; set; }
    public Sprite sprite { get; set; }
    public Team team { get; set; }
    public Vector2Int location { get; set; }
    public ActorInit() { }
    public ActorInit(string name, Sprite sprite, Team team, Vector2Int location)
    {
        this.name = name;
        this.sprite = sprite;
        this.team = team;
        this.location = location;
    }
}

public class ActorPair
{
    public ActorPair() { }
    public ActorPair(ActorBehavior actor1, ActorBehavior actor2, Axis axis)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.axis = axis;
    }

    public ActorBehavior actor1 { get; set; }
    public ActorBehavior actor2 { get; set; }
    public Axis axis { get; set; }
    public List<TileBehavior> gaps { get; set; } = new List<TileBehavior>();
    public List<ActorBehavior> enemies { get; set; } = new List<ActorBehavior>();
    public List<ActorBehavior> players { get; set; } = new List<ActorBehavior>();

}



public class ActorRenderers
{
    public ActorRenderers() { }

    public SpriteRenderer thumbnail { get; set; }
    public SpriteRenderer healthBarBack { get; set; }
    public SpriteRenderer healthBar { get; set; }
}

public class BattleParticipants
{
    public HashSet<ActorPair> alignedPairs = new HashSet<ActorPair>();
    public HashSet<ActorPair> attackingPairs = new HashSet<ActorPair>();

    public HashSet<ActorBehavior> attackers = new HashSet<ActorBehavior>();
    public HashSet<ActorBehavior> supports = new HashSet<ActorBehavior>();
    public HashSet<ActorBehavior> defenders = new HashSet<ActorBehavior>();

    public BattleParticipants() { }


    public void Reset()
    {
        //Clear pairs
        alignedPairs.Clear();
        attackingPairs.Clear();

        //Clear participants
        attackers.Clear();
        supports.Clear();
        defenders.Clear();
    }
}