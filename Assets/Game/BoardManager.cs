using System.Linq;
using UnityEngine;

public class BoardManager : ExtendedMonoBehavior
{
    public GameObject tilePrefab;
    public GameObject actorPrefab;



    void Awake()
    {

    }

    void Start()
    {
        transform.position = board.offset;
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

        //Assign actors list
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList()
           .ForEach(x => GameManager.instance.actors.Add(x.GetComponent<ActorBehavior>()));
    }



    void Update()
    {

    }






}