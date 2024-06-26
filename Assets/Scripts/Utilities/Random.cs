using System;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

static class Random
{
    [ThreadStatic] public static System.Random rng = new System.Random();

    public static int Int(int min, int max)
    {
        return rng.Next(min, max + 1);
    }

    public static float Float(float min, float max)
    {
        return (float)rng.NextDouble() * (max - min) + min;
    }

    public static float Percent()
    {
        return (float)rng.NextDouble();
    }

    public static float Range(float amount)
    {
        return (-amount * Percent()) + (amount * Percent());
    }


    public static bool Boolean()
    {
        return Int(1, 2) == 1 ? true : false;
    }

    public static Direction Direction()
    {
        var result = Int(1, 4);
        return result switch
        {
            1 => global::Direction.North,
            2 => global::Direction.East,
            3 => global::Direction.South,
            _ => global::Direction.West,
        };
    }


    public static AttackStrategy AttackStrategy(params int[] ratios)
    {
        //int sum = Int(0, ratios.Sum());

        //int ratio0 = ratios[0];
        //int ratio1 = ratio0 + ratios[1];
        //int ratio2 = ratio1 + ratios[2];
        //int ratio3 = ratio2 + ratios[3];
        //int ratio4 = ratio3 + ratios[4];
        //int ratio5 = ratio4 + ratios[5];

        //int result = Int(0, sum);

        //if ((result -= ratio0) < 0) return global::AttackStrategy.AttackClosest;

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
        //    1 => global::AttackStrategy.MoveAnywhere,
        //    2 => global::AttackStrategy.AttackClosest,
        //    3 => global::AttackStrategy.AttackWeakest,
        //    4 => global::AttackStrategy.AttackStrongest,
        //    5 => global::AttackStrategy.AttackRandom,
        //    _ => global::AttackStrategy.MoveAnywhere,
        //};

        var result = Int(1, 2);
        return result switch
        {
            1 => global::AttackStrategy.AttackClosest,
            2 => global::AttackStrategy.AttackRandom,
            _ => global::AttackStrategy.AttackClosest,
        };

    }




    public static ActorBehavior Player()
    {
        return GameManager.instance.Actors
            .Where(x => x.Team.Equals(Team.Player))
            .OrderBy(x => Guid.NewGuid())
            .First();
    }

    public static ActorBehavior Enemy()
    {
        return GameManager.instance.Actors
            .Where(x => x.Team.Equals(Team.Enemy))
            .OrderBy(x => Guid.NewGuid())
            .First();
    }

    public static TileBehavior Tile()
    {
        return GameManager.instance.Tiles
            .OrderBy(x => Guid.NewGuid())
            .First();
    }

    public static Vector2Int Location()
    {
        int col = Int(1, GameManager.instance.Board.ColumnCount);
        int row = Int(1, GameManager.instance.Board.RowCount);
        return new Vector2Int(col, row);
    }

}
