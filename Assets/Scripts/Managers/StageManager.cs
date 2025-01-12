using Assets.Scripts.Utilities;
using Game.Behaviors;
using Game.Manager;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Properties
    protected DatabaseManager databaseManager => GameManager.instance.databaseManager;
    protected int totalCoins
    {
        get { return GameManager.instance.totalCoins; }
        set { GameManager.instance.totalCoins = value; }
    }
    protected ProfileManager profileManager => GameManager.instance.profileManager;
    protected ActorInstance focusedActor
    {
        get { return GameManager.instance.focusedActor; }
        set { GameManager.instance.focusedActor = value; }
    }
    protected ActorInstance selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }
    protected CoinBarInstance coinBar => GameManager.instance.coinBar;
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected TurnManager turnManager => GameManager.instance.turnManager;
    protected TimerBarInstance timerBar => GameManager.instance.timerBar;
    protected ActorManager actorManager => GameManager.instance.actorManager;

    protected CanvasOverlay canvasOverlay => GameManager.instance.canvasOverlay;
    protected Vector3 tileScale => GameManager.instance.tileScale;

    protected List<ActorInstance> actors
    {
        get => GameManager.instance.actors;
        set => GameManager.instance.actors = value;
    }
    protected BoardInstance board => GameManager.instance.board;

    #endregion

    //Variables
    [SerializeField] public int currentStage;
    [SerializeField] public GameObject actorPrefab;
    private ActorStats RandomStats => Formulas.RandomStats(10);

    private ActorStats ActorStats(string name)
    {
        return databaseManager.GetActorStats(name);
    }

    public void PreviousStage()
    {
        if (currentStage > 1) currentStage--;
        Load();
    }

    public void NextStage()
    {
        currentStage++;
        Load();
    }

    public void Load()
    {
        totalCoins = profileManager.currentProfile.Global.TotalCoins;
        currentStage = profileManager.currentProfile.Stage.CurrentStage;

        focusedActor = null;
        selectedPlayer = null;
        coinBar.Refresh();
        supportLineManager.Clear();
        attackLineManager.Clear();
        turnManager.Reset();
        timerBar.TriggerResetUI();
        actorManager.Clear();
        actorManager.Clear();

        canvasOverlay.Show($"Stage {currentStage}");
        canvasOverlay.TriggerFadeOut(delay: Interval.ThreeSeconds);
        //titleManager.TriggerFadeIn($"Stage {currentStage}");

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:



                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;

            case 2:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;

            case 3:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;

            case 4:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                //Dynamic opponents
                Add(new StageActor(Character.Slime, "Slime B", ActorStats("Slime"), Team.Enemy, Rarity.Common, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Cleric, "Cleric", RandomStats, Team.Player, Rarity.Common));
                Add(new StageActor(Character.Ninja, "Ninja", RandomStats, Team.Player, Rarity.Common));

                //Enemies
                //Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Opponent, Rarity.Common));
                //Add(new StageActor(Character.Slime, "Slime B", ActorStats("Slime"), Team.Opponent, Rarity.Common));
                //Add(new StageActor(Character.Slime, "Slime C", ActorStats("Slime"), Team.Opponent, Rarity.Common));
                Add(new StageActor(Character.Yeti, "Yeti A", ActorStats("Scorpion"), Team.Enemy, Rarity.Common));

                break;

            case 6:

                //Players
                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Cleric, "Cleric", RandomStats, Team.Player, Rarity.Common));

                //Enemies
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Slime, "Slime B", ActorStats("Slime"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Slime, "Slime C", ActorStats("Slime"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Slime, "Slime D", ActorStats("Slime"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Slime, "Slime E", ActorStats("Slime"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Slime, "Slime F", ActorStats("Slime"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Slime, "Slime G", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                Add(new StageActor(Character.Scorpion, "Scorpion A", ActorStats("Scorpion"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Scorpion, "Scorpion B", ActorStats("Scorpion"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Scorpion, "Scorpion C", ActorStats("Scorpion"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Scorpion, "Scorpion D", ActorStats("Scorpion"), Team.Enemy, Rarity.Common));

                Add(new StageActor(Character.Bat, "Bat A", ActorStats("Bat"), Team.Enemy, Rarity.Common));
                Add(new StageActor(Character.Bat, "Bat B", ActorStats("Bat"), Team.Enemy, Rarity.Common));

                Add(new StageActor(Character.Yeti, "Yeti A", ActorStats("Yeti"), Team.Enemy, Rarity.Common));

                //Dynamic opponents
                Add(new StageActor(Character.Slime, "Slime H", ActorStats("Slime"), Team.Enemy, Rarity.Common, spawnTurn: 3));
                Add(new StageActor(Character.Slime, "Slime I", ActorStats("Slime"), Team.Enemy, Rarity.Common, spawnTurn: 4));
                Add(new StageActor(Character.Slime, "Slime J", ActorStats("Slime"), Team.Enemy, Rarity.Common, spawnTurn: 5));
                Add(new StageActor(Character.Scorpion, "Scorpion E", ActorStats("Scorpion"), Team.Enemy, Rarity.Common, spawnTurn: 6));
                Add(new StageActor(Character.Scorpion, "Scorpion F", ActorStats("Scorpion"), Team.Enemy, Rarity.Common, spawnTurn: 6));
                Add(new StageActor(Character.Bat, "Bat C", ActorStats("Bat"), Team.Enemy, Rarity.Common, spawnTurn: 7));
                Add(new StageActor(Character.Bat, "Bat D", ActorStats("Bat"), Team.Enemy, Rarity.Common, spawnTurn: 8));
                Add(new StageActor(Character.Bat, "Bat E", ActorStats("Bat"), Team.Enemy, Rarity.Common, spawnTurn: 9));
                Add(new StageActor(Character.Yeti, "Yeti B", ActorStats("Yeti"), Team.Enemy, Rarity.Common, spawnTurn: 10));

                break;

            case 7:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;


            case 8:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;

            case 9:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;

            case 10:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;

            default:

                Add(new StageActor(Character.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Character.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Character.Slime, "Slime A", ActorStats("Slime"), Team.Enemy, Rarity.Common));

                break;
        }

    }

    public void Add(StageActor stageActor)
    {
        var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<ActorInstance>();
        instance.parent = board.transform;
        instance.character = stageActor.character;
        instance.name = stageActor.name;
        //instance.thumbnail = stageActor.thumbnail;
        instance.team = stageActor.team;
        instance.quality = stageActor.quality;
        instance.render.SetQualityColor(instance.isPlayer ? Color.white : Color.red);
        instance.sortingOrder = SortingOrder.Min;

        //Assign stats
        instance.stats = new ActorStats(stageActor.stats);
        instance.transform.localScale = tileScale;

        if (stageActor.IsSpawning)
        {
            instance.Spawn(stageActor.location);
        }
        else
        {
            instance.spawnDelay = stageActor.spawnTurn;
            instance.gameObject.SetActive(false);
        }

        actors.Add(instance);
    }



}
