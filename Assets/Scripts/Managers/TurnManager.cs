using System.Linq;
using UnityEngine;
using Phase = TurnPhase;

public class TurnManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public int currentTurn = 1;
    [SerializeField] public Team currentTeam = Team.Player;
    [SerializeField] public Phase currentPhase = Phase.Start;

    //Properties
    public bool IsPlayerTurn => currentTeam.Equals(Team.Player);
    public bool IsEnemyTurn => currentTeam.Equals(Team.Enemy);
    public bool IsStartPhase => currentPhase.Equals(Phase.Start);
    public bool IsMovePhase => currentPhase.Equals(Phase.Move);
    public bool IsAttackPhase => currentPhase.Equals(Phase.Attack);

    public bool IsFirstTurn => currentTurn == 1;


    void Awake()
    {
    }

    void Start()
    {
        Reset();
    }

    void Update() {
    
    }

    void FixedUpdate() { 
    
    }

    public void Reset()
    {
        currentTurn = 1;
        currentTeam = Team.Player;
        currentPhase = Phase.Start;

        musicSource.Stop();
        musicSource.PlayOneShot(resourceManager.MusicTrack($"MelancholyLull"));
    }

    public void NextTurn()
    {
        currentTeam = IsPlayerTurn ? Team.Enemy : Team.Player;
        currentPhase = Phase.Start;

        actors.ForEach(x => x.sortingOrder = ZAxis.Min);


        if (IsPlayerTurn)
        {
            currentTurn++;
            titleManager.Print($"Turn {currentTurn}");
            timer.Set(scaleX: 1f, start: false);
            //players.Where(x => x.IsPlaying).ToList().ForEach(x => StartCoroutine(x.GlowIn()));
            //enemies.Where(x => x.IsPlaying).ToList().ForEach(x => StartCoroutine(x.GlowOut()));
        }
        else if (IsEnemyTurn)
        {
            actorManager.CheckEnemySpawn();
            actorManager.CheckEnemyMove();
            //enemies.Where(x => x.IsPlaying && x.IsReady).ToList().ForEach(x => StartCoroutine(x.GlowIn()));
            //players.Where(x => x.IsPlaying).ToList().ForEach(x => StartCoroutine(x.GlowOut()));
        }

        audioManager.Play("NextTurn");
    }


}
