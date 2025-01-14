using Assets.Scripts.Models;
using Game.Behaviors;
using Game.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    #region Properties
    protected List<ActorInstance> actors => GameManager.instance.actors;
    protected IQueryable<ActorInstance> players => GameManager.instance.players;
    protected IQueryable<ActorInstance> enemies => GameManager.instance.enemies;
    protected ActorManager actorManager => GameManager.instance.actorManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected CoinManager coinManager => GameManager.instance.coinManager;
    protected DamageTextManager damageTextManager => GameManager.instance.damageTextManager;
    protected DatabaseManager databaseManager => GameManager.instance.databaseManager;
    protected PortraitManager portraitManager => GameManager.instance.portraitManager;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected StageManager stageManager => GameManager.instance.stageManager;
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected TooltipManager tooltipManager => GameManager.instance.tooltipManager;
    protected TurnManager turnManager => GameManager.instance.turnManager;
    protected VFXManager vfxManager => GameManager.instance.vfxManager;
    protected CanvasOverlay canvasOverlay => GameManager.instance.canvasOverlay;


    #endregion

    //Variables
    [SerializeField] private TMP_Dropdown Dropdown;
    public bool showActorNameTag = false;
    public bool showActorFrame = false;
    public bool isPlayerInvincible = false;
    public bool isEnemyInvincible = false;
    public bool isTimerInfinite = false;
    public bool isEnemyStunned = false;
    ActorInstance paladin => players.First(x => x.name == "Paladin");
    ActorInstance barbarian => players.First(x => x.name == "Barbarian");
    ActorInstance cleric => players.First(x => x.name == "Cleric");
    ActorInstance ninja => players.First(x => x.name == "Ninja");

    public void PortraitTest()
    {
        var player = Random.Player;
        var direction = Random.Direction;
        portraitManager.SlideIn(player, direction);
    }

    public void DamageTextTest()
    {
        var text = $"{Random.Int(1, 3)}";
        damageTextManager.Spawn(text, paladin.position);
    }

    public void BumpTest()
    {
        var direction = Random.Direction;
        paladin.action.TriggerBump(direction);
    }

    public void ShakeTest()
    {
        var intensity = Random.ShakeIntensityLevel();
        var duration = Random.Float(Interval.HalfSecond, Interval.TwoSeconds);
        paladin.action.TriggerShake(intensity, duration);
    }

    public void DodgeTest()
    {
        paladin.action.TriggerDodge();
    }

    public void SpinTest()
    {
        paladin.action.TriggerSpin360();
    }

    public void SupportLineTest()
    {
        var alignedPairs = new HashSet<ActorPair>();
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1 == null || actor2 == null || actor1.Equals(actor2) || !actor1.isActive || !actor1.isAlive || !actor2.isActive || !actor2.isAlive)
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Vertical);
                    alignedPairs.Add(pair);
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Horizontal);
                    alignedPairs.Add(pair);
                }

            }
        }

        foreach (var pair in alignedPairs)
        {
            pair.startActor.sortingOrder = SortingOrder.Supporter;
            pair.endActor.sortingOrder = SortingOrder.Supporter;
            supportLineManager.Spawn(pair);
        }

        IEnumerator _()
        {
            yield return Wait.For(Interval.ThreeSeconds);

            foreach (var supportLine in supportLineManager.supportLines.Values)
            {
                supportLine.TriggerDespawn();
            }
        }

        StartCoroutine(_());
    }

    public void AttackLineTest()
    {
        var enemy1 = enemies.Skip(0).Take(1).FirstOrDefault();
        var enemy2 = enemies.Skip(1).Take(1).FirstOrDefault();
        var enemy3 = enemies.Skip(2).Take(1).FirstOrDefault();
        var enemy4 = enemies.Skip(3).Take(1).FirstOrDefault();
        var enemy5 = enemies.Skip(4).Take(1).FirstOrDefault();
        var enemy6 = enemies.Skip(5).Take(1).FirstOrDefault();

        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 1))?.Teleport(new Vector2Int(1, 1));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 2))?.Teleport(new Vector2Int(1, 2));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 3))?.Teleport(new Vector2Int(1, 3));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 4))?.Teleport(new Vector2Int(1, 4));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 5))?.Teleport(new Vector2Int(1, 5));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 6))?.Teleport(new Vector2Int(1, 6));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 7))?.Teleport(new Vector2Int(1, 7));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 8))?.Teleport(new Vector2Int(1, 8));

        paladin.Teleport(new Vector2Int(3, 1));
        enemy1?.Teleport(new Vector2Int(3, 2));
        enemy2?.Teleport(new Vector2Int(3, 3));
        enemy3?.Teleport(new Vector2Int(3, 4));
        enemy4?.Teleport(new Vector2Int(3, 5));
        enemy5?.Teleport(new Vector2Int(3, 6));
        enemy6?.Teleport(new Vector2Int(3, 7));
        barbarian.Teleport(new Vector2Int(3, 8));




        var alignedPairs = new HashSet<ActorPair>();
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1 == null || actor2 == null
                    || actor1.Equals(actor2)
                    || !actor1.isActive || !actor1.isAlive
                    || !actor2.isActive || !actor2.isAlive)
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Vertical);
                    alignedPairs.Add(pair);
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Horizontal);
                    alignedPairs.Add(pair);
                }

            }
        }

        foreach (var pair in alignedPairs)
        {
            pair.startActor.sortingOrder = SortingOrder.Attacker;
            pair.endActor.sortingOrder = SortingOrder.Attacker;
            attackLineManager.Spawn(pair);
        }

        IEnumerator _()
        {
            yield return Wait.For(Interval.ThreeSeconds);

            foreach (var attackLine in attackLineManager.attackLines.Values)
            {
                attackLine.TriggerDespawn();
            }
        }

        StartCoroutine(_());
    }

    public void EnemyAttackTest()
    {
        var attackingEnemies = enemies.Where(x => x.isActive && x.isAlive).ToList();
        attackingEnemies.ForEach(x => x.SetReady());

        if (turnManager.isPlayerTurn)
            turnManager.NextTurn();

    }

    public void TitleTest()
    {
        var text = DateTime.UtcNow.Ticks.ToString();
        canvasOverlay.TriggerFadeIn(text);
        canvasOverlay.TriggerFadeOut(Interval.ThreeSeconds);

    }

    public void TooltipTest()
    {
        var text = $"Test {Random.Int(1000, 9999)}";
        var position = Random.Player.currentTile.position;
        tooltipManager.Spawn(text, position);
    }

    public void VFXTest_Blue_Slash_01()
    {
        var attack = new AttackResult()
        {
            Opponent = paladin,
            IsHit = true,
            IsCriticalHit = Random.Int(1, 10) == 10,
            Damage = 3
        };

        if (attack.IsCriticalHit)
        {
            var crit = resourceManager.VisualEffect("Yellow_Hit");
            vfxManager.TriggerSpawn(crit, paladin.position);
            attack.Damage = (int)Math.Round(attack.Damage * 1.5f);
        }

        var vfx = resourceManager.VisualEffect("Blue_Slash_01");
        var trigger = new Trigger(paladin.TakeDamage(attack));
        vfxManager.TriggerSpawn(vfx, paladin.position, trigger);
    }

    public void VFXTest_Blue_Slash_02()
    {
        var vfx = resourceManager.VisualEffect("Blue_Slash_02");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Blue_Slash_03()
    {
        var vfx = resourceManager.VisualEffect("Blue_Slash_03");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Blue_Sword()
    {
        var vfx = resourceManager.VisualEffect("Blue_Sword");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Blue_Sword_4X()
    {
        var vfx = resourceManager.VisualEffect("Blue_Sword_4X");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Blood_Claw()
    {
        var vfx = resourceManager.VisualEffect("Blood_Claw");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Level_Up()
    {
        var vfx = resourceManager.VisualEffect("Level_Up");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Yellow_Hit()
    {
        var vfx = resourceManager.VisualEffect("Yellow_Hit");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Double_Claw()
    {
        var vfx = resourceManager.VisualEffect("Double_Claw");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Lightning_Explosion()
    {
        var vfx = resourceManager.VisualEffect("Lightning_Explosion");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Buff_Life()
    {
        var vfx = resourceManager.VisualEffect("Buff_Life");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Rotary_Knife()
    {
        var vfx = resourceManager.VisualEffect("Rotary_Knife");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Air_Slash()
    {
        var vfx = resourceManager.VisualEffect("Air_Slash");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Fire_Rain()
    {
        var vfx = resourceManager.VisualEffect("Fire_Rain");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Ray_Blast()
    {
        var vfx = resourceManager.VisualEffect("Ray_Blast");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Lightning_Strike()
    {
        var vfx = resourceManager.VisualEffect("Lightning_Strike");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Puffy_Explosion()
    {
        var vfx = resourceManager.VisualEffect("Puffy_Explosion");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Red_Slash_2X()
    {
        var vfx = resourceManager.VisualEffect("Red_Slash_2X");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_God_Rays()
    {
        var vfx = resourceManager.VisualEffect("God_Rays");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Acid_Splash()
    {
        var vfx = resourceManager.VisualEffect("Acid_Splash");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }
    public void VFXTest_Green_Buff()
    {
        var vfx = resourceManager.VisualEffect("Green_Buff");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Gold_Buff()
    {
        var vfx = resourceManager.VisualEffect("Gold_Buff");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Hex_Shield()
    {
        var vfx = resourceManager.VisualEffect("Hex_Shield");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Toxic_Cloud()
    {
        var vfx = resourceManager.VisualEffect("Toxic_Cloud");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Orange_Slash()
    {
        var vfx = resourceManager.VisualEffect("Orange_Slash");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Moon_Feather()
    {
        var vfx = resourceManager.VisualEffect("Moon_Feather");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Pink_Spark()
    {
        var vfx = resourceManager.VisualEffect("Pink_Spark");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_BlueYellow_Sword()
    {
        var vfx = resourceManager.VisualEffect("BlueYellow_Sword");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_BlueYellow_Sword_3X()
    {
        var vfx = resourceManager.VisualEffect("BlueYellow_Sword_3X");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }

    public void VFXTest_Red_Sword()
    {
        var vfx = resourceManager.VisualEffect("Red_Sword");
        vfxManager.TriggerSpawn(vfx, paladin.position);
        vfxManager.TriggerSpawn(vfx, barbarian.position);
    }


    public void AlignTest()
    {
        // Spawn exactly nine slimes
        for (int i = 0; i < 9; i++)
            SpawnSlime();

        // Assign specific enemies for teleportation
        var enemy1 = enemies.ElementAtOrDefault(0);
        var enemy2 = enemies.ElementAtOrDefault(1);
        var enemy3 = enemies.ElementAtOrDefault(2);
        var enemy4 = enemies.ElementAtOrDefault(3);
        var enemy5 = enemies.ElementAtOrDefault(4);
        var enemy6 = enemies.ElementAtOrDefault(5);
        var enemy7 = enemies.ElementAtOrDefault(6);
        var enemy8 = enemies.ElementAtOrDefault(7);
        var enemy9 = enemies.ElementAtOrDefault(8);

        // Define the group to remain aligned
        var group = new[] { paladin, barbarian, cleric, ninja, enemy1, enemy2, enemy3, enemy4, enemy5, enemy6, enemy7, enemy8, enemy9 };

        // Teleport actors in the group to specific positions
        paladin?.Teleport(new Vector2Int(1, 1));
        enemy1?.Teleport(new Vector2Int(1, 2));
        enemy2?.Teleport(new Vector2Int(1, 3));
        barbarian?.Teleport(new Vector2Int(1, 4));
        enemy3?.Teleport(new Vector2Int(2, 4));
        enemy4?.Teleport(new Vector2Int(3, 4));
        enemy5?.Teleport(new Vector2Int(4, 4));
        enemy6?.Teleport(new Vector2Int(5, 4));
        cleric?.Teleport(new Vector2Int(6, 4));
        enemy7?.Teleport(new Vector2Int(6, 5));
        enemy8?.Teleport(new Vector2Int(6, 6));
        enemy9?.Teleport(new Vector2Int(6, 7));
        ninja?.Teleport(new Vector2Int(6, 8));

        // Move all other actors to unoccupied locations
        actors.Except(group).ToList().ForEach(x => x.Teleport(Random.UnoccupiedLocation));
    }



    public void CoinTest()
    {
        var vfx = resourceManager.VisualEffect("Yellow_Hit");


        IEnumerator spawnTenCoins()
        {
            var i = 0;
            do
            {
                coinManager.Spawn(paladin.position);
                i++;
            } while (i < 10);

            yield return true;
        }
        var trigger = new Trigger(spawnTenCoins());

        vfxManager.TriggerSpawn(vfx, paladin.position, trigger);
    }

    public void SpawnSlime()
    {
        var location = Random.UnoccupiedLocation;
        var stats = databaseManager.GetActorStats("Slime");
        stageManager.Add(new StageActor(Character.Slime, $"Slime {Guid.NewGuid()}", Team.Enemy, Rarity.Common, location));
    }

    public void SpawnBat()
    {
        var location = Random.UnoccupiedLocation;
        var stats = databaseManager.GetActorStats("Bat");
        stageManager.Add(new StageActor(Character.Bat, $"Bat {Guid.NewGuid()}", Team.Enemy, Rarity.Common, location));
    }

    public void SpawnScorpion()
    {
        var location = Random.UnoccupiedLocation;
        var stats = databaseManager.GetActorStats("Scorpion");
        stageManager.Add(new StageActor(Character.Scorpion, $"Scorpion {Guid.NewGuid()}", Team.Enemy, Rarity.Common, location));
    }

    public void SpawnYeti()
    {
        var location = Random.UnoccupiedLocation;
        var stats = databaseManager.GetActorStats("Yeti");
        stageManager.Add(new StageActor(Character.Yeti, $"Yeti {Guid.NewGuid()}", Team.Enemy, Rarity.Common, location));
    }
}
