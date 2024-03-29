
public enum Team
{
    Player,
    Enemy,
    Independant
}


public enum Direction
{
    None,
    North,
    East,
    South,
    West,
    NorthWest,
    NorthEast,
    SouthEast,
    SouthWest,
    Up,
    Down
}

public enum Axis
{
    None,
    Vertical,
    Horizontal
}

public enum TurnPhase
{
    Start,
    Move,
    Attack,
    //End
}


public enum ActionIcon
{
    None,
    Attack,
    Sleep,
    Support
}

public enum GlowState
{
    Off,
    On
}

public enum EnemyTurnDelay
{
    None,
    Random
}

public enum Archetype
{
    Paladin,
    Barbarian,
    Ninja,
    Sentinel,
    PandaGirl,
    Cleric,
    Slime,
    Bat,
    Scorpion,
    Yeti
}
