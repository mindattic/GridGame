
public enum Team
{
    Player,
    Enemy,
    Neutral
}

public enum ActorMoveState
{
    Idle,
    Moving
}

public enum Direction
{
    None,
    North,
    East,
    South,
    West,
}

public enum Axis
{
    None,
    Vertical,
    Horizontal
}

public enum TurnPhase
{
    None,
    Start,
    Move,
    PreAttack,
    Attack,
    PostAttack,
    End
}