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

    public void Run()
    {
        int index = Dropdown.value;

        switch (index)
        {
            case 1: PortraitTest(); break;
            case 2: DamageTextTest(); break;
            case 3: BumpTest(); break;
            case 4: SupportLineTest(); break;
            case 5: EnemyAttackTest(); break;
        }
    }

    public void PortraitTest()
    {
        var player = Random.Player();
        var direction = Random.Direction();
        PortraitManager.SlideIn(player, direction);
    }

    public void DamageTextTest()
    {
        var text = $"{Random.Int(1, 3)}";
        var player = Players.First(x => x.name == "Paladin");
        DamageTextManager.Spawn(text, player.Position);
    }

    public void BumpTest()
    {
        var player = Players.First(x => x.name == "Paladin");
        var direction = Random.Direction();
        StartCoroutine(player.Bump(direction));
    }

    public void SupportLineTest()
    {

        var alignedPairs = new HashSet<ActorPair>();
        foreach (var actor1 in Players)
        {
            foreach (var actor2 in Players)
            {
                if (actor1 == null || actor2 == null || actor1.Equals(actor2)
                    || !actor1.IsAlive || !actor1.IsActive || !actor2.IsAlive || !actor2.IsActive)
                    continue;

                if (actor1.IsSameColumn(actor2.Location))
                {
                    var highest = actor1.Location.y > actor2.Location.y ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    alignedPairs.Add(new ActorPair(highest, lowest, Axis.Vertical));
                }
                else if (actor1.IsSameRow(actor2.Location))
                {
                    var highest = actor1.Location.x > actor2.Location.x ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    alignedPairs.Add(new ActorPair(highest, lowest, Axis.Horizontal));
                }

            }
        }

        foreach (var pair in alignedPairs)
        {
            SupportLineManager.Spawn(pair);
        }

    }

    public void EnemyAttackTest()
    {
        var attackingEnemies = Enemies.Where(x => x.IsPlaying).ToList();
        attackingEnemies.ForEach(x => x.ReadyUp());

        if (TurnManager.IsPlayerTurn)
            TurnManager.NextTurn();

    }

}
