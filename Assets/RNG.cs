using System;

static class RNG
{
    [ThreadStatic] public static Random random = new Random();

    public static int RandomInt(int min, int max)
    {
        return random.Next(min, max + 1);
    }

    public static float RandomPercent()
    {
        return (float)random.NextDouble();
    }


    public static Direction RandomDirection()
    {
        var result = RandomInt(1, 4);
        return result switch
        {
            1 => Direction.North,
            2 => Direction.East,
            3 => Direction.South,
            _ => Direction.West,
        };
    }

}
