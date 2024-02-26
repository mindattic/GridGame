using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Phase = TurnPhase;


public class TurnManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public int turnNumber = 1;
    [SerializeField] public Team activeTeam = Team.Player;
    [SerializeField] public Phase phase = Phase.Start;
    
    //Properties
    public bool IsPlayerActive => activeTeam.Equals(Team.Player);
    public bool IsEnemyActive => activeTeam.Equals(Team.Enemy);
    public bool IsStartPhase => phase.Equals(Phase.Start);
    public bool IsMovePhase => phase.Equals(Phase.Move);
    public bool IsAttackPhase => phase.Equals(Phase.Attack);
    public bool IsEndPhase => phase.Equals(Phase.End);

    void Awake()
    {
    }

    void Start()
    {
        Play();  
    }

    public void Reset()
    {
        turnNumber = 1;
        activeTeam = Team.Player;
        phase = Phase.Start;
        Play();
    }

    public void Play()
    {
        titleManager.text = turnNumber == 1 ? "Battle Start" : $"Turn {turnNumber}";
        StartCoroutine(FadeInOut());
    }


    IEnumerator FadeInOut()
    {

        yield return this.WaitAll(overlayManager.FadeInOut(), titleManager.FadeInOut());

        turnManager.phase = Phase.Move;
    }

    public void NextTurn()
    {
        turnNumber++;
        turnManager.activeTeam = IsPlayerActive ? Team.Enemy : Team.Player;
        turnManager.phase = TurnPhase.Start;

        Play();
    }




    void Update()
    {
        



    }



}
