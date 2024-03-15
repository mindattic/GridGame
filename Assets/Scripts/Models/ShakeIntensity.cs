public class ShakeIntensity
{
    public float High;
    public float Medium;
    public float Low;

    public ShakeIntensity(float maxSize)
    {
        High = maxSize / 6f;
        Medium = maxSize / 12f;
        Low = maxSize / 24f;
    }

}

