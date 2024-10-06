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

        //DespawnAll and clear variables
        focusedActor = null;
        selectedPlayer = null;
        supportLineManager.Clear();
        attackLineManager.Clear();
        turnManager.Reset();
        timer.Reset();
        actorManager.Clear();
        //overlayManager.Show();
        overlayManager.Show();
        titleManager.Print($"Stage {currentStage}");

        //DespawnAll existing actors
        actorManager.Clear();

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:



                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;

            case 2:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;

            case 3:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;

            case 4:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Uncommon));
                Add(new StageActor(Archetype.Cleric, "Cleric", Formulas.RandomStats(10), Team.Player, Qualities.Common));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime F", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime G", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                Add(new StageActor(Archetype.Scorpion, "Scorpion A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                Add(new StageActor(Archetype.Bat, "Bat A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Bat, "Bat B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                Add(new StageActor(Archetype.Yeti, "Yeti A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime H", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime I", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime J", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Scorpion, "Scorpion F", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Bat, "Bat C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 7));
                Add(new StageActor(Archetype.Bat, "Bat D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 8));
                Add(new StageActor(Archetype.Bat, "Bat E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 9));
                Add(new StageActor(Archetype.Yeti, "Yeti B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 10));

                break;

            case 6:

                //players
                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Cleric, "Cleric", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Ninja, "Ninja", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", Formulas.RandomStats(10), Team.Player, Qualities.Common));

                //enemies
                Add(new StageActor(Archetype.Slime, "Slime", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Bat, "Bat", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Yeti, "Yeti", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Slime, "Slime B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Slime, "Slime C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Bat, "Bat A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Bat, "Bat B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Bat, "Bat C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Bat, "Bat D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Bat, "Bat E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Yeti, "Yeti A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Yeti, "Yeti B", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Yeti, "Yeti C", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Yeti, "Yeti D", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Yeti, "Yeti E", Formulas.RandomStats(10), Team.Enemy, Qualities.Common, spawnTurn: 5));

                break;

            case 7:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;


            case 8:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;

            case 9:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;

            case 10:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;

            default:

                Add(new StageActor(Archetype.Paladin, "Paladin", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", Formulas.RandomStats(10), Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", Formulas.RandomStats(10), Team.Enemy, Qualities.Common));

                break;
        }

    }

    public void Add(StageActor stageActor)
    {
        var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        var actor = prefab.GetComponent<ActorBehavior>();
        actor.parent = board.transform;
        actor.archetype = stageActor.archetype;
        actor.name = stageActor.name;
        actor.thumbnail = stageActor.thumbnail;
        actor.team = stageActor.team;
        actor.quality = stageActor.quality;
        actor.renderers.SetQualityColor(actor.IsPlayer ? Color.white : Color.red);
        actor.sortingOrder = SortingOrder.Min;

        //Assign stats
        actor.stats = stageActor.stats;
        actor.transform.localScale = tileScale;

        if (stageActor.IsSpawning)
        {
            actor.Spawn(stageActor.location);
        }
        else
        {
            actor.spawnTurn = stageActor.spawnTurn;
            actor.gameObject.SetActive(false);
        }

        actors.Add(actor);
    }



}
