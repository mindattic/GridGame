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

        musicSource.PlayOneShot(resourceManager.MusicTrack($"MelancholyLull"));
    }

    public void NextTurn()
    {
        turnNumber++;
        turnManager.currentTurn = IsPlayerTurn ? Team.Enemy : Team.Player;
        turnManager.currentPhase = Phase.Start;

        timer.Set(scale: 1f, start: false);
        soundSource.PlayOneShot(resourceManager.SoundEffect($"NextTurn"));
    }

    void Update()
    {

    }



}
