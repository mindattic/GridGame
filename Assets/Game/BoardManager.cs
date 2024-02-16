using System;
using System.Collections.Generic;
using System.Linq;
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
        transform.position = board.offset;
        GenerateTiles();
        GenerateActors();
        GenerateLines();
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
        var actorInit = new List<ActorInit>()
        {
            new ActorInit("Sentinel", spriteManager.sentinel, Team.Player, randomLocation[i++]),
            new ActorInit("Corsair", spriteManager.corsair, Team.Player, randomLocation[i++]),
            new ActorInit("Oracle", spriteManager.oracle, Team.Player, randomLocation[i++]),
            new ActorInit("Mechanic", spriteManager.mechanic, Team.Player, randomLocation[i++]),
            new ActorInit("Mercenary", spriteManager.mercenary, Team.Player, randomLocation[i++]),
            new ActorInit("Slime A", spriteManager.slime, Team.Enemy, randomLocation[i++]),
            new ActorInit("Slime B", spriteManager.slime, Team.Enemy, randomLocation[i++]),
            new ActorInit("Slime C", spriteManager.slime, Team.Enemy, randomLocation[i++]),
            new ActorInit("Bat A", spriteManager.bat, Team.Enemy, randomLocation[i++]),
            new ActorInit("Bat B", spriteManager.bat, Team.Enemy, randomLocation[i++]),
            new ActorInit("Bat C", spriteManager.bat, Team.Enemy, randomLocation[i++]),
        };

        GameObject prefab;
        ActorBehavior actor;
        foreach (var init in actorInit)
        {
            prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
            actor = prefab.GetComponent<ActorBehavior>();
            actor.name = init.name;
            actor.sprite = init.sprite;
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
    }

    void GenerateLines()
    {
        GameObject prefab;
        LineBehavior line;

        foreach (var player in players)
        {
            for (int i = 1; i <= 4; i++)
            {
                prefab = Instantiate(linePrefab, Vector2.zero, Quaternion.identity);
                line = prefab.GetComponent<LineBehavior>();
                line.name = $"Line_{Guid.NewGuid()}";
                line.parent = board.transform;
            }
        }

        //Assign line list
        GameObject.FindGameObjectsWithTag(Tag.Line).ToList()
           .ForEach(x => GameManager.instance.lines.Add(x.GetComponent<LineBehavior>()));
    }

    void Update()
    {

    }

}