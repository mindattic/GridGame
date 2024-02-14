
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

public enum From
{
    Above,
    Right,
    Below,
    Left
}

public enum Direction
{
    None,
    North,
    East,
    South,
    West
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
