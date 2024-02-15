using System;

static class RNG
{
    [ThreadStatic] public static Random random = new Random();

    public static int RandomInt(int min, int max)
    {
        return random.Next(min, max + 1);
    }
}
