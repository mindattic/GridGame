using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugManager : ExtendedMonoBehavior
{

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    private void Update()
    {

    }


    public void PortraitTest()
    {
        var player = players[Random.Int(0, players.Count - 1)];
        var direction = Random.Direction();
        portraitManager.SlideIn(player, direction);
    }


    public void BumpTest()
    {
        var player = players.First(x => x.name == "Paladin");
        var direction = Random.Direction();
        StartCoroutine(player.Bump(direction));
    }


    public void DamageTextTest()
    {
        var text = $"{Random.Int(1, 3)}";
        var player = players.First(x => x.name == "Paladin");
        damageTextManager.Spawn(text, player.position);
    }

    public void SupportLineTest()
    {

        var alignedPairs = new HashSet<ActorPair>();
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2) || actor1 == null || actor2 == null
                    || !actor1.IsAlive || !actor1.IsActive || !actor2.IsAlive || !actor2.IsActive)
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var highest = actor1.location.y > actor2.location.y ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    alignedPairs.Add(new ActorPair(highest, lowest, Axis.Vertical));
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var highest = actor1.location.x > actor2.location.x ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    alignedPairs.Add(new ActorPair(highest, lowest, Axis.Horizontal));
                }

            }
        }




        foreach(var pair in alignedPairs)
        {
            supportLineManager.Spawn(pair);
        }


      
    }




}
