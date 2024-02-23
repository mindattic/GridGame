using System.Collections.Generic;
using UnityEngine;








public class Destination
{

    public Destination() { }

    public Vector2Int? location;
    public Vector3? position;
    public Direction direction; 


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
    public string name;
    public Sprite sprite;
    public Team team;
    public Vector2Int location;
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

    public ActorBehavior actor1;
    public ActorBehavior actor2;
    public Axis axis;
    public List<TileBehavior> gaps = new List<TileBehavior>();
    public List<ActorBehavior> enemies = new List<ActorBehavior>();
    public List<ActorBehavior> players = new List<ActorBehavior>();

}



public class ActorRenderers
{
    public ActorRenderers() { }

    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;
    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBar;
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