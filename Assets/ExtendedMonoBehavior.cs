using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ExtendedMonoBehavior : MonoBehaviour
{
    //Managers
    protected SpriteManager spriteManager => GameManager.instance.spriteManager;

    //Behaviors
    protected BoardBehavior board => GameManager.instance.board;
    protected TimerBehavior timer => GameManager.instance.timer;
    protected List<TileBehavior> tiles => GameManager.instance.tiles;
    protected List<ActorBehavior> actors => GameManager.instance.actors;
    protected List<ActorBehavior> players => GameManager.instance.players;
    protected List<ActorBehavior> enemies => GameManager.instance.enemies;
    protected List<LineBehavior> lines => GameManager.instance.lines;

    //Actor
    protected bool HasSelectedPlayer => selectedPlayer != null;

    //Scale
    protected float tileSize => GameManager.instance.tileSize;
    protected Vector2 tileScale => GameManager.instance.tileScale;

    //Percent
    protected float percent25 => Constants.percent25;
    protected float percent33 => Constants.percent33;
    protected float percent50 => Constants.percent50;
    protected float percent66 => Constants.percent66;
    protected float percent75 => Constants.percent75;
    protected float percent100 => Constants.percent100;
    protected float percent333 => Constants.percent333;
    protected float percent666 => Constants.percent666;

    //Size
    protected Vector2 size25 => Constants.size25;
    protected Vector2 size33 => Constants.size33;
    protected Vector2 size50 => Constants.size50;
    protected Vector2 size66 => Constants.size66;
    protected Vector2 size75 => Constants.size75;
    protected Vector2 size100 => Constants.size100;

    //Mouse
    protected Vector3 mousePosition2D => GameManager.instance.mousePosition2D;
    protected Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    protected Vector3 mouseOffset
    {
        get { return GameManager.instance.mouseOffset; }
        set { GameManager.instance.mouseOffset = value; }
    }

    protected float moveSpeed => GameManager.instance.moveSpeed;
    protected float slideSpeed => GameManager.instance.slideSpeed;
    protected float snapDistance => GameManager.instance.snapDistance;


    protected HashSet<string> attackerNames
    {
        get { return GameManager.instance.attackerNames; }
        set { GameManager.instance.attackerNames = value; }
    }
    protected HashSet<string> defenderNames
    {
        get { return GameManager.instance.defenderNames; }
        set { GameManager.instance.defenderNames = value; }
    }

    protected bool InSameColumn(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x;
    protected bool InSameRow(ActorBehavior a, ActorBehavior b) => a.location.y == b.location.y;
    protected bool IsAbove(ActorBehavior a, ActorBehavior b) => InSameColumn(a, b) && a.location.y == b.location.y - 1;
    protected bool IsRight(ActorBehavior a, ActorBehavior b) => InSameRow(a, b) && a.location.x == b.location.x + 1;
    protected bool IsBelow(ActorBehavior a, ActorBehavior b) => InSameColumn(a, b) && a.location.y == b.location.y + 1;
    protected bool IsLeft(ActorBehavior a, ActorBehavior b) => InSameRow(a, b) && a.location.x == b.location.x - 1;

    protected bool IsAdjacent(ActorBehavior a, ActorBehavior b)
    {
        return IsAbove(a, b) || IsRight(a, b) || IsBelow(a, b) || IsLeft(a, b);
    }

    protected ActorBehavior selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }

}

