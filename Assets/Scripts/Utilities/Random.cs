using Game.Behaviors.Actor;
using System;
using System.Linq;
using UnityEngine;

static class Random
{
    [ThreadStatic] public static System.Random rng = new System.Random();

    public static int Int(int min, int max) => rng.Next(min, max + 1);

    public static float Float(float min, float max) => (float)rng.NextDouble() * (max - min) + min;

    public static float Percent => (float)rng.NextDouble();

    public static float Range(float amount) => (-amount * Percent) + (amount * Percent);

    public static bool Boolean => Int(1, 2) == 1;

    public static Direction Direction
    {
        get
        {
            var result = Int(1, 4);
            return result switch
            {
                1 => Direction.North,
                2 => Direction.East,
                3 => Direction.South,
                _ => Direction.West,
            };
        }
    }


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




    public static ActorBehavior Player => GameManager.instance.actors.Where(x => x.team.Equals(Team.Player)).OrderBy(x => Guid.NewGuid()).First();

    public static ActorBehavior Enemy => GameManager.instance.actors.Where(x => x.team.Equals(Team.Enemy)).OrderBy(x => Guid.NewGuid()).First();

    public static TileBehavior Tile => GameManager.instance.tiles.OrderBy(x => Guid.NewGuid()).First();

    public static TileBehavior UnoccupiedTile => GameManager.instance.tiles.Where(x => !x.IsOccupied).OrderBy(x => Guid.NewGuid()).First();

    public static Vector2Int Location => new Vector2Int(Int(1, GameManager.instance.board.columnCount), GameManager.instance.board.rowCount);

    public static Vector2Int UnoccupiedLocation => UnoccupiedTile.location;

}
