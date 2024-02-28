using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Phase = TurnPhase;


public class TurnManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public int turnNumber = 1;
    [SerializeField] public Team currentTurn = Team.Player;
    [SerializeField] public Phase currentPhase = Phase.Start;

    //Properties
    public bool IsPlayerTurn => currentTurn.Equals(Team.Player);
    public bool IsEnemyTurn => currentTurn.Equals(Team.Enemy);
    public bool IsStartPhase => currentPhase.Equals(Phase.Start);
    public bool IsMovePhase => currentPhase.Equals(Phase.Move);
    public bool IsAttackPhase => currentPhase.Equals(Phase.Attack);
    //public bool IsEndPhase => currentPhase.Equals(Phase.End);

    void Awake()
    {
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        turnNumber = 1;
        currentTurn = Team.Player;
        currentPhase = Phase.Start;
    }

    public void NextTurn()
    {
        turnNumber++;
        turnManager.currentTurn = IsPlayerTurn ? Team.Enemy : Team.Player;
        turnManager.currentPhase = Phase.Start;
    }

    void Update()
    {

    }



}
