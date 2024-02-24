using UnityEngine;

public class ActorOptions
{
    public string name;
    public Sprite sprite;
    public Team team;
    public Vector2Int location;
    public ActorOptions() { }
    public ActorOptions(string name, Sprite sprite, Team team, Vector2Int location)
    {
        this.name = name;
        this.sprite = sprite;
        this.team = team;
        this.location = location;
    }
}