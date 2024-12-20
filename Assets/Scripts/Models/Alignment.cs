﻿using System.Collections.Generic;

public class Alignment
{
    public List<TileInstance> gaps;
    public List<ActorInstance> enemies;
    public List<ActorInstance> players;

    public bool hasEnemiesBetween => enemies.Count > 0;
    public bool hasPlayersBetween => players.Count > 0;
    public bool hasGapsBetween => gaps.Count > 0;
}
