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
        var player = Random.Player;
        var direction = Random.Direction;
        portraitManager.SlideIn(player, direction);
    }

    public void DamageTextTest()
    {
        var text = $"{Random.Int(1, 3)}";
        var player = players.First(x => x.name == "Paladin");
        damageTextManager.Spawn(text, player.position);
    }

    public void BumpTest()
    {
        var player = players.First(x => x.name == "Paladin");
        var direction = Random.Direction;
        StartCoroutine(player.Bump(direction));
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
            //pair.sortingOrder = ZAxis.Max;
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
        attackingEnemies.ForEach(x => x.ReadyUp());

        if (turnManager.IsPlayerTurn)
            turnManager.NextTurn();

    }

}
