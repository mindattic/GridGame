using System.Collections;
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
        StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn()
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null || !enemy.IsAlive || enemy.IsActive || enemy.HasSpawned) continue;

            var unoccupitedTile = Geometry.RandomUnoccupiedTile();
            enemy.location = unoccupitedTile.location;
            enemy.position = unoccupitedTile.position;
            enemy.gameObject.SetActive(true);

            yield return enemy.FadeIn();
            yield return new WaitForSeconds(0.5f);
        }
    }



    void Update()
    {

    }



}
