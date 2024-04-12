using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ExtendedMonoBehavior : MonoBehaviour
{


    protected Canvas canvas2D => GameManager.instance.canvas2D;
    protected Canvas canvas3D => GameManager.instance.canvas3D;

    //Managers
    protected InputManager inputManager => GameManager.instance.inputManager;
    protected StageManager stageManager => GameManager.instance.stageManager;
    protected TurnManager turnManager => GameManager.instance.turnManager;
    protected ActorManager actorManager => GameManager.instance.actorManager;
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected DamageTextManager damageTextManager => GameManager.instance.damageTextManager;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected GhostManager ghostManager => GameManager.instance.ghostManager;
    protected PortraitManager portraitManager => GameManager.instance.portraitManager;
    protected OverlayManager overlayManager => GameManager.instance.overlayManager;
    protected TitleManager titleManager => GameManager.instance.titleManager;
    protected ConsoleManager consoleManager => GameManager.instance.consoleManager;
    protected CardManager cardManager => GameManager.instance.cardManager;
    protected SelectedPlayerManager selectedPlayerManager => GameManager.instance.selectedPlayerManager;
    protected PlayerManager playerManager => GameManager.instance.playerManager;
    protected EnemyManager enemyManager => GameManager.instance.enemyManager;
    protected TileManager tileManager => GameManager.instance.tileManager;
    protected FootstepManager footstepManager => GameManager.instance.footstepManager;


    //Audio
    protected AudioSource soundSource => GameManager.instance.soundSource;
    protected AudioSource musicSource => GameManager.instance.musicSource;

    //Behaviors
    protected BoardBehavior board => GameManager.instance.board;
    protected TimerBehavior timer => GameManager.instance.timer;
    protected List<TileBehavior> tiles => GameManager.instance.tiles;
    protected List<ActorBehavior> actors
    {
        get => GameManager.instance.actors;
        set => GameManager.instance.actors = value;
    }

    protected List<ActorBehavior> players => GameManager.instance.actors.Where(x => x.team.Equals(Team.Player)).ToList();
    protected List<ActorBehavior> enemies => GameManager.instance.actors.Where(x => x.team.Equals(Team.Enemy)).ToList();

    //Actor
    protected bool HasSelectedPlayer => selectedPlayer != null;
    protected bool HasCurrentPlayer => currentPlayer != null;

    //Scale
    protected float tileSize => GameManager.instance.tileSize;
    protected Vector2 tileScale => GameManager.instance.tileScale;

    protected ShakeIntensity ShakeIntensity => GameManager.instance.shakeIntensity;

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

    protected float cursorSpeed => GameManager.instance.cursorSpeed;
    protected float slideSpeed => GameManager.instance.slideSpeed;
    protected float snapDistance => GameManager.instance.snapDistance;


    protected AttackParticipants attackParticipants
    {
        get { return GameManager.instance.attackParticipants; }
        set { GameManager.instance.attackParticipants = value; }
    }

    //protected bool InSameColumn(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x;
    //protected bool InSameRow(ActorBehavior a, ActorBehavior b) => a.location.y == b.location.y;
    //protected bool IsAbove(ActorBehavior a, ActorBehavior b) => InSameColumn(a, b) && a.location.y == b.location.y - 1;
    //protected bool IsRight(ActorBehavior a, ActorBehavior b) => InSameRow(a, b) && a.location.x == b.location.x + 1;
    //protected bool IsBelow(ActorBehavior a, ActorBehavior b) => InSameColumn(a, b) && a.location.y == b.location.y + 1;
    //protected bool IsLeft(ActorBehavior a, ActorBehavior b) => InSameRow(a, b) && a.location.x == b.location.x - 1;

    //protected bool IsAdjacent(ActorBehavior a, ActorBehavior b)
    //{
    //    return IsAbove(a, b) || IsRight(a, b) || IsBelow(a, b) || IsLeft(a, b);
    //}


    protected ActorBehavior selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }

    protected ActorBehavior currentPlayer
    {
        get { return GameManager.instance.currentPlayer; }
        set { GameManager.instance.currentPlayer = value; }
    }

    public TileBehavior unoccupiedTile => tiles.Where(x => !x.IsOccupied).OrderBy(x => Guid.NewGuid()).First();


}

