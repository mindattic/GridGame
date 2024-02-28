using UnityEngine;

public static class Constants
{
    public const string Game = "Game";
    public const string Board = "Board";
    public const string Timer = "Timer";
    public const string Canvas2D = "Canvas2D";
    public const string Canvas3D = "Canvas3D";
    public const string Art = "Art";


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
    public static string DamageText = "DamageText";
    public static string AnnouncementText = "AnnouncementText";
    public static string Portrait = "ActorPortrait";
    public static string Ghost = "Ghost";
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
        public static Color Green = Common.ColorRGBA(0, 255, 0);
        public static Color Gray = new Color(0.5f, 0.5f, 0.5f);
    }

    public static class Translucent
    {
        public static Color Gold = Common.ColorRGBA(255, 215, 0, 100);
        public static Color White = Common.ColorRGBA(255, 255, 255, 100);
        public static Color LightBlue = Common.ColorRGBA(128, 128, 255, 100);
        public static Color LightRed = Common.ColorRGBA(255, 128, 128, 100);
        public static Color Red = Common.ColorRGBA(255, 0, 0, 100);
        public static Color Green = Common.ColorRGBA(0, 255, 0, 100);
    }



}

public static class Interval
{
    public static float One = 0.01f;
    public static float Two = 0.02f;
    public static float Five = 0.05f;
    public static float Ten = 0.1f;
}

public static class Increment
{
    public static float One = 0.01f;
    public static float Two = 0.02f;
    public static float Five = 0.05f;
    public static float Ten = 0.1f;
}