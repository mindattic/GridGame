using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;



public class ResourceManager : ExtendedMonoBehavior
{

    [SerializeField] public List<ActorSprite> actorSprites = new List<ActorSprite>();

    [SerializeField] public List<StatusSprite> statusSprites = new List<StatusSprite>();

    private void Awake()
    {
        //sprites.Create(new ArtPiece("Barbarian", Load("Thumbnails\\Barbarian"), Load("Portraits\\Barbarian")));
        //sprites.Create(new ArtPiece("Bat", Load("Thumbnails\\Bat"), Load("Portraits\\Bat")));
        //sprites.Create(new ArtPiece("Cleric", Load("Thumbnails\\Cleric"), Load("Portraits\\Cleric")));
        //sprites.Create(new ArtPiece("Paladin", Load("Thumbnails\\Paladin"), Load("Portraits\\Paladin")));
        //sprites.Create(new ArtPiece("Panda Girl", Load("Thumbnails\\PandaGirl"), Load("Portraits\\PandaGirl")));
        //sprites.Create(new ArtPiece("Ninja", Load("Thumbnails\\Ninja"), Load("Portraits\\Ninja")));
        //sprites.Create(new ArtPiece("Sentinel", Load("Thumbnails\\Sentinel"), Load("Portraits\\Sentinel")));
        //sprites.Create(new ArtPiece("Slime", Load("Thumbnails\\Slime"), Load("Portraits\\Slime")));    
    }

    public Sprite ActorThumbnail(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).thumbnail;
    }

    public Sprite ActorPortrait(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).portrait;
    }


    public Sprite StatusThumbnail(string id)
    {
        return statusSprites.First(x => x.id.Equals(id)).thumbnail;
    }

}
