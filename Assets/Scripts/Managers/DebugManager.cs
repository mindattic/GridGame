using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Phase = TurnPhase;

public class DebugManager : ExtendedMonoBehavior
{
    [SerializeField] private TMP_Dropdown Dropdown;

    ActorInstance Paladin => players.First(x => x.name == "Paladin");
    ActorInstance Barbarian => players.First(x => x.name == "Barbarian");

    public void PortraitTest()
    {
        var player = Random.Player;
        var direction = Random.Direction;
        portraitManager.SlideIn(player, direction);
    }

    public void DamageTextTest()
    {
        var text = $"{Random.Int(1, 3)}";
        damageTextManager.Spawn(text, Paladin.position);
    }

    public void BumpTest()
    {
        var direction = Random.Direction;
        StartCoroutine(Paladin.Bump(direction));
    }

    public void SupportLineTest()
    {
        var alignedPairs = new HashSet<ActorPair>();
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1 == null || actor2 == null || actor1.Equals(actor2) || !actor1.IsPlaying || !actor2.IsPlaying)
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
            supportLineManager.Spawn(pair);
        }

        IEnumerator _()
        {
            yield return Wait.For(3);

            foreach (var supportLine in supportLineManager.supportLines)
            {
                supportLine.DespawnAsync();
            }
        }

        StartCoroutine(_());
    }

    public void EnemyAttackTest()
    {
        var attackingEnemies = enemies.Where(x => x.IsPlaying).ToList();
        attackingEnemies.ForEach(x => x.SetReady());

        if (turnManager.IsPlayerTurn)
            turnManager.NextTurn();

    }

    public void TitleTest()
    {
        titleManager.Print(DateTime.UtcNow.Ticks.ToString());
    }

    public void TooltipTest()
    {
        var text = $"Test {Random.Int(1000, 9999)}";
        var position = Random.Player.currentTile.position;
        tooltipManager.Spawn(text, position);
    }

    public void VFXTest_Blue_Slash_01()
    {

        int damage = 3;
        var isCriticalHit = Random.Int(1, 10) == 10;
        if (isCriticalHit)
        {
            var crit = resourceManager.VisualEffect("Yellow_Hit");
            vfxManager.SpawnAsync(crit, Paladin.position);
            damage = (int)Math.Round(damage * 1.5f);
        }

        var vfx = resourceManager.VisualEffect("Blue_Slash_01");
        vfxManager.SpawnAsync(vfx, Paladin.position, Paladin.TakeDamage(damage, isCriticalHit));
    }

    public void VFXTest_Blue_Slash_02()
    {
        var vfx = resourceManager.VisualEffect("Blue_Slash_02");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Slash_03()
    {
        var vfx = resourceManager.VisualEffect("Blue_Slash_03");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Sword()
    {
        var vfx = resourceManager.VisualEffect("Blue_Sword");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Sword_4X()
    {
        var vfx = resourceManager.VisualEffect("Blue_Sword_4X");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Blood_Claw()
    {
        var vfx = resourceManager.VisualEffect("Blood_Claw");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Level_Up()
    {
        var vfx = resourceManager.VisualEffect("Level_Up");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Yellow_Hit()
    {
        var vfx = resourceManager.VisualEffect("Yellow_Hit");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Double_Claw()
    {
        var vfx = resourceManager.VisualEffect("Double_Claw");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Lightning_Explosion()
    {
        var vfx = resourceManager.VisualEffect("Lightning_Explosion");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Buff_Life()
    {
        var vfx = resourceManager.VisualEffect("Buff_Life");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Rotary_Knife()
    {
        var vfx = resourceManager.VisualEffect("Rotary_Knife");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Air_Slash()
    {
        var vfx = resourceManager.VisualEffect("Air_Slash");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Fire_Rain()
    {
        var vfx = resourceManager.VisualEffect("Fire_Rain");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Ray_Blast()
    {
        var vfx = resourceManager.VisualEffect("Ray_Blast");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Lightning_Strike()
    {
        var vfx = resourceManager.VisualEffect("Lightning_Strike");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Puffy_Explosion()
    {
        var vfx = resourceManager.VisualEffect("Puffy_Explosion");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Red_Slash_2X()
    {
        var vfx = resourceManager.VisualEffect("Red_Slash_2X");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_God_Rays()
    {
        var vfx = resourceManager.VisualEffect("God_Rays");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Acid_Splash()
    {
        var vfx = resourceManager.VisualEffect("Acid_Splash");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }
    public void VFXTest_Green_Buff()
    {
        var vfx = resourceManager.VisualEffect("Green_Buff");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Gold_Buff()
    {
        var vfx = resourceManager.VisualEffect("Gold_Buff");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Hex_Shield()
    {
        var vfx = resourceManager.VisualEffect("Hex_Shield");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Toxic_Cloud()
    {
        var vfx = resourceManager.VisualEffect("Toxic_Cloud");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Orange_Slash()
    {
        var vfx = resourceManager.VisualEffect("Orange_Slash");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Moon_Feather()
    {
        var vfx = resourceManager.VisualEffect("Moon_Feather");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Pink_Spark()
    {
        var vfx = resourceManager.VisualEffect("Pink_Spark");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_BlueYellow_Sword()
    {
        var vfx = resourceManager.VisualEffect("BlueYellow_Sword");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_BlueYellow_Sword_3X()
    {
        var vfx = resourceManager.VisualEffect("BlueYellow_Sword_3X");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void VFXTest_Red_Sword()
    {
        var vfx = resourceManager.VisualEffect("Red_Sword");
        vfxManager.SpawnAsync(vfx, Paladin.position);
        vfxManager.SpawnAsync(vfx, Barbarian.position);
    }

    public void DodgeTest()
    {
        StartCoroutine(Paladin.Dodge());
    }

    public void SpinTest()
    {
        StartCoroutine(Paladin.Spin360());
    }

    public void AlignTest()
    {
        var enemy1 = enemies.Skip(0).Take(1).FirstOrDefault();
        var enemy2 = enemies.Skip(1).Take(1).FirstOrDefault();
        var enemy3 = enemies.Skip(2).Take(1).FirstOrDefault();
        var enemy4 = enemies.Skip(3).Take(1).FirstOrDefault();
        var enemy5 = enemies.Skip(4).Take(1).FirstOrDefault();
        var enemy6 = enemies.Skip(5).Take(1).FirstOrDefault();

        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 1))?.Relocate(new Vector2Int(1, 1));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 2))?.Relocate(new Vector2Int(1, 2));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 3))?.Relocate(new Vector2Int(1, 3));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 4))?.Relocate(new Vector2Int(1, 4));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 5))?.Relocate(new Vector2Int(1, 5));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 6))?.Relocate(new Vector2Int(1, 6));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 7))?.Relocate(new Vector2Int(1, 7));
        actors.FirstOrDefault(x => x.location == new Vector2Int(3, 8))?.Relocate(new Vector2Int(1, 8));

        Paladin.Relocate(new Vector2Int(3, 1));
        enemy1?.Relocate(new Vector2Int(3, 2));
        enemy2?.Relocate(new Vector2Int(3, 3));
        enemy3?.Relocate(new Vector2Int(3, 4));
        enemy4?.Relocate(new Vector2Int(3, 5));
        enemy5?.Relocate(new Vector2Int(3, 6));
        enemy6?.Relocate(new Vector2Int(3, 7));
        Barbarian.Relocate(new Vector2Int(3, 8));
    }


    public void CoinTest()
    {
        var vfx = resourceManager.VisualEffect("Yellow_Hit");


        IEnumerator spawnMany()
        {
            var i = 0;
            do
            {
                coinManager.Spawn(Paladin.position);
                i++;
            } while (i < 10);

            yield return true;
        }


        vfxManager.SpawnAsync(vfx, Paladin.position, spawnMany());

    }
}
