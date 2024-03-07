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
        titleManager.Show($"Stage {currentStage}");

        var attributes = new ActorAttributes() { HP = 100, MaxHP = 100 };
        int i = 0;
        var randomLocation = Common.RandomLocations();

        List<StageActor> stageActors = new List<StageActor>();

        switch (currentStage)
        {
            case 1:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 2:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 3:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 4:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 5:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Ninja, "Ninja", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Sentinel, "Sentinel", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.PandaGirl, "Panda Girl", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Cleric, "Cleric", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime B", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime C", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Bat, "Bat A", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Bat, "Bat B", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Bat, "Bat C", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Scorpion, "Scorpion A", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Scorpion, "Scorpion B", attributes, Team.Enemy, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Scorpion, "Scorpion C", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 6:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 7:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;


            case 8:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 9:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            case 10:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;

            default:

                stageActors.Add(new StageActor(Archetype.Paladin, "Paladin", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Barbarian, "Barbarian", attributes, Team.Player, randomLocation[i++]));
                stageActors.Add(new StageActor(Archetype.Slime, "Slime A", attributes, Team.Enemy, randomLocation[i++]));

                break;
        }

        //Iterate and populate gameobjects
        foreach (var x in stageActors)
        {
            var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
            var actor = prefab.GetComponent<ActorBehavior>();
            actor.id = x.id;
            actor.name = x.name;
            actor.attributes = x.attributes;
            actor.thumbnail = x.thumbnail;
            actor.parent = board.transform;
            actor.location = x.location;
            actor.team = x.team;
        }

        //Assign actors list
        GameObject.FindGameObjectsWithTag(Tag.Actor).Where(x => x != null).ToList()
           .ForEach(x => actors.Add(x.GetComponent<ActorBehavior>()));

    }

}
