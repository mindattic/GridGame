using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public static class Constants
{
    public const string Game = "Game";
    public const string Board = "Board";
    public const string Canvas2D = "Canvas2D";
    public const string Canvas3D = "Canvas3D";
    public const string Art = "Art";
    public const string Console = "Console";
    public const string Log = "Log";
    public const string Overlay = "Overlay";
    public const string Title = "Title";
    public const string Card = "Card";
    public const string TimerBar = "TimerBar";
   
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


    //Card
    public const string CardBack = "Card/Back";
    public const string CardProfile = "Card/Profile";
    public const string CardTitle = "Card/Title";
    public const string CardDetails = "Card/Details";

    //Audio sources
    public const int SoundSourceIndex = 0;
    public const int MusicSourceIndex = 1;
}

public static class Location
{
    public static readonly Vector2Int Nowhere = new Vector2Int(-1, -1);

    public static readonly Vector2Int A1 = new Vector2Int(1, 1);
    public static readonly Vector2Int A2 = new Vector2Int(1, 2);
    public static readonly Vector2Int A3 = new Vector2Int(1, 3);
    public static readonly Vector2Int A4 = new Vector2Int(1, 4);
    public static readonly Vector2Int A5 = new Vector2Int(1, 5);
    public static readonly Vector2Int A6 = new Vector2Int(1, 6);

    public static readonly Vector2Int B1 = new Vector2Int(2, 1);
    public static readonly Vector2Int B2 = new Vector2Int(2, 2);
    public static readonly Vector2Int B3 = new Vector2Int(2, 3);
    public static readonly Vector2Int B4 = new Vector2Int(2, 4);
    public static readonly Vector2Int B5 = new Vector2Int(2, 5);
    public static readonly Vector2Int B6 = new Vector2Int(2, 6);

    public static readonly Vector2Int C1 = new Vector2Int(3, 1);
    public static readonly Vector2Int C2 = new Vector2Int(3, 2);
    public static readonly Vector2Int C3 = new Vector2Int(3, 3);
    public static readonly Vector2Int C4 = new Vector2Int(3, 4);
    public static readonly Vector2Int C5 = new Vector2Int(3, 5);
    public static readonly Vector2Int C6 = new Vector2Int(3, 6);

    public static readonly Vector2Int D1 = new Vector2Int(4, 1);
    public static readonly Vector2Int D2 = new Vector2Int(4, 2);
    public static readonly Vector2Int D3 = new Vector2Int(4, 3);
    public static readonly Vector2Int D4 = new Vector2Int(4, 4);
    public static readonly Vector2Int D5 = new Vector2Int(4, 5);
    public static readonly Vector2Int D6 = new Vector2Int(4, 6);

    public static readonly Vector2Int E1 = new Vector2Int(5, 1);
    public static readonly Vector2Int E2 = new Vector2Int(5, 2);
    public static readonly Vector2Int E3 = new Vector2Int(5, 3);
    public static readonly Vector2Int E4 = new Vector2Int(5, 4);
    public static readonly Vector2Int E5 = new Vector2Int(5, 5);
    public static readonly Vector2Int E6 = new Vector2Int(5, 6);

    public static readonly Vector2Int F1 = new Vector2Int(6, 1);
    public static readonly Vector2Int F2 = new Vector2Int(6, 2);
    public static readonly Vector2Int F3 = new Vector2Int(6, 3);
    public static readonly Vector2Int F4 = new Vector2Int(6, 4);
    public static readonly Vector2Int F5 = new Vector2Int(6, 5);
    public static readonly Vector2Int F6 = new Vector2Int(6, 6);

    public static readonly Vector2Int G1 = new Vector2Int(7, 1);
    public static readonly Vector2Int G2 = new Vector2Int(7, 2);
    public static readonly Vector2Int G3 = new Vector2Int(7, 3);
    public static readonly Vector2Int G4 = new Vector2Int(7, 4);
    public static readonly Vector2Int G5 = new Vector2Int(7, 5);
    public static readonly Vector2Int G6 = new Vector2Int(7, 6);

    public static readonly Vector2Int H1 = new Vector2Int(8, 1);
    public static readonly Vector2Int H2 = new Vector2Int(8, 2);
    public static readonly Vector2Int H3 = new Vector2Int(8, 3);
    public static readonly Vector2Int H4 = new Vector2Int(8, 4);
    public static readonly Vector2Int H5 = new Vector2Int(8, 5);
    public static readonly Vector2Int H6 = new Vector2Int(8, 6);
}

public static class Tag
{
    public static string Board = "board";
    public static string Tile = "Tile";
    public static string Actor = "Actor";
    public static string SupportLine = "SupportLine";
    public static string AttackLine = "AttackLine";
    public static string Trail = "Trail";
    public static string Select = "Select";
    public static string DamageText = "DamageText";
    public static string AnnouncementText = "AnnouncementText";
    public static string Portrait = "ActorPortrait";
    public static string Ghost = "Ghost";
    public static string Footstep = "Footstep";
    public static string Wall = "Wall";
    public static string Tooltip = "Tooltip";

}

public static class Colors
{
    public static Color RGB(float r, float g, float b) => Shared.RGB(r, g, b);
    public static Color RGBA(float r, float g, float b, float a) => Shared.RGBA(r, g, b, a);
  

    public static class Solid
    {
        public static Color Gold = RGB(255, 215, 0);
        public static Color Black = RGB(0, 0, 0);
        public static Color Gray = RGB(128, 128, 128);
        public static Color White = RGB(255, 255, 255);
        public static Color LightBlue = RGB(128, 128, 255);
        public static Color LightRed = RGB(255, 128, 128);
        public static Color Red = RGB(255, 0, 0);
        public static Color Green = RGB(0, 255, 0);
    }

    public static class HealthBar
    {
        public static Color Green = new Color(0f, 0.75f, 0.125f);

    }

    public static class ActionBar
    {
        public static Color Blue = new Color(0, 0.35f, 0.75f);
        public static Color Yellow = Color.yellow;
        public static Color Pink = RGB(100, 75, 80);
        public static Color White = Color.white;
    }

    public static class Translucent
    {
        public static Color Gold = RGBA(255, 215, 0, 128);
        public static Color White = RGBA(255, 255, 255, 128);
        public static Color Black = RGBA(0, 0, 0, 128);
        public static Color LightBlue = RGBA(128, 128, 255, 128);
        public static Color LightRed = RGBA(255, 128, 128, 128);
        public static Color Red = RGBA(255, 0, 0, 128);
        public static Color Green = RGBA(0, 255, 0, 128);
    }

    public static class Transparent
    {
        public static Color White = RGBA(255, 255, 255, 0);
        public static Color Red = RGBA(255, 0, 0, 0);
    }

}

public static class Qualities
{
    public static Quality Junk = new Quality("Junk", Shared.RGB(128, 128, 128));
    public static Quality Common = new Quality("Common", Shared.RGB(255, 255, 255));
    public static Quality Uncommon = new Quality("Uncommon", Shared.RGB(30, 255, 0));
    public static Quality Rare = new Quality("Rare", Shared.RGB(0, 112, 221));
    public static Quality Epic = new Quality("Epic", Shared.RGB(163, 53, 238));
    public static Quality Legendary = new Quality("Legendary", Shared.RGB(255, 128, 0));
}

public static class Interval
{
    public static float OneTick = 0.01f;
    public static float FiveTicks = 0.05f;
    public static float TenTicks = 0.1f;
    public static float QuarterSecond = 0.25f;
    public static float HalfSecond = 0.5f;
    public static float OneSecond = 1.0f;
    public static float TwoSeconds = 2.0f;
    public static float ThreeSeconds = 3.0f;
    public static float FourSeconds = 4.0f;
    public static float FiveSeconds = 5.0f;
}

public static class Increment
{
    public static float OnePercent = 0.01f;
    public static float TwoPercent = 0.02f;
    public static float FivePercent = 0.05f;
    public static float TenPercent = 0.1f;
    public static float FiftyPercent = 0.5f;
    public static float HundredPercent = 1.0f;
}


public static class Wait
{
    public static WaitForSeconds OneTick() => new WaitForSeconds(Interval.OneTick * GameManager.instance.gameSpeed);
    public static WaitForSeconds Ticks(int amount) => new WaitForSeconds(Interval.OneTick * amount * GameManager.instance.gameSpeed);

    public static WaitForSeconds For(float seconds) => new WaitForSeconds(seconds * GameManager.instance.gameSpeed);
    public static WaitForSeconds None() => new WaitForSeconds(0);



    public static IEnumerator Duration(float amount)
    {
        float ticks = 0f;
        while (ticks < amount)
        {
            ticks += Interval.OneTick;
            yield return OneTick();
        }
    }

}




public static class SortingOrder
{
    public const int Min = 0;
    public const int Default = 100;
    public const int SupportLine = 110;
    public const int AttackLine = 110;
    public const int Defender = 120;
    public const int Supporter = 140;
    public const int Attacker = 150;
    public const int Max = 999;
}



public static class NameFormat
{
    public static string AttackLine(ActorPair pair)
        => "AttackLine_{0}+{1}".Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);

    public static string SupportLine(ActorPair pair)
        => "SupportLine_{0}+{1}".Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);

}
