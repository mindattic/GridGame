using System;
using System.Collections;
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
        if (currentStage > 1)
            currentStage--;
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
        selectedPlayer = null;
        supportLineManager.Clear();
        attackLineManager.Clear();
        turnManager.Reset();
        timer.Set(scale: 1f, start: false);
        actorManager.Clear();
        overlayManager.Show();
        titleManager.Print($"Stage {currentStage}");

        var attributes = new ActorAttributes() { HP = 100, MaxHP = 100 };
        int i = 0;
        var onoccupiedLocation = Common.RandomLocations();

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 2:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 3:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 4:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 5:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime", attributes, Team.Enemy, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Scorpion, "Scorpion", attributes, Team.Enemy, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Bat, "Bat", attributes, Team.Enemy, onoccupiedLocation[i++]));

                //Add enemies randomly to stage
                var enemies = new List<StageActor>
                {
                    new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, spawnTurn: 1),
                    new StageActor(Archetype.Slime, "Slime B", attributes, Team.Enemy, spawnTurn: 2),
                    new StageActor(Archetype.Slime, "Slime C", attributes, Team.Enemy, spawnTurn: 3),
                    new StageActor(Archetype.Slime, "Slime D", attributes, Team.Enemy, spawnTurn: 4),
                    new StageActor(Archetype.Slime, "Slime E", attributes, Team.Enemy, spawnTurn: 5),
                    new StageActor(Archetype.Scorpion, "Scorpion A", attributes, Team.Enemy, spawnTurn: 1),
                    new StageActor(Archetype.Scorpion, "Scorpion B", attributes, Team.Enemy, spawnTurn: 2),
                    new StageActor(Archetype.Scorpion, "Scorpion C", attributes, Team.Enemy, spawnTurn: 3),
                    new StageActor(Archetype.Scorpion, "Scorpion D", attributes, Team.Enemy, spawnTurn: 4),
                    new StageActor(Archetype.Scorpion, "Scorpion E", attributes, Team.Enemy, spawnTurn: 5),
                    new StageActor(Archetype.Bat, "Bat A", attributes, Team.Enemy, spawnTurn: 1),
                    new StageActor(Archetype.Bat, "Bat B", attributes, Team.Enemy, spawnTurn: 2),
                    new StageActor(Archetype.Bat, "Bat C", attributes, Team.Enemy, spawnTurn: 3),
                    new StageActor(Archetype.Bat, "Bat D", attributes, Team.Enemy, spawnTurn: 4),
                    new StageActor(Archetype.Bat, "Bat E", attributes, Team.Enemy, spawnTurn: 5)
                };
                stageActors.AddRange(enemies.OrderBy(x => Guid.NewGuid()));

                break;


            case 6:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 7:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;


            case 8:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 9:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            case 10:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;

            default:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, onoccupiedLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, onoccupiedLocation[i++]));

                break;
        }

        //Clear existing actors
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
        actors.Clear();

        //Iterate and populate gameobjects
        foreach (var x in stageActors)
        {
            var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
            var actor = prefab.GetComponent<ActorBehavior>();
            actor.archetype = x.archetype;
            actor.name = x.name;
            actor.MaxHP = x.attributes.MaxHP;
            actor.HP = x.attributes.HP;
            actor.thumbnail = x.thumbnail;
            actor.parent = board.transform;
            actor.team = x.team;
            actor.location = x.location;
            actor.spawnTurn = x.spawnTurn;
        }

        //Assign actors list
        GameObject.FindGameObjectsWithTag(Tag.Actor).Where(x => x != null).ToList()
           .ForEach(x => actors.Add(x.GetComponent<ActorBehavior>()));

    }

}
