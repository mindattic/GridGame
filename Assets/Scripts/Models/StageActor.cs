using UnityEngine;

public class StageActor
{
    public Character character;
    public string name;
    public ActorStats stats;
    public Sprite thumbnail;
    public Team team;
    public Quality quality;
    public Vector2Int location;
    public int spawnTurn = -1;




    public bool IsSpawning => spawnTurn < 1;


    public StageActor() { }

    public StageActor(Character character, string name, ActorStats stats, Team team, Quality quality, Vector2Int? location = null, int spawnTurn = -1)
    {
        this.character = character;
        this.name = name;
        this.stats = stats;
        this.thumbnail = GameManager.instance.resourceManager.ActorSprite(this.character.ToString()).idle;
        this.team = team;
        this.quality = quality;
        this.location = location.HasValue ? location.Value : Random.UnoccupiedLocation;
        this.spawnTurn = spawnTurn;
    }


    //public StageActor(Character character, string name, ActorStats stats, Team team, Quality quality, int spawnDelay)
    //{
    //    this.character = character;
    //    this.name = name;
    //    this.stats = stats;
    //    this.idle = GameManager.instance.resourceManager.ActorThumbnail(this.character.ToString());
    //    this.team = team;
    //    this.quality = quality;
    //    this.boardLocation = boardLocation.NowhereLocation;
    //    this.spawnDelay = spawnDelay;
    //}



}
