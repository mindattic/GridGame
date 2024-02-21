using UnityEngine;

public static class Constants
{
    public const string Game = "Game";
    public const string Board = "Board";
    public const string Timer = "Timer";

    //Percent
    public const float percent25 = 0.25f;
    public const float percent33 = 0.333333f;
    public const float percent50 = 0.5f;
    public const float percent66 = 0.666666f;
    public const float percent75 = 0.75f;
    public const float percent100 = 1.0f;
    public const float percent333 = 3.333333f;
    public const float percent666 = 6.666666f;

    //Size
    public static readonly Vector2 size25 = new Vector2(percent25, percent25);
    public static readonly Vector2 size33 = new Vector2(percent33, percent33);
    public static readonly Vector2 size50 = new Vector2(percent50, percent50);
    public static readonly Vector2 size66 = new Vector2(percent66, percent66);
    public static readonly Vector2 size75 = new Vector2(percent75, percent75);
    public static readonly Vector2 size100 = new Vector2(percent100, percent100);
}

public static class Tag
{
    public static string Board = "Board";
    public static string Tile = "Tile";
    public static string Actor = "Actor";
    public static string SupportLine = "SupportLine";
    public static string Trail = "Trail";
    public static string Select = "Select";
    public static string Wall = "Wall";
}


public static class Colors
{
    public static class Solid
    {
        public static Color Gold = Common.ColorRGBA(255, 215, 0);
        public static Color White = Color.white;
        public static Color LightBlue = Common.ColorRGBA(128, 128, 255);
        public static Color LightRed = Common.ColorRGBA(255, 128, 128);
        public static Color Red = Common.ColorRGBA(255, 0, 0);

    }

    public static class Transparent
    {
        public static Color Gold = Common.ColorRGBA(255, 215, 0, 100);
        public static Color White = Common.ColorRGBA(255, 255, 255, 100);
        public static Color LightBlue = Common.ColorRGBA(128, 128, 255, 100);
        public static Color LightRed = Common.ColorRGBA(255, 128, 128, 100);
        public static Color Red = Common.ColorRGBA(255, 0, 0, 100);
    }

}
