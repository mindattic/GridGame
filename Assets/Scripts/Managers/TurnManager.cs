using UnityEngine;
using Phase = TurnPhase;

public class TurnManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public int turnNumber = 1;
    [SerializeField] public Team currentTeam = Team.Player;
    [SerializeField] public Phase currentPhase = Phase.Start;

    //Properties
    public bool IsPlayerTurn => currentTeam.Equals(Team.Player);
    public bool IsEnemyTurn => currentTeam.Equals(Team.Enemy);
    public bool IsStartPhase => currentPhase.Equals(Phase.Start);
    public bool IsMovePhase => currentPhase.Equals(Phase.Move);
    public bool IsAttackPhase => currentPhase.Equals(Phase.Attack);

    public bool IsFirstTurn => turnNumber == 1;


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
        currentTeam = Team.Player;
        currentPhase = Phase.Start;

        overlayManager.FadeIn();
        titleManager.Print("Battle Start");

        musicSource.Stop();
        musicSource.PlayOneShot(resourceManager.MusicTrack($"MelancholyLull"));
    }

    public void NextTurn()
    {
        currentTeam = IsPlayerTurn ? Team.Enemy : Team.Player;
        currentPhase = Phase.Start;

        if (IsPlayerTurn)
            turnNumber++;

        titleManager.Print($"Turn {turnNumber}");

        CheckEnemySpawn();
       
        timer.Set(scale: 1f, start: false);
        soundSource.PlayOneShot(resourceManager.SoundEffect($"NextTurn"));
    }

    private void CheckEnemySpawn()
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null || !enemy.IsAlive || enemy.IsActive || enemy.HasSpawned) continue;

            var tile = unoccupiedTile;
            enemy.location = tile.location;
            enemy.position = tile.position;
            enemy.render.SetColor(new Color(1, 1, 1, 0));
            enemy.gameObject.SetActive(true);

            float delay = Random.Float(0f, 1f);
            StartCoroutine(enemy.FadeIn(delay));
        }
    }






    void Update()
    {

    }



}
