using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;




public class ResourceManager : ExtendedMonoBehavior
{
    //[SerializeField] public GameObject spritePrefab;
    //[SerializeField] public GameObject artPrefab;

    [SerializeField] public List<ActorSprite> actorSprites = new List<ActorSprite>();
    //ArtCollection portraits = new ArtCollection();
 
    //[SerializeField] public Sprite paladin;
    //[SerializeField] public Sprite barbarian;
    //[SerializeField] public Sprite ninja;
    //[SerializeField] public Sprite cleric;
    //[SerializeField] public Sprite sentinel;
    //[SerializeField] public Sprite pandagirl;

    //[SerializeField] public Sprite slime;
    //[SerializeField] public Sprite bat;



    int sortingOrder = 0;

    private void Awake()
    {
        //sprites.Add(new ArtPiece("Barbarian", Load("Thumbnails\\Barbarian"), Load("Portraits\\Barbarian")));
        //sprites.Add(new ArtPiece("Bat", Load("Thumbnails\\Bat"), Load("Portraits\\Bat")));
        //sprites.Add(new ArtPiece("Cleric", Load("Thumbnails\\Cleric"), Load("Portraits\\Cleric")));
        //sprites.Add(new ArtPiece("Paladin", Load("Thumbnails\\Paladin"), Load("Portraits\\Paladin")));
        //sprites.Add(new ArtPiece("Panda Girl", Load("Thumbnails\\PandaGirl"), Load("Portraits\\PandaGirl")));
        //sprites.Add(new ArtPiece("Ninja", Load("Thumbnails\\Ninja"), Load("Portraits\\Ninja")));
        //sprites.Add(new ArtPiece("Sentinel", Load("Thumbnails\\Sentinel"), Load("Portraits\\Sentinel")));
        //sprites.Add(new ArtPiece("Slime", Load("Thumbnails\\Slime"), Load("Portraits\\Slime")));    
    }

    public Sprite Thumbnail(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).thumbnail;
    }

    public Sprite Portrait(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).portrait;
    }


}
