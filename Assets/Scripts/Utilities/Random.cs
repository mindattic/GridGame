using Game.Behaviors.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Random
{
    [ThreadStatic] public static System.Random rng = new System.Random();

    //Properties
    private static IQueryable<ActorBehavior> players => GameManager.instance.players;
    private static IQueryable<ActorBehavior> enemies => GameManager.instance.enemies;
    private static List<ActorBehavior> actors => GameManager.instance.actors;
    private static List<TileBehavior> tiles => GameManager.instance.tiles;
    private static int columnCount => GameManager.instance.board.columnCount;
    private static int rowCount => GameManager.instance.board.rowCount;

    public static int Int(int min, int max) => rng.Next(min, max + 1);

    public static float Float(float min = 0f, float max = 1f) => (float)rng.NextDouble() * (max - min) + min;

    public static float Percent => (float)rng.NextDouble();

    public static float Range(float amount) => (-amount * Percent) + (amount * Percent);

    public static bool Boolean => Int(1, 2) == 1;

    public static Direction Direction
    {
        get
        {
            var result = Int(1, 4);
            return result switch {
                1 => Direction.North,
                2 => Direction.East,
                3 => Direction.South,
                _ => Direction.West,
            };
        }
    }

    public static Color Color => new Color(Float(), Float(), Float(), 1f);

    public static AttackStrategy Strategy(params int[] ratios)
    {
        //int sum = Int(0, ratios.Sum());

        //int ratio0 = ratios[0];
        //int ratio1 = ratio0 + ratios[1];
        //int ratio2 = ratio1 + ratios[2];
        //int ratio3 = ratio2 + ratios[3];
        //int ratio4 = ratio3 + ratios[4];
        //int ratio5 = ratio4 + ratios[5];

        //int result = Int(0, sum);

        //if ((result -= ratio0) < 0) return Strategy.AttackClosest;

        //{
        //    do_something1();
        //}
        //else if ((x -= RATIO_CHANCE_B) < 0) // Test for B
        //{
        //    do_something2();
        //}
        //// ... etc
        //else // No need for final if statement
        //{
        //    do_somethingN();
        //}


        //TODO: Add in weighted value so some attacks are more common that others...

        //int result = Int(0, ratios.Sum());

        /*
        int RATIO_CHANCE_A = 10;
        int RATIO_CHANCE_B = 30;
        int RATIO_CHANCE_C = 60;    
        int RATIO_TOTAL = RATIO_CHANCE_A + RATIO_CHANCE_B + RATIO_CHANCE_C;

        Random random = new Random();
        int x = random.Next(0, RATIO_TOTAL);

        if ((x -= RATIO_CHANCE_A) < 0) // Test for A
        { 
             do_something1();
        } 
        else if ((x -= RATIO_CHANCE_B) < 0) // Test for B
        { 
             do_something2();
        }
        // ... etc
        else // No need for final if statement
        { 
             do_somethingN();
        }
        */




        //var result = Int(1, 5);
        //return result switch
        //{
        //    1 => Strategy.MoveAnywhere,
        //    2 => Strategy.AttackClosest,
        //    3 => Strategy.AttackWeakest,
        //    4 => Strategy.AttackStrongest,
        //    5 => Strategy.AttackRandom,
        //    _ => Strategy.MoveAnywhere,
        //};

        var result = Int(1, 2);
        return result switch
        {
            1 => AttackStrategy.AttackClosest,
            2 => AttackStrategy.AttackRandom,
            _ => AttackStrategy.AttackClosest,
        };

    }





    public static ActorBehavior Player => players.Where(x => x.IsPlaying).OrderBy(x => Guid.NewGuid()).First();

    public static ActorBehavior Enemy => enemies.Where(x => x.IsPlaying).OrderBy(x => Guid.NewGuid()).First();

    public static TileBehavior Tile => tiles.OrderBy(x => Guid.NewGuid()).First();

    public static TileBehavior UnoccupiedTile => tiles.Where(x => !x.IsOccupied).OrderBy(x => Guid.NewGuid()).First();

    public static Vector2Int Location => new Vector2Int(Int(1, columnCount), Int(1, rowCount));

    public static Vector2Int UnoccupiedLocation => UnoccupiedTile.location;

}
