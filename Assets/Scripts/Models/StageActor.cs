using UnityEngine;

public class StageActor
{
    public string id;
    public string name;
    public ActorAttributes attributes;
    public Sprite thumbnail;
    public Team team;
    public Vector2Int location;
    public int? spawnTurn = null;


    public StageActor() { }
    public StageActor(string id, string name, ActorAttributes attributes, Team team, Vector2Int location)
    {
        this.id = id;
        this.name = name;
        this.attributes = attributes;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(id);
        this.team = team;
        this.location = location;
        this.spawnTurn = null;
    }


    public StageActor(string id, string name, ActorAttributes attributes, Team team, int spawnTurn)
    {
        this.id = id;
        this.name = name;
        this.attributes = attributes;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(id);
        this.team = team;
        this.location = Vector2Int.zero;
        this.spawnTurn = spawnTurn;
    }
}