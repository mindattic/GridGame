using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum CollisionDirection
{
    Top,
    Right,
    Bottom,
    Left 
}

public enum Team
{
    Player,
    Enemy,
    Neutral
}

public enum State
{
    Idle,
    Moving
}
