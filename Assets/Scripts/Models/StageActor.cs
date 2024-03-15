using UnityEngine;

public class StageActor
{
    public Archetype archetype;
    public string name;
    public ActorAttributes attributes;
    public Sprite thumbnail;
    public Team team;
    public Vector2Int location;
    public int? spawnTurn = null;


    public StageActor() { }
    public StageActor(Archetype archetype, string name, ActorAttributes attributes, Team team, Vector2Int location)
    {
        this.archetype = archetype;
        this.name = name;
        this.attributes = attributes;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(this.archetype.ToString());
        this.team = team;
        this.location = location;
        this.spawnTurn = null;
    }


    public StageActor(Archetype archetype, string name, ActorAttributes attributes, Team team, int spawnTurn)
    {
        this.archetype = archetype;
        this.name = name;
        this.attributes = attributes;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(this.archetype.ToString());
        this.team = team;
        this.location = Vector2Int.zero;
        this.spawnTurn = spawnTurn;
    }
}