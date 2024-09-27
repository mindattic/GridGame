using Game.Behaviors.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

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


    private ActorAttributes RandomAttributes => new ActorAttributes()
    {
        Level = 1,
        MaxHP = 100,
        HP = 100,
        Attack = Random.Int(5, 10),
        Defense = Random.Int(5, 10),
        Accuracy = Random.Int(5, 10),
        Evasion = Random.Int(5, 10),
        Speed = Random.Int(5, 10),
        Luck = Random.Int(5, 10)
    };

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

        var attributes = new ActorAttributes()
        {
            Level = 1,
            MaxHP = 100,
            HP = 100,
            Attack = 10,
            Defense = 10,
            Accuracy = 5,
            Evasion = 5,
            Speed = 10,
            Luck = 10
        };

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;

            case 2:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;

            case 3:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;

            case 4:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Rare));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Uncommon));
                Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, Qualities.Common));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime B", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime C", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime D", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime E", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime F", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime G", RandomAttributes, Team.Enemy, Qualities.Common));

                Add(new StageActor(Archetype.Scorpion, "Scorpion A", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", RandomAttributes, Team.Enemy, Qualities.Common));

                Add(new StageActor(Archetype.Bat, "Bat A", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Bat, "Bat B", RandomAttributes, Team.Enemy, Qualities.Common));

                Add(new StageActor(Archetype.Yeti, "Yeti A", RandomAttributes, Team.Enemy, Qualities.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime H", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime I", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime J", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Scorpion, "Scorpion F", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Bat, "Bat C", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 7));
                Add(new StageActor(Archetype.Bat, "Bat D", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 8));
                Add(new StageActor(Archetype.Bat, "Bat E", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 9));
                Add(new StageActor(Archetype.Yeti, "Yeti B", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 10));

                break;

            case 6:

                //players
                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, Qualities.Common));

                //enemies
                Add(new StageActor(Archetype.Slime, "Slime", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Bat, "Bat", RandomAttributes, Team.Enemy, Qualities.Common));
                Add(new StageActor(Archetype.Yeti, "Yeti", RandomAttributes, Team.Enemy, Qualities.Common));

                //Dynamic enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Slime, "Slime B", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Slime, "Slime C", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime D", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime E", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion A", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Bat, "Bat A", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Bat, "Bat B", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Bat, "Bat C", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Bat, "Bat D", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Bat, "Bat E", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Yeti, "Yeti A", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Yeti, "Yeti B", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Yeti, "Yeti C", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Yeti, "Yeti D", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Yeti, "Yeti E", RandomAttributes, Team.Enemy, Qualities.Common, spawnTurn: 5));

                break;

            case 7:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;


            case 8:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;

            case 9:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;

            case 10:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;

            default:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Qualities.Common));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Qualities.Common));

                break;
        }

    }

    public void Add(StageActor stageActor)
    {
        var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        var actor = prefab.GetComponent<ActorBehavior>();
        actor.Parent = board.transform;
        actor.archetype = stageActor.archetype;
        actor.name = stageActor.name;
        actor.thumbnail = stageActor.thumbnail;
        actor.team = stageActor.team;
        actor.quality = stageActor.quality;
        actor.renderers.SetQualityColor(actor.IsPlayer ? Color.white : Color.red);
        actor.sortingOrder = SortingOrder.Min;

        //Assign attributes
        actor.level = stageActor.attributes.Level;
        actor.maxHp = stageActor.attributes.MaxHP;
        actor.hp = stageActor.attributes.HP;
        actor.attack = stageActor.attributes.Attack;
        actor.defense = stageActor.attributes.Defense;
        actor.accuracy = stageActor.attributes.Accuracy;
        actor.evasion = stageActor.attributes.Evasion;
        actor.speed = stageActor.attributes.Speed;
        actor.luck = stageActor.attributes.Luck;
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
