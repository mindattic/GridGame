using Assets.Scripts.Utilities;
using Game.Behaviors;
using Game.Manager;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Properties
    protected DataManager dataManager => GameManager.instance.dataManager;
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
    protected DottedLineManager dottedLineManager => GameManager.instance.dottedLineManager;



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
        //totalCoins = profileManager.currentProfile.Global.TotalCoins;
        //currentStage = profileManager.currentProfile.Stage.CurrentStage;

        focusedActor = null;
        selectedPlayer = null;
        coinBar.Refresh();
        supportLineManager.Clear();
        attackLineManager.Clear();
        turnManager.Reset();
        timerBar.TriggerInitialize();
        actorManager.Clear();
        canvasOverlay.Reset();
        dottedLineManager.Clear();

        canvasOverlay.Show($"Stage {currentStage}");
        canvasOverlay.TriggerFadeOut(delay: Interval.ThreeSeconds);
        //titleManager.TriggerFadeIn($"Stage {currentStage}");

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:

                Add(new StageActor(Character.Paladin, Team.Player, new Vector2Int(2, 7)));
                Add(new StageActor(Character.Slime, Team.Enemy, new Vector2Int(5, 6)));
                Add(new StageActor(Character.Barbarian, Team.Player, new Vector2Int(4, 5)));

                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(2, 3));
                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(2, 4));
                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(2, 5));
                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(2, 6));
                dottedLineManager.Spawn(DottedLineSegment.TurnBottomRight, new Vector2Int(2, 2));
                dottedLineManager.Spawn(DottedLineSegment.Horizontal, new Vector2Int(3, 2));
                dottedLineManager.Spawn(DottedLineSegment.Horizontal, new Vector2Int(4, 2));
                dottedLineManager.Spawn(DottedLineSegment.TurnBottomLeft, new Vector2Int(5, 2));
                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(5, 3));
                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(5, 4));
                dottedLineManager.Spawn(DottedLineSegment.Vertical, new Vector2Int(5, 5));
                dottedLineManager.Spawn(DottedLineSegment.TurnTopRight, new Vector2Int(5, 6));
                dottedLineManager.Spawn(DottedLineSegment.TurnTopLeft, new Vector2Int(6, 6));
                dottedLineManager.Spawn(DottedLineSegment.ArrowUp, new Vector2Int(6, 5));

                break;

            case 2:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;

            case 3:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;

            case 4:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                //Dynamic opponents
                Add(new StageActor(Character.Slime, Team.Enemy, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Cleric, Team.Player));
                Add(new StageActor(Character.Ninja, Team.Player));

                //Enemies
                Add(new StageActor(Character.Yeti, Team.Enemy));

                break;

            case 6:

                //Players
                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Cleric, Team.Player));

                //Enemies
                Add(new StageActor(Character.Slime, Team.Enemy));
                Add(new StageActor(Character.Slime, Team.Enemy));
                Add(new StageActor(Character.Slime, Team.Enemy));
                Add(new StageActor(Character.Slime, Team.Enemy));
                Add(new StageActor(Character.Slime, Team.Enemy));
                Add(new StageActor(Character.Slime, Team.Enemy));
                Add(new StageActor(Character.Slime, Team.Enemy));

                Add(new StageActor(Character.Scorpion, Team.Enemy));
                Add(new StageActor(Character.Scorpion, Team.Enemy));
                Add(new StageActor(Character.Scorpion, Team.Enemy));
                Add(new StageActor(Character.Scorpion, Team.Enemy));

                Add(new StageActor(Character.Bat, Team.Enemy));
                Add(new StageActor(Character.Bat, Team.Enemy));

                Add(new StageActor(Character.Yeti, Team.Enemy));

                //Dynamic opponents
                Add(new StageActor(Character.Slime, Team.Enemy, spawnTurn: 3));
                Add(new StageActor(Character.Slime, Team.Enemy, spawnTurn: 4));
                Add(new StageActor(Character.Slime, Team.Enemy, spawnTurn: 5));
                Add(new StageActor(Character.Scorpion, Team.Enemy, spawnTurn: 6));
                Add(new StageActor(Character.Scorpion, Team.Enemy, spawnTurn: 6));
                Add(new StageActor(Character.Bat, Team.Enemy, spawnTurn: 7));
                Add(new StageActor(Character.Bat, Team.Enemy, spawnTurn: 8));
                Add(new StageActor(Character.Bat, Team.Enemy, spawnTurn: 9));
                Add(new StageActor(Character.Yeti, Team.Enemy, spawnTurn: 10));

                break;

            case 7:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;


            case 8:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;

            case 9:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;

            case 10:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;

            default:

                Add(new StageActor(Character.Paladin, Team.Player));
                Add(new StageActor(Character.Barbarian, Team.Player));
                Add(new StageActor(Character.Slime, Team.Enemy));

                break;
        }

    }

    public void Add(StageActor stageActor)
    {
        var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<ActorInstance>();
        instance.parent = board.transform;
        instance.character = stageActor.character;
        instance.name = stageActor.character.ToString();
        //instance.thumbnailSettings = stageActor.thumbnailSettings;
        instance.team = stageActor.team;
        //instance.quality = stageActor.quality;
        //instance.render.SetQualityColor(instance.isPlayer ? Color.white : Color.red);
        //instance.sortingOrder = SortingOrder.Min;

        //Assign Stats
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



    public void AddEnemy(Character character)
    {
        Add(new StageActor(character, Team.Enemy, Random.UnoccupiedLocation));
    }

}
