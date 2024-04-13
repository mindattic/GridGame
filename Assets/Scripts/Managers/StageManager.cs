using System;
using System.Collections.Generic;
using System.Linq;
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

        //Clear and clear variables
        currentPlayer = null;
        supportLineManager.Clear();
        attackLineManager.Clear();
        turnManager.Reset();
        timer.Set(scale: 1f, start: false);
        actorManager.Clear();
        //overlayManager.Show();
        titleManager.Print($"Stage {currentStage}", showOverlay: true);

        //Clear existing actors
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
        actors.Clear();

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

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            case 2:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            case 3:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            case 4:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                //Dynamic Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, unoccupiedTile.location));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime", attributes, Team.Enemy, unoccupiedTile.location));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", attributes, Team.Enemy, unoccupiedTile.location));
                Add(new StageActor(Archetype.Bat, "Bat", attributes, Team.Enemy, unoccupiedTile.location));
                Add(new StageActor(Archetype.Yeti, "Yeti", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            case 6:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, unoccupiedTile.location));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime", attributes, Team.Enemy, unoccupiedTile.location));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", attributes, Team.Enemy, unoccupiedTile.location));
                Add(new StageActor(Archetype.Bat, "Bat", attributes, Team.Enemy, unoccupiedTile.location));
                Add(new StageActor(Archetype.Yeti, "Yeti", attributes, Team.Enemy, unoccupiedTile.location));

                //Dynamic Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, spawnTurn: 1));
                Add(new StageActor(Archetype.Slime, "Slime B", attributes, Team.Enemy, spawnTurn: 2));
                Add(new StageActor(Archetype.Slime, "Slime C", attributes, Team.Enemy, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime D", attributes, Team.Enemy, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime E", attributes, Team.Enemy, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion A", attributes, Team.Enemy, spawnTurn: 1));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", attributes, Team.Enemy, spawnTurn: 2));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", attributes, Team.Enemy, spawnTurn: 3));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", attributes, Team.Enemy, spawnTurn: 4));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", attributes, Team.Enemy, spawnTurn: 5));
                Add(new StageActor(Archetype.Bat, "Bat A", attributes, Team.Enemy, spawnTurn: 1));
                Add(new StageActor(Archetype.Bat, "Bat B", attributes, Team.Enemy, spawnTurn: 2));
                Add(new StageActor(Archetype.Bat, "Bat C", attributes, Team.Enemy, spawnTurn: 3));
                Add(new StageActor(Archetype.Bat, "Bat D", attributes, Team.Enemy, spawnTurn: 4));
                Add(new StageActor(Archetype.Bat, "Bat E", attributes, Team.Enemy, spawnTurn: 5));
                Add(new StageActor(Archetype.Yeti, "Yeti A", attributes, Team.Enemy, spawnTurn: 1));
                Add(new StageActor(Archetype.Yeti, "Yeti B", attributes, Team.Enemy, spawnTurn: 2));
                Add(new StageActor(Archetype.Yeti, "Yeti C", attributes, Team.Enemy, spawnTurn: 3));
                Add(new StageActor(Archetype.Yeti, "Yeti D", attributes, Team.Enemy, spawnTurn: 4));
                Add(new StageActor(Archetype.Yeti, "Yeti E", attributes, Team.Enemy, spawnTurn: 5));

                break;

            case 7:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;


            case 8:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            case 9:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            case 10:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

                break;

            default:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, unoccupiedTile.location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, unoccupiedTile.location));

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
        actor.guid = Guid.NewGuid();
        actor.thumbnail = stageActor.thumbnail;
        actor.team = stageActor.team;
        actor.render.SetBackColor(actor.IsPlayer ? Color.white : Color.red);

        //Assign attributes
        actor.Level = stageActor.attributes.Level;
        actor.MaxHP = stageActor.attributes.MaxHP;
        actor.HP = stageActor.attributes.HP;
        actor.Attack = stageActor.attributes.Attack;
        actor.Defense = stageActor.attributes.Defense;
        actor.Accuracy = stageActor.attributes.Accuracy;
        actor.Evasion = stageActor.attributes.Evasion;
        actor.Speed = stageActor.attributes.Speed;
        actor.Luck = stageActor.attributes.Luck;

        if (stageActor.IsSpawning)
        {
            actor.location = stageActor.location;
            actor.Init(spawn: true);
        }
        else
        {
            actor.spawnTurn = stageActor.spawnTurn;
            actor.Init(spawn: false);
        }


        actors.Add(actor);
    }



}
