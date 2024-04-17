using UnityEngine;

public class StageActor
{
    public Archetype archetype;
    public string name;
    public ActorAttributes attributes;
    public Sprite thumbnail;
    public Team team;
    public Quality rarity;
    public Vector2Int location;
    public int spawnTurn = -1;




    public bool IsSpawning => spawnTurn < 1;


    public StageActor() { }

    public StageActor(Archetype archetype, string name, ActorAttributes attributes, Team team, Quality rarity, Vector2Int location)
    {
        this.archetype = archetype;
        this.name = name;
        this.attributes = attributes;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(this.archetype.ToString());
        this.team = team;
        this.rarity = rarity;
        this.location = location;
        this.spawnTurn = -1;
    }


    public StageActor(Archetype archetype, string name, ActorAttributes attributes, Team team, Quality rarity, int spawnTurn)
    {
        this.archetype = archetype;
        this.name = name;
        this.attributes = attributes;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(this.archetype.ToString());
        this.team = team;
        this.rarity = rarity;
        this.location = Locations.nowhere;
        this.spawnTurn = spawnTurn;
    }
}