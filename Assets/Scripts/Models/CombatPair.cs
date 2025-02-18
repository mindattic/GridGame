﻿using System;
using System.Collections.Generic;
using System.Linq;

public class CombatPair
{
    //Fields
    public ActorInstance actor1 = null;
    public ActorInstance actor2 = null;
    public Axis axis = Axis.None;
    public List<TileInstance> gaps = null;
    public List<ActorInstance> opponents = null;
    public List<ActorInstance> allies = null;

    public bool hasOpponentsBetween => opponents?.Count > 0;
    public bool hasAlliesBetween => allies?.Count > 0;
    public bool hasGapsBetween => gaps?.Count > 0;

    /// <summary>
    /// Property that retrieves either the top-most or right-most actor depending upon axial alignment
    /// </summary>
    public ActorInstance startActor
    {
        get
        {
            return (axis == Axis.Vertical)
                ? actor1.location.y > actor2.location.y ? actor1 : actor2
                : actor1.location.x > actor2.location.x ? actor1 : actor2;
        }
    }

    /// <summary>
    /// Property that retrieves either the bottom-most or left-most actor depending upon axial alignment
    /// </summary>
    public ActorInstance endActor
    {
        get
        {
            return (axis == Axis.Vertical)
                ? actor1.location.y < actor2.location.y ? actor1 : actor2
                : actor1.location.x < actor2.location.x ? actor1 : actor2;
        }
    }

    public int sortingOrder
    {
        get
        {
            return actor1.sortingOrder;
        }
        set
        {
            actor1.sortingOrder = value;
            actor2.sortingOrder = value;
            opponents.ForEach(x => x.sortingOrder = value);
            allies.ForEach(x => x.sortingOrder = value);
        }
    }

    public float start => axis == Axis.Vertical ? startActor.location.y : startActor.location.x;
    public float end => axis == Axis.Vertical ? endActor.location.y : endActor.location.x;

    public CombatPair(ActorInstance actor1, ActorInstance actor2, Axis axis)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.axis = axis;
 
        if (axis == Axis.Vertical)
        {
            opponents = GameManager.instance.actors.Where(x => x.isActive && x.isAlive && x.isEnemy && x.IsSameColumn(actor1.location) && AlignmentHelper.IsBetween(x.location.y, end, start)).OrderBy(x => x.location.y).ToList();
            allies = GameManager.instance.actors.Where(x => x.isActive && x.isAlive && x.isPlayer && x.IsSameColumn(actor1.location) && AlignmentHelper.IsBetween(x.location.y, end, start)).OrderBy(x => x.location.y).ToList();
            gaps = GameManager.instance.tiles.Where(x => !x.IsOccupied && actor1.IsSameColumn(x.location) && AlignmentHelper.IsBetween(x.location.y, end, start)).OrderBy(x => x.location.y).ToList();
        }
        else if (axis == Axis.Horizontal)
        {
            opponents = GameManager.instance.actors.Where(x => x.isActive && x.isAlive && x.isEnemy && x.IsSameRow(actor1.location) && AlignmentHelper.IsBetween(x.location.x, end, start)).OrderBy(x => x.location.x).ToList();
            allies = GameManager.instance.actors.Where(x => x.isActive && x.isAlive && x.isPlayer && x.IsSameRow(actor1.location) && AlignmentHelper.IsBetween(x.location.x, end, start)).OrderBy(x => x.location.x).ToList();
            gaps = GameManager.instance.tiles.Where(x => !x.IsOccupied && actor1.IsSameRow(x.location) && AlignmentHelper.IsBetween(x.location.x, end, start)).OrderBy(x => x.location.x).ToList();
        }

    }

    public bool HasPair(ActorInstance actor1, ActorInstance actor2)
    {
        return (this.actor1 == actor1 && this.actor2 == actor2) || (this.actor1 == actor2 && this.actor2 == actor1);
    }

    public void Reset()
    {
        this.actor1 = null;
        this.actor2 = null;
        this.axis = Axis.None;
        gaps = null;
        opponents = null;
        allies = null;
    }


}
