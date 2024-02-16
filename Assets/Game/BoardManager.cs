using System;
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
                prefab = Instantiate(tilePrefab, transform);
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
        GameObject prefab;
        ActorBehavior actor;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Sentinel";
        actor.sprite = spriteManager.sentinel;
        actor.parent = transform;
        actor.location = new Vector2Int(2, 3);
        actor.team = Team.Player;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Corsair";
        actor.sprite = spriteManager.corsair;
        actor.parent = transform;
        actor.location = new Vector2Int(4, 4);
        actor.team = Team.Player;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Oracle";
        actor.sprite = spriteManager.oracle;
        actor.parent = transform;
        actor.location = new Vector2Int(5, 6);
        actor.team = Team.Player;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Mechanic";
        actor.sprite = spriteManager.mechanic;
        actor.parent = transform;
        actor.location = new Vector2Int(1, 7);
        actor.team = Team.Player;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Mercenary";
        actor.sprite = spriteManager.mercenary;
        actor.parent = transform;
        actor.location = new Vector2Int(2, 7);
        actor.team = Team.Player;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Slime A";
        actor.sprite = spriteManager.slime;
        actor.parent = transform;
        actor.location = new Vector2Int(3, 2);
        actor.team = Team.Enemy;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Slime B";
        actor.sprite = spriteManager.slime;
        actor.parent = transform;
        actor.location = new Vector2Int(3, 3);
        actor.team = Team.Enemy;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Slime C";
        actor.sprite = spriteManager.slime;
        actor.parent = transform;
        actor.location = new Vector2Int(4, 1);
        actor.team = Team.Enemy;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Bat A";
        actor.sprite = spriteManager.bat;
        actor.parent = transform;
        actor.location = new Vector2Int(4, 2);
        actor.team = Team.Enemy;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Bat B";
        actor.sprite = spriteManager.bat;
        actor.parent = transform;
        actor.location = new Vector2Int(5, 4);
        actor.team = Team.Enemy;

        prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        actor = prefab.GetComponent<ActorBehavior>();
        actor.name = "Bat C";
        actor.sprite = spriteManager.bat;
        actor.parent = transform;
        actor.location = new Vector2Int(6, 8);
        actor.team = Team.Enemy;

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
            for(int i = 1; i <= 4; i++)
            {
                prefab = Instantiate(linePrefab, Vector2.zero, Quaternion.identity);
                line = prefab.GetComponent<LineBehavior>();
                line.name = $"Line_{Guid.NewGuid()}";
                line.parent = transform;
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