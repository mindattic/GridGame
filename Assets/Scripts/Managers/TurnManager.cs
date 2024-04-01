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

    public void Reset()
    {
        currentTurn = 1;
        currentTeam = Team.Player;
        currentPhase = Phase.Start;

        //titleManager.Print("Battle Start", showOverlay: true);

        musicSource.Stop();
        musicSource.PlayOneShot(resourceManager.MusicTrack($"MelancholyLull"));
    }

    public void NextTurn()
    {
        currentTeam = IsPlayerTurn ? Team.Enemy : Team.Player;
        currentPhase = Phase.Start;

        if (IsPlayerTurn)
            currentTurn++;

        titleManager.Print($"Turn {currentTurn}");

        CheckEnemySpawn();
       
        timer.Set(scale: 1f, start: false);
        soundSource.PlayOneShot(resourceManager.SoundEffect($"NextTurn"));
    }

    private void CheckEnemySpawn()
    {
        foreach (var enemy in enemies.Where(x => x.IsSpawnable))
        {
            enemy.Spawn(unoccupiedTile.location);
        }
    }


    void Update()
    {

    }



}
