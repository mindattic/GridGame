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
        var vfx = resourceManager.VisualEffect("Blue_Slash_01");  
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Slash_02()
    {
        var vfx = resourceManager.VisualEffect("Blue_Slash_02");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Slash_03()
    {
        var vfx = resourceManager.VisualEffect("Blue_Slash_03");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Sword()
    {
        var vfx = resourceManager.VisualEffect("Blue_Sword");  
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Blue_Sword_4X()
    {
        var vfx = resourceManager.VisualEffect("Blue_Sword_4X");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Blood_Claw()
    {
        var vfx = resourceManager.VisualEffect("Blood_Claw");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Level_Up()
    {
        var vfx = resourceManager.VisualEffect("Level_Up");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Yellow_Hit()
    {
        var vfx = resourceManager.VisualEffect("Yellow_Hit");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Double_Claw()
    {
        var vfx = resourceManager.VisualEffect("Double_Claw");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Lightning_Explosion()
    {
        var vfx = resourceManager.VisualEffect("Lightning_Explosion");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

    public void VFXTest_Buff_Life()
    {
        var vfx = resourceManager.VisualEffect("Buff_Life");
        vfxManager.Spawn(vfx, Paladin.position);
        vfxManager.Spawn(vfx, Barbarian.position);
    }

}
