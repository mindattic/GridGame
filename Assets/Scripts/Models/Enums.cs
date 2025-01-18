
using NUnit.Framework;

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
    //Stop
}


public enum Status
{
    None,
    Poisoned,
    Cursed,
    Sleeping,
    Doom
}



//public enum GlowState
//{
//    Off,
//    On
//}

public enum Character
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
    AttackClosest,
    AttackWeakest,
    AttackStrongest,
    AttackRandom,
    MoveAnywhere
}

//public enum BumpStage
//{
//    StartCoroutine,
//    MoveToward,
//    MoveAway,
//    End
//}

public enum DodgeStage
{
    Start,
    TwistForward,
    TwistBackward,
    End
}

public enum AttackOutcome
{
    None = 0,
    Miss = 1,
    Hit = 2,
    CriticalHit = 3
}

public enum CoinState
{
    Start,
    Move,
    Stop,
    Destroy
}

public enum WeaponType
{
    Dagger,
    Hammer,
    Katana,
    Mace,
    Spear, 
    Sword,
    Wand
}

public enum DebugOptions
{
    None,
    DodgeTest,
    SpinTest,
    ShakeTest,
    BumpTest,
    AlignTest,
    CoinTest,
    PortraitTest,
    DamageTextTest,  
    SupportLineTest,
    AttackLineTest,
    EnemyAttackTest,
    TitleTest,
    TooltipTest
}

public enum VFXOptions
{
    None,
    VFXTest_Blue_Slash_01,
    VFXTest_Blue_Slash_02,
    VFXTest_Blue_Slash_03,
    VFXTest_Blue_Sword,
    VFXTest_Blue_Sword_4X,
    VFXTest_Blood_Claw,
    VFXTest_Level_Up,
    VFXTest_Yellow_Hit,
    VFXTest_Double_Claw,
    VFXTest_Lightning_Explosion,
    VFXTest_Buff_Life,
    VFXTest_Rotary_Knife,
    VFXTest_Air_Slash,
    VFXTest_Fire_Rain,
    VFXTest_Ray_Blast,
    VFXTest_Lightning_Strike,
    VFXTest_Puffy_Explosion,
    VFXTest_Red_Slash_2X,
    VFXTest_God_Rays,
    VFXTest_Acid_Splash,
    VFXTest_Green_Buff,
    VFXTest_Gold_Buff,
    VFXTest_Hex_Shield,
    VFXTest_Toxic_Cloud,
    VFXTest_Orange_Slash,
    VFXTest_Moon_Feather,
    VFXTest_Pink_Spark,
    VFXTest_BlueYellow_Sword,
    VFXTest_BlueYellow_Sword_3X,
    VFXTest_Red_Sword
}

public enum GameSpeedOption
{
    Paused = 0,
    Slower = 1,
    Slow = 2,
    Normal = 3,
    Fast = 4,
    Faster = 5
}