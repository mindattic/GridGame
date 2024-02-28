
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

public enum PortraitTransitionState
{
    None,
    FadeIn,
    FadeOut,
    FadeInOut,
    SlideIn,
    SlideOut,
    SlideInOut
}