using System;

static class Random
{
    [ThreadStatic] public static System.Random random = new System.Random();

    public static int Int(int min, int max)
    {
        return random.Next(min, max + 1);
    }

    public static float Percent()
    {
        return (float)random.NextDouble();
    }

    public static float Range(float amount)
    {
        return -amount + amount * Percent();
    }


    public static bool Boolean()
    {
        return Int(1, 2) == 1 ? true : false;
    }

    //public static Direction Direction()
    //{
    //    var result = Int(1, 4);
    //    return result switch
    //    {
    //        1 => global::Direction.North,
    //        2 => global::Direction.East,
    //        3 => global::Direction.South,
    //        _ => global::Direction.West,
    //    };
    //}

}
