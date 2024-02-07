using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject actorPrefab;

    SpriteManager spriteManager;

    private float tileSize => GameManager.instance.tileSize;
    private Vector2 tileScale => GameManager.instance.tileScale;
    private Vector2 boardOffset => GameManager.instance.boardOffset;

    void Awake()
    {
        spriteManager = GameObject.Find(Constants.Sprites).GetComponent<SpriteManager>();

    }

    void Start()
    {
        transform.position = boardOffset;
        GenerateTiles();
        GenerateActors();
    }

    void GenerateTiles()
    {
        int columns = 5;
        int rows = 8;

        for (int col = 1; col <= columns; col++)
        {
            for (int row = 1; row <= rows; row++)
            {
                var prefab = Instantiate(tilePrefab, transform);        
                var tile = prefab.GetComponent<TileBehavior>();
                tile.name = $"{col}x{row}";
                tile.location = new Vector2Int(col, row);
            }
        }
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

        //var prefab = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        //player2.name = "Corsair";
        //player2.GetComponent<SpriteRenderer>().sprite = spriteManager.corsair;
        //player2.transform.SetParent(transform, true);
        //player2.transform.localScale = tileScale;

        //var player3 = Instantiate(actorPrefab, Vector2.zero, Quaternion.identity);
        //player3.name = "Oracle";
        //player3.GetComponent<SpriteRenderer>().sprite = spriteManager.oracle;
        //player3.transform.SetParent(transform, true);
        //player3.transform.localScale = tileScale;

        //Assign actors list
        GameManager.instance.actors = GameObject.Find(Constants.Board).GetComponents<ActorBehavior>().ToList();

    }

}