using UnityEngine;

public class StageActor
{
    public Archetype archetype;
    public string name;
    public ActorStats stats;
    public Sprite thumbnail;
    public Team team;
    public Quality quality;
    public Vector2Int location;
    public int spawnTurn = -1;




    public bool IsSpawning => spawnTurn < 1;


    public StageActor() { }

    public StageActor(Archetype archetype, string name, ActorStats stats, Team team, Quality quality, Vector2Int? location = null, int spawnTurn = -1)
    {
        this.archetype = archetype;
        this.name = name;
        this.stats = stats;
        this.thumbnail = GameManager.instance.resourceManager.ActorThumbnail(this.archetype.ToString());
        this.team = team;
        this.quality = quality;
        this.location = location.HasValue ? location.Value : Random.UnoccupiedLocation;
        this.spawnTurn = spawnTurn;
    }


    //public StageActor(Archetype archetype, string name, ActorStats stats, Team team, Quality quality, int spawnDelay)
    //{
    //    this.archetype = archetype;
    //    this.name = name;
    //    this.stats = stats;
    //    this.idle = GameManager.instance.resourceManager.ActorThumbnail(this.archetype.ToString());
    //    this.team = team;
    //    this.quality = quality;
    //    this.location = location.Nowhere;
    //    this.spawnDelay = spawnDelay;
    //}



}
