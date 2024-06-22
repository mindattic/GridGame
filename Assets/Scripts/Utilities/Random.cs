using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;

static class Random
{
    [ThreadStatic] public static System.Random random = new System.Random();

    public static int Int(int min, int max)
    {
        return random.Next(min, max + 1);
    }

    public static float Float(float min, float max)
    {
        return (float)random.NextDouble() * (max - min) + min;
    }

    public static float Percent()
    {
        return (float)random.NextDouble();
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


    public static AttackStrategy Strategy()
    {
        var result = Int(1, 3);
        return result switch
        {
            1 => AttackStrategy.MoveAnywhere,
            2 => AttackStrategy.AttackClosest,
            3 => AttackStrategy.AttackClosest,
            _ => AttackStrategy.MoveAnywhere,
        };
    }




    public static ActorBehavior Player()
    {
        return GameManager.instance.actors
            .Where(x => x.team.Equals(Team.Player))
            .OrderBy(x => Guid.NewGuid())
            .First();
    }

    public static ActorBehavior Enemy()
    {
        return GameManager.instance.actors
            .Where(x => x.team.Equals(Team.Enemy))
            .OrderBy(x => Guid.NewGuid())
            .First();
    }

    public static TileBehavior Tile()
    {
        return GameManager.instance.tiles
            .OrderBy(x => Guid.NewGuid())
            .First();
    }
}
