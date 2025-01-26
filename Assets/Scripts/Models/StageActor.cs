using Game.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageActor
{
    protected DataManager dataManager => GameManager.instance.dataManager;
    protected List<ActorInstance> actors => GameManager.instance.actors;



    public Character character;
    public string name;
    public ActorStats stats;
    public Sprite thumbnail;
    public Team team;
    public Rarity quality;
    public Vector2Int location;
    public int spawnTurn = -1;

    public bool IsSpawning => spawnTurn < 1;

    public StageActor() { }

    public StageActor(Character character, Team team, Vector2Int? location = null, int spawnTurn = -1)
    {
        this.character = character;
        this.name = $"{character.ToString()}{GenerateNameSuffix(character)}";
        //this.name = character.ToString();
        this.stats = dataManager.GetStats(character.ToString());
        //this.thumbnailSettings = GameManager.instance.resourceManager.ActorSprite(this.character.ToString()).idle;
        this.team = team;
        this.location = location ?? Random.UnoccupiedLocation;
        this.spawnTurn = spawnTurn;
    }


    public string GenerateNameSuffix(Character character)
    {   
        var count = actors.Where(x => x.character == character).Count();
        if (count <= 1)
            return ""; //handle empty case 

        const int index = 65; // ASCII: 65 is 'A'
        var letter = ((char)(index + count)).ToString();
        return $" {letter}";
    }



    //public StageActor(Character character, string name, ActorStats Stats, Team team, Rarity quality, int spawnDelay)
    //{
    //    this.character = character;
    //    this.name = name;
    //    this.Stats = Stats;
    //    this.idle = GameManager.db.resourceManager.ActorThumbnail(this.character.ToString());
    //    this.team = team;
    //    this.quality = quality;
    //    this.boardLocation = boardLocation.NowhereLocation;
    //    this.spawnDelay = spawnDelay;
    //}



}
