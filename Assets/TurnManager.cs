using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phase = TurnPhase;

public class TurnManager : ExtendedMonoBehavior
{
    [SerializeField] public Team currentTeam = Team.Player;
    [SerializeField] public Phase currentPhase = Phase.None;

    public void Init(Team team = Team.Player, Phase phase = Phase.Start)
    {
        currentTeam = team;
        currentPhase = phase;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
       
    }

   


}
