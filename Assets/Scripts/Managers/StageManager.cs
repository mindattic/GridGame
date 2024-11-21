using Assets.Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : ExtendedMonoBehavior
{
    [SerializeField] public int currentStage;
    [SerializeField] public GameObject actorPrefab;

    void Start()
    {

    }

    void Update()
    {

    }

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
        currentStage = saveFileManager.currentSaveFile.CurrentStage;
        totalCoins = saveFileManager.currentSaveFile.TotalCoins;
 
        focusedActor = null;
        selectedPlayer = null;
        coinBar.Refresh();
        supportLineManager.Clear();
        attackLineManager.Clear();
        turnManager.Reset();
        timerBar.Reset();
        actorManager.Clear();
        overlayManager.Show();
        titleManager.Print($"Stage {currentStage}");

        //DespawnAll existing actors
        actorManager.Clear();

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:



                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;

            case 2:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;

            case 3:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;

            case 4:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Cleric, "Cleric", RandomStats, Team.Player, Rarity.Common));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Slime, "Slime B", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Slime, "Slime C", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Slime, "Slime D", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Slime, "Slime E", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Slime, "Slime F", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Slime, "Slime G", RandomStats, Team.Enemy, Rarity.Common));

                Add(new StageActor(Archetype.Scorpion, "Scorpion A", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", RandomStats, Team.Enemy, Rarity.Common));

                Add(new StageActor(Archetype.Bat, "Bat A", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Bat, "Bat B", RandomStats, Team.Enemy, Rarity.Common));

                Add(new StageActor(Archetype.Yeti, "Yeti A", RandomStats, Team.Enemy, Rarity.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime H", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime I", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime J", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Scorpion, "Scorpion F", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Bat, "Bat C", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 7));
                Add(new StageActor(Archetype.Bat, "Bat D", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 8));
                Add(new StageActor(Archetype.Bat, "Bat E", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 9));
                Add(new StageActor(Archetype.Yeti, "Yeti B", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 10));

                break;

            case 6:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Cleric, "Cleric", RandomStats, Team.Player, Rarity.Common));
                Add(new StageActor(Archetype.Ninja, "Ninja", RandomStats, Team.Player, Rarity.Common));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", RandomStats, Team.Player, Rarity.Common));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", RandomStats, Team.Player, Rarity.Common));

                //enemies
                Add(new StageActor(Archetype.Slime, "Slime", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Bat, "Bat", RandomStats, Team.Enemy, Rarity.Common));
                Add(new StageActor(Archetype.Yeti, "Yeti", RandomStats, Team.Enemy, Rarity.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Slime, "Slime B", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Slime, "Slime C", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime D", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime E", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion A", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Bat, "Bat A", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Bat, "Bat B", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Bat, "Bat C", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Bat, "Bat D", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Bat, "Bat E", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Yeti, "Yeti A", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Yeti, "Yeti B", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Yeti, "Yeti C", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Yeti, "Yeti D", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Yeti, "Yeti E", RandomStats, Team.Enemy, Rarity.Common, spawnTurn: 5));

                break;

            case 7:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;


            case 8:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;

            case 9:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;

            case 10:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;

            default:

                Add(new StageActor(Archetype.Paladin, "Paladin", RandomStats, Team.Player, Rarity.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", RandomStats, Team.Player, Rarity.Uncommon));
                Add(new StageActor(Archetype.Slime, "Slime A", RandomStats, Team.Enemy, Rarity.Common));

                break;
        }

    }

    public void Add(StageActor stageActor)
    {
        var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<ActorInstance>();
        instance.parent = board.transform;
        instance.archetype = stageActor.archetype;
        instance.name = stageActor.name;
        instance.thumbnail = stageActor.thumbnail;
        instance.team = stageActor.team;
        instance.quality = stageActor.quality;
        instance.renderers.SetQualityColor(instance.IsPlayer ? Color.white : Color.red);
        instance.sortingOrder = SortingOrder.Min;

        //Assign stats
        instance.stats = stageActor.stats;
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
