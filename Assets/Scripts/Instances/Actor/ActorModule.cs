using Assets.Scripts.Behaviors.Actor;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Instances.Actor
{
    public class ActorModule
    {

        //Variables
        protected ActorInstance instance;


        //Instance Properties
        protected bool IsActive => instance.IsActive;
        protected bool IsAlive => instance.IsAlive;
        protected TileInstance currentTile => tileManager.tileMap[location]; //tiles.First(x => x.location.Equals(location));
        protected ActorRenderers render => instance.render;
        protected ActorFlags flags => instance.flags;
        protected ActorStats stats => instance.stats;

       
        //Properties
        protected GameManager gameManager => GameManager.instance;
        protected AudioManager audioManager => gameManager.audioManager;
        protected TileManager tileManager => gameManager.tileManager;
        protected float snapDistance => gameManager.snapDistance;
        protected float tileSize => gameManager.tileSize;
        protected Vector3 tileScale => gameManager.tileScale;
        protected float moveSpeed => gameManager.moveSpeed;
        protected ActorInstance focusedActor => gameManager.focusedActor;
        protected ActorInstance selectedPlayer => gameManager.selectedPlayer;
        protected List<ActorInstance> actors => gameManager.actors;
        protected BoardInstance board => gameManager.board;
        protected float percent33 => Constants.percent33;

        protected int sortingOrder
        {
            get => instance.sortingOrder;
            set => instance.sortingOrder = value;
        }

        protected Vector2Int previousLocation
        {
            get => instance.previousLocation;
            set => instance.previousLocation = value;
        }

        protected Vector2Int location
        {
            get => instance.location;
            set => instance.location = value;
        }


        protected Vector3 position
        {
            get => instance.transform.position;
            set => instance.transform.position = value;
        }

        protected Vector3 destination
        {
            get => instance.destination;
            set => instance.destination = value;
        }

        protected Vector3 thumbnailPosition
        {
            get => instance.transform.GetChild("Thumbnail").gameObject.transform.position;
            set => instance.transform.GetChild("Thumbnail").gameObject.transform.position = value;
        }

        protected Quaternion rotation
        {
            get => instance.transform.rotation;
            set => instance.transform.rotation = value;
        }

        protected Vector3 scale
        {
            get => instance.transform.localScale;
            set => instance.transform.localScale = value;
        }



        //Methods

        public void Initialize(ActorInstance parentInstance)
        {
            this.instance = parentInstance;
        }

    }
}
