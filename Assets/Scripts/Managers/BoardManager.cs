using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;

public class BoardManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject tilePrefab;
    [SerializeField] public GameObject actorPrefab;
    [SerializeField] public GameObject linePrefab;


    void Awake()
    {

    }

    void Start()
    {
        GenerateTiles();
        GenerateActors();
    }

    void GenerateTiles()
    {
        GameObject prefab;
        TileBehavior tile;

        for (int col = 1; col <= board.columns; col++)
        {
            for (int row = 1; row <= board.rows; row++)
            {
                prefab = Instantiate(tilePrefab, board.transform);
                tile = prefab.GetComponent<TileBehavior>();
                tile.name = $"{col}x{row}";
                tile.id = tile.name;
                tile.location = new Vector2Int(col, row);
            }
        }

        //Assign tiles list
        GameObject.FindGameObjectsWithTag(Tag.Tile).ToList()
            .ForEach(x => GameManager.instance.tiles.Add(x.GetComponent<TileBehavior>()));


    }

    void GenerateActors()
    {
        int i = 0;
        var randomLocation = Common.RandomLocations();
        var actorInit = new List<ActorOptions>();
        actorInit.Add(new ActorOptions("Paladin", resourceManager.ActorThumbnail("Paladin"), Team.Player, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Barbarian", resourceManager.ActorThumbnail("Barbarian"), Team.Player, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Ninja", resourceManager.ActorThumbnail("Ninja"), Team.Player, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Sentinel", resourceManager.ActorThumbnail("Sentinel"), Team.Player, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Panda Girl", resourceManager.ActorThumbnail("Panda Girl"), Team.Player, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Cleric", resourceManager.ActorThumbnail("Cleric"), Team.Player, randomLocation[i++]));

        actorInit.Add(new ActorOptions("Slime A", resourceManager.ActorThumbnail("Slime"), Team.Enemy, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Slime B", resourceManager.ActorThumbnail("Slime"), Team.Enemy, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Slime C", resourceManager.ActorThumbnail("Slime"), Team.Enemy, randomLocation[i++]));

        actorInit.Add(new ActorOptions("Bat A", resourceManager.ActorThumbnail("Bat"), Team.Enemy, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Bat B", resourceManager.ActorThumbnail("Bat"), Team.Enemy, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Bat C", resourceManager.ActorThumbnail("Bat"), Team.Enemy, randomLocation[i++]));

        actorInit.Add(new ActorOptions("Scorpion A", resourceManager.ActorThumbnail("Scorpion"), Team.Enemy, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Scorpion B", resourceManager.ActorThumbnail("Scorpion"), Team.Enemy, randomLocation[i++]));
        actorInit.Add(new ActorOptions("Scorpion C", resourceManager.ActorThumbnail("Scorpion"), Team.Enemy, randomLocation[i++]));

        GameObject prefab;
        ActorBehavior actor;
        foreach (var init in actorInit)
        {
            prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
            actor = prefab.GetComponent<ActorBehavior>();
            actor.name = init.name;
            actor.id = init.name;
            actor.thumbnail = init.sprite;
            actor.parent = board.transform;
            actor.location = init.location;
            actor.team = init.team;
        }

        //Assign actors list
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList()
           .ForEach(x => actors.Add(x.GetComponent<ActorBehavior>()));

        //Assign players list
        players.AddRange(actors.Where(p => p.team == Team.Player).ToList());

        //Assign enemies list
        enemies.AddRange(actors.Where(e => e.team == Team.Enemy).ToList());


        foreach(var enemy in enemies)
        {
            enemy.GenerateTurnDelay();
        }

    }


    void Update()
    {

    }

}