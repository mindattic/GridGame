
using Unity.Burst.Intrinsics;

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


public enum Status
{
    None,
    Attack,
    Wait,
    Support,
    Move,
    Ready
}

//public enum GlowState
//{
//    Off,
//    On
//}

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

public enum Glow
{
    None,
    White,
    Red,
    Green,
    Blue
}

public enum Shadow
{
    None,
    White,
    Red,
    Green,
    Blue
}

public enum AttackStrategy
{
    MoveAnywhere,
    AttackClosest,
    AttackWeakest
}

