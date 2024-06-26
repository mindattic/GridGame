using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtendedMonoBehavior : MonoBehaviour
{

    //Canvases
    protected Canvas canvas2D => GameManager.instance.Canvas2D;
    protected Canvas canvas3D => GameManager.instance.Canvas3D;

    //Managers
    protected InputManager InputManager => GameManager.instance.InputManager;
    protected StageManager StageManager => GameManager.instance.StageManager;
    protected TurnManager TurnManager => GameManager.instance.TurnManager;
    protected ActorManager ActorManager => GameManager.instance.ActorManager;
    protected SupportLineManager SupportLineManager => GameManager.instance.SupportLineManager;
    protected AttackLineManager AttackLineManager => GameManager.instance.AttackLineManager;
    protected DamageTextManager DamageTextManager => GameManager.instance.DamageTextManager;
    protected ResourceManager ResourceManager => GameManager.instance.ResourceManager;
    protected GhostManager GhostManager => GameManager.instance.GhostManager;
    protected PortraitManager PortraitManager => GameManager.instance.PortraitManager;
    protected OverlayManager OverlayManager => GameManager.instance.OverlayManager;
    protected TitleManager TitleManager => GameManager.instance.TitleManager;
    protected ConsoleManager ConsoleManager => GameManager.instance.ConsoleManager;
    protected CardManager CardManager => GameManager.instance.CardManager;
    protected SelectedPlayerManager SelectedPlayerManager => GameManager.instance.SelectedPlayerManager;
    protected PlayerManager PlayerManager => GameManager.instance.PlayerManager;
    protected EnemyManager EnemyManager => GameManager.instance.EnemyManager;
    protected TileManager TileManager => GameManager.instance.TileManager;
    protected FootstepManager FootstepManager => GameManager.instance.FootstepManager;

    //Audio
    protected AudioSource SoundSource => GameManager.instance.SoundSource;
    protected AudioSource MusicSource => GameManager.instance.MusicSource;

    //Behaviors
    protected BoardBehavior Board => GameManager.instance.Board;
    protected TimerBehavior Timer => GameManager.instance.Timer;
    protected List<TileBehavior> Tiles => GameManager.instance.Tiles;
    protected List<ActorBehavior> Actors
    {
        get => GameManager.instance.Actors;
        set => GameManager.instance.Actors = value;
    }

    protected List<ActorBehavior> Players => GameManager.instance.Actors.Where(x => x.Team.Equals(Team.Player)).ToList();
    protected List<ActorBehavior> Enemies => GameManager.instance.Actors.Where(x => x.Team.Equals(Team.Enemy)).ToList();

    //Layers
    protected static class ActorLayer
    {
        public const int Back = 0;
        public const int Parallax = 1;
        public const int Glow = 2;
        public const int Thumbnail = 3;
        public const int Frame = 4;
        public const int StatusIcon = 5;
        public const int HealthBarBack = 6;
        public const int HealthBar = 7;
        public const int HealthBarFront = 8;
        public const int HealthText = 9;
        public const int ActionBarBack = 10;
        public const int ActionBar = 11;
        public const int ActionText = 12;
        public const int RadialBack = 13;
        public const int RadialFill = 14;
        public const int RadialText = 15;
        public const int Selection = 16;
        public const int Mask = 17;
    }

    //Actor
    protected bool HasFocusedPlayer => FocusedPlayer != null;
    protected bool HasSelectedPlayer => SelectedPlayer != null;

    //Scale
    protected float TileSize => GameManager.instance.TileSize;
    protected Vector2 TileScale => GameManager.instance.TileScale;

    protected ShakeIntensity ShakeIntensity => GameManager.instance.ShakeIntensity;

    //Percent
    protected float Percent25 => Constants.Percent25;
    protected float Percent33 => Constants.Percent33;
    protected float Percent50 => Constants.Percent50;
    protected float Percent66 => Constants.Percent66;
    protected float Percent75 => Constants.Percent75;
    protected float Percent100 => Constants.Percent100;
    protected float Percent333 => Constants.Percent333;
    protected float Percent666 => Constants.Percent666;

    //Size
    protected Vector2 Size25 => Constants.size25;
    protected Vector2 Size33 => Constants.size33;
    protected Vector2 Size50 => Constants.size50;
    protected Vector2 Size66 => Constants.size66;
    protected Vector2 Size75 => Constants.size75;
    protected Vector2 Size100 => Constants.size100;

    //Mouse
    protected Vector3 MousePosition2D => GameManager.instance.MousePosition2D;
    protected Vector3 MousePosition3D => GameManager.instance.MousePosition3D;
    protected Vector3 MouseOffset
    {
        get { return GameManager.instance.MouseOffset; }
        set { GameManager.instance.MouseOffset = value; }
    }

    protected float CursorSpeed => GameManager.instance.CursorSpeed;
    protected float SlideSpeed => GameManager.instance.SlideSpeed;
    protected float BumpSpeed => GameManager.instance.BumpSpeed;
    protected float SnapDistance => GameManager.instance.SnapDistance;


    protected AttackParticipants AttackParticipants
    {
        get { return GameManager.instance.AttackParticipants; }
        set { GameManager.instance.AttackParticipants = value; }
    }

    protected ActorBehavior FocusedPlayer
    {
        get { return GameManager.instance.FocusedPlayer; }
        set { GameManager.instance.FocusedPlayer = value; }
    }

    protected ActorBehavior SelectedPlayer
    {
        get { return GameManager.instance.SelectedPlayer; }
        set { GameManager.instance.SelectedPlayer = value; }
    }

    public TileBehavior UnoccupiedTile => Tiles.Where(x => !x.IsOccupied).OrderBy(x => Guid.NewGuid()).First();


    public void CheckAlignment(ActorPair pair, out bool hasEnemiesBetween, out bool hasPlayersBetween, out bool hasGapsBetween)
    {
        hasEnemiesBetween = false;
        hasPlayersBetween = false;
        hasGapsBetween = false;

        if (pair.Axis == Axis.Vertical)
        {
            pair.HighestActor = pair.Actor1.Location.y > pair.Actor2.Location.y ? pair.Actor1 : pair.Actor2;
            pair.LowestActor = pair.HighestActor == pair.Actor1 ? pair.Actor2 : pair.Actor1;
            pair.Enemies = Enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameColumn(pair.Actor1.Location) && Common.IsBetween(x.Location.y, pair.Floor, pair.Ceiling)).OrderBy(x => x.Location.y).ToList();
            pair.Players = Players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameColumn(pair.Actor1.Location) && Common.IsBetween(x.Location.y, pair.Floor, pair.Ceiling)).OrderBy(x => x.Location.y).ToList();
            pair.Gaps = Tiles.Where(x => !x.IsOccupied && pair.Actor1.IsSameColumn(x.Location) && Common.IsBetween(x.Location.y, pair.Floor, pair.Ceiling)).OrderBy(x => x.Location.y).ToList();
        }
        else if (pair.Axis == Axis.Horizontal)
        {
            pair.HighestActor = pair.Actor1.Location.x > pair.Actor2.Location.x ? pair.Actor1 : pair.Actor2;
            pair.LowestActor = pair.HighestActor == pair.Actor1 ? pair.Actor2 : pair.Actor1;
            pair.Enemies = Enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameRow(pair.Actor1.Location) && Common.IsBetween(x.Location.x, pair.Floor, pair.Ceiling)).OrderBy(x => x.Location.x).ToList();
            pair.Players = Players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameRow(pair.Actor1.Location) && Common.IsBetween(x.Location.x, pair.Floor, pair.Ceiling)).OrderBy(x => x.Location.x).ToList();
            pair.Gaps = Tiles.Where(x => !x.IsOccupied && pair.Actor1.IsSameRow(x.Location) && Common.IsBetween(x.Location.x, pair.Floor, pair.Ceiling)).OrderBy(x => x.Location.x).ToList();
        }

        hasEnemiesBetween = pair.Enemies.Any();
        hasPlayersBetween = pair.Players.Any();
        hasGapsBetween = pair.Gaps.Any();
        //var hasDuplicate = AttackParticipants.attackingPairs.Count(x => (x.Actor1 == Pair.Actor1 && x.Actor2 == Pair.Actor2) || (x.Actor1 == Pair.Actor2 && x.Actor2 == Pair.Actor1)) > 0;
    }




}

