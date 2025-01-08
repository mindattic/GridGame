using System.Collections.Generic;

public class Alignment
{
    public List<TileInstance> gaps;
    public List<ActorInstance> opponents;
    public List<ActorInstance> players;

    public bool hasOpponentsBetween => opponents.Count > 0;
    public bool hasAlliesBetween => players.Count > 0;
    public bool hasGapsBetween => gaps.Count > 0;
}
