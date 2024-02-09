using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MonoBehaviorBase : MonoBehaviour
{
    //Managers
    protected SpriteManager spriteManager => GameManager.instance.spriteManager;

    //Behaviors
    protected BoardBehavior board => GameManager.instance.board;
    protected TimerBehavior timer => GameManager.instance.timer;

    //Collections
    protected List<TileBehavior> tiles => GameManager.instance.tiles;
    protected List<ActorBehavior> actors => GameManager.instance.actors;


    //Actor
    protected bool HasActiveActor => activeActor != null;
    protected bool IsSameColumn(Vector2Int location) => HasActiveActor && activeActor.location.x == location.x;
    protected bool IsSameRow(Vector2Int location) => HasActiveActor && activeActor.location.y == location.y;
    protected bool IsAbove(Vector2Int location) => activeActor.location.y == location.y - 1;
    protected bool IsRight(Vector2Int location) => activeActor.location.x == location.x + 1;
    protected bool IsBelow(Vector2Int location) => activeActor.location.y == location.y + 1;
    protected bool IsLeft(Vector2Int location) => activeActor.location.x == location.x - 1;

    //Scale
    protected float tileSize => GameManager.instance.tileSize;
    protected Vector2 tileScale => GameManager.instance.tileScale;

    //Size
    protected Vector2 size25 => GameManager.instance.size25;
    protected Vector2 size33 => GameManager.instance.size33;
    protected Vector2 size50 => GameManager.instance.size50;
    protected Vector2 size66 => GameManager.instance.size66;
    protected Vector2 size100 => GameManager.instance.size100;

    //Mouse
    protected Vector3 mousePosition2D => GameManager.instance.mousePosition2D;
    protected Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    protected float moveSpeed => GameManager.instance.moveSpeed;
    protected float snapDistance => GameManager.instance.snapDistance;




    protected Vector3 mouseOffset
    {
        get { return GameManager.instance.mouseOffset; }
        set { GameManager.instance.mouseOffset = value; }
    }


    protected ActorBehavior activeActor
    {
        get { return GameManager.instance.activeActor; }
        set { GameManager.instance.activeActor = value; }
    }

}

