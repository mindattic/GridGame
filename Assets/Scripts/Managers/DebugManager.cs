using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Phase = TurnPhase;

public class DebugManager : ExtendedMonoBehavior
{
    [SerializeField] private TMP_Dropdown Dropdown;

    ActorBehavior Paladin => players.First(x => x.name == "Paladin");
    ActorBehavior Barbarian => players.First(x => x.name == "Barbarian");

    public void Run()
    {
        int index = Dropdown.value;
        if (index < 1) 
            return;

        switch (index)
        {
            case 1: PortraitTest(); break;
            case 2: DamageTextTest(); break;
            case 3: BumpTest(); break;
            case 4: SupportLineTest(); break;
            case 5: EnemyAttackTest(); break;
            case 6: TitleTest(); break;
            case 7: TooltipTest(); break;
            case 8: VFXTest_Blue_Slash_01(); break;
            case 9: VFXTest_Blue_Slash_02(); break;
            case 10: VFXTest_Blue_Slash_03(); break;
            case 11: VFXTest_Blue_Sword(); break;
            case 12: VFXTest_Blue_Sword_4X(); break;
            case 13: VFXTest_Blood_Claw(); break;
            case 14: VFXTest_Level_Up(); break;
            case 15: VFXTest_Yellow_Hit(); break;
            case 16: VFXTest_Double_Claw(); break;
            case 17: VFXTest_Lightning_Explosion(); break;
            case 18: VFXTest_Buff_Life(); break;
            case 19: VFXTest_Rotary_Knife(); break;
            case 20: VFXTest_Air_Slash(); break;
            case 21: VFXTest_Fire_Rain(); break;
            case 22: VFXTest_Ray_Blast(); break;
            case 23: VFXTest_Lightning_Strike(); break;
            case 24: VFXTest_Puffy_Explosion(); break;
            case 25: VFXTest_Red_Slash_2X(); break;
            case 26: VFXTest_God_Rays(); break;
            case 27: VFXTest_Acid_Splash(); break;
            case 28: VFXTest_Green_Buff(); break;
            case 29: VFXTest_Gold_Buff(); break;
            case 30: VFXTest_Hex_Shield(); break;
            case 31: VFXTest_Toxic_Cloud(); break;
            case 32: VFXTest_Orange_Slash(); break;
            case 33: VFXTest_Moon_Feather(); break;
            case 34: VFXTest_Pink_Spark(); break;
            case 35: VFXTest_BlueYellow_Sword(); break;
            case 36: VFXTest_BlueYellow_Sword_3X(); break;
            case 37: VFXTest_Red_Sword(); break;




        }
    }

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
      
        var damage = 3f;
        var isCriticalHit = Random.Int(1, 10) == 10;
        if (isCriticalHit)
        {
            var crit = resourceManager.VisualEffect("Yellow_Hit");
            vfxManager.SpawnAsync(crit, Paladin.position);
            damage = (float)Math.Round(damage * 1.5f);
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

}
