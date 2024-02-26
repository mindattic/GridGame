using UnityEngine;
using Phase = TurnPhase;

public class TurnManager : ExtendedMonoBehavior
{
    [SerializeField] public Team currentTeam = Team.Player;
    [SerializeField] public Phase currentPhase = Phase.Start;
    [SerializeField] public int turnNumber = 1;

    public bool IsPlayerTurn => currentTeam.Equals(Team.Player);
    public bool IsEnemyTurn => currentTeam.Equals(Team.Player);

    public bool IsStartPhase => currentPhase.Equals(Phase.Start);

    public bool IsMovePhase => currentPhase.Equals(Phase.Move);
    public bool IsAttackPhase => currentPhase.Equals(Phase.Attack);
    public bool IsEndPhase => currentPhase.Equals(Phase.End);


    void Awake()
    {
   
    }

    void Start()
    {
        //announcementManager.Show($"Battle Start");
    }


    public void NextTurn()
    {
        currentTeam = IsPlayerTurn ? Team.Enemy : currentTeam = Team.Player;
        currentPhase = Phase.Start;
        announcementManager.Show($"Turn {turnNumber++}");
    }

    public void NextPhase()
    {
        if (currentPhase.Equals(Phase.End))
            NextTurn();

        currentPhase = currentPhase.Next();
    }

    void Update()
    {

    }

}
