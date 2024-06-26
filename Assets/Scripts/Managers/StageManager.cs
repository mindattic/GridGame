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

        //Clear and clear variables
        SelectedPlayer = null;
        SupportLineManager.Clear();
        AttackLineManager.Clear();
        TurnManager.Reset();
        Timer.Set(scaleX: 1f, start: false);
        ActorManager.Clear();
        //OverlayManager.Show();
        TitleManager.Print($"Stage {currentStage}", showOverlay: true);

        //Clear existing Actors
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
        Actors.Clear();

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

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;

            case 2:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;

            case 3:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;

            case 4:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                //Dynamic Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, spawnTurn: 1));

                break;

            case 5:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Rare, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Uncommon, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Bat, "Bat", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Yeti, "Yeti", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                //Dynamic Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Slime, "Slime B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 8));
                Add(new StageActor(Archetype.Slime, "Slime C", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 9));
                Add(new StageActor(Archetype.Scorpion, "Scorpion A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 7));
                Add(new StageActor(Archetype.Bat, "Bat A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Bat, "Bat B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 6));
                Add(new StageActor(Archetype.Bat, "Bat C", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 8));
                Add(new StageActor(Archetype.Yeti, "Yeti A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 9));

                break;

            case 6:

                //Players
                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));

                //Enemies
                Add(new StageActor(Archetype.Slime, "Slime", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Scorpion, "Scorpion", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Bat, "Bat", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Yeti, "Yeti", RandomAttributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                //Dynamic Enemies
                Add(new StageActor(Archetype.Slime, "Slime A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Slime, "Slime B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Slime, "Slime C", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Slime, "Slime D", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Slime, "Slime E", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Scorpion, "Scorpion A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Scorpion, "Scorpion B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Scorpion, "Scorpion C", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Scorpion, "Scorpion D", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Scorpion, "Scorpion E", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Bat, "Bat A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Bat, "Bat B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Bat, "Bat C", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Bat, "Bat D", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Bat, "Bat E", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 5));
                Add(new StageActor(Archetype.Yeti, "Yeti A", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 1));
                Add(new StageActor(Archetype.Yeti, "Yeti B", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 2));
                Add(new StageActor(Archetype.Yeti, "Yeti C", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 3));
                Add(new StageActor(Archetype.Yeti, "Yeti D", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 4));
                Add(new StageActor(Archetype.Yeti, "Yeti E", RandomAttributes, Team.Enemy, Colors.Common, spawnTurn: 5));

                break;

            case 7:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;


            case 8:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;

            case 9:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;

            case 10:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;

            default:

                Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, Colors.Common, UnoccupiedTile.Location));
                Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, Colors.Common, UnoccupiedTile.Location));

                break;
        }

    }

    public void Add(StageActor stageActor)
    {
        var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        var actor = prefab.GetComponent<ActorBehavior>();
        actor.parent = Board.transform;
        actor.Archetype = stageActor.archetype;
        actor.name = stageActor.name;
        actor.Guid = Guid.NewGuid();
        actor.thumbnail = stageActor.thumbnail;
        actor.Team = stageActor.team;
        actor.Quality = stageActor.rarity;
        actor.Renderers.SetBackColor(actor.IsPlayer ? Color.white : Color.red);
        actor.sortingOrder = ZAxis.Min;

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
            actor.Location = stageActor.location;
            actor.Init(spawn: true);
        }
        else
        {
            actor.SpawnTurn = stageActor.spawnTurn;
            actor.Init(spawn: false);
        }


        Actors.Add(actor);
    }



}
