using Game.Manager;
using UnityEngine;

public class StageActor
{
    protected DatabaseManager databaseManager => GameManager.instance.databaseManager;



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

    public StageActor(Character character, string name, Team team, Quality quality, Vector2Int? location = null, int spawnTurn = -1)
    {
        this.character = character;
        this.name = name;
        this.stats = databaseManager.GetActorStats(character.ToString());
        //this.thumbnail = GameManager.instance.resourceManager.ActorSprite(this.character.ToString()).idle;
        this.team = team;
        this.quality = quality;
        this.location = location ?? Random.UnoccupiedLocation;
        this.spawnTurn = spawnTurn;
    }


    //public StageActor(Character character, string name, ActorStats stats, Team team, Quality quality, int spawnDelay)
    //{
    //    this.character = character;
    //    this.name = name;
    //    this.stats = stats;
    //    this.idle = GameManager.db.resourceManager.ActorThumbnail(this.character.ToString());
    //    this.team = team;
    //    this.quality = quality;
    //    this.boardLocation = boardLocation.NowhereLocation;
    //    this.spawnDelay = spawnDelay;
    //}



}
