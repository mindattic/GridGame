﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class StringExtensions
{
    public static string SanitizeFileName(this string src)
    {
        //Trim and replace spaces
        src = src.Trim().Replace(" ", "-");

        //Remove illegal characters for Windows, iOS, and Android
        System.IO.Path.GetInvalidFileNameChars().ToList().ForEach(c => src = src.Replace(c.ToString(), ""));

        return src;
    }
}

public static class TransformExtensions
{
    public static Transform GetChild(this Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            // Recursively search in the child hierarchy
            Transform found = child.GetChild(childName);
            if (found != null)
                return found;
        }
        return null; // Return null if no matching child is found
    }
}

public static class EnumExtensions
{
    public static T Next<T>(this T src) where T : struct
    {
        var values = (T[])Enum.GetValues(src.GetType());
        int index = Array.IndexOf<T>(values, src) + 1;
        return (values.Length == index) ? values[0] : values[index];
    }

    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }


}


public static class ListExtensions
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        return list.OrderBy(x => Guid.NewGuid()).ToList();
    }
}

public static class Vector2IntExtensions
{
    public static void Shift(this ref Vector2Int vector, int x, int y)
    {
        vector.x += x;
        vector.y += y;
    }
}


public static class Vector3Extensions
{
    public static Vector3 SetX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 AddX(this Vector3 v, float x)
    {
        return new Vector3(v.x + x, v.y, v.z);
    }

    public static Vector3 SubtractX(this Vector3 v, float x)
    {
        return new Vector3(v.x - x, v.y, v.z);
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 AddY(this Vector3 v, float y)
    {
        return new Vector3(v.x, v.y + y, v.z);
    }

    public static Vector3 SubtractY(this Vector3 v, float y)
    {
        return new Vector3(v.x, v.y - y, v.z);
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 AddZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, v.z + z);
    }

    public static Vector3 SubtractZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, v.z - z);
    }

    public static bool HasNaN(this Vector3 v)
    {
        return v.x == float.NaN || v.y == float.NaN || v.z == float.NaN;
    }

    public static Vector3 RandomizeOffset(this Vector3 v, float amount)
    {
        return new Vector3(v.x + Random.Float(-amount, amount), v.y + Random.Float(-amount, amount), v.z);
    }
}

public static class ColorExtensions
{

    public static Color SetA(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, c.a);
    }

    public static Color AddA(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, c.a + a);
    }
}

public static class IntExtensions
{
    public static float ToFloat(this int i)
    {
        return (float)i;
    }
}

public static class FloatExtensions
{
    public static int ToInt(this float f)
    {
        return (int)f;
    }
}

//public static class BooleanExtensions
//{
//    public static bool IsNot(this bool b, bool value)
//    {
//        return b != value;
//    }

//    public static bool Toggle(this bool b, bool value)
//    {
//        if (b == value)
//            return false;

//        b = value;
//        return true;
//    }

//}

static class StopwatchExtensions
{
    /// <summary>
    /// Gets estimated time on compleation. 
    /// </summary>
    /// <param src="sw"></param>
    /// <param src="counter"></param>
    /// <param src="counterGoal"></param>
    /// <returns></returns>
    public static TimeSpan GetEta(this Stopwatch sw, int counter, int counterGoal)
    {
        /* this is based off of:
         * (TimeTaken / linesProcessed) * linesLeft=timeLeft
         * so we have
         * (10/100) * 200 = 20 Seconds now 10 seconds go past
         * (20/100) * 200 = 40 Seconds left now 10 more seconds and we process 100 more lines
         * (30/200) * 100 = 15 Seconds and now we all see why the copy file dialog jumps from 3 hours to 30 minutes :-)
         * 
         * pulled from http://stackoverflow.com/questions/473355/calculate-time-remaining/473369#473369
         */
        if (counter == 0) return TimeSpan.Zero;
        float elapsedMin = ((float)sw.ElapsedMilliseconds / 1000) / 60;
        float minLeft = (elapsedMin / counter) * (counterGoal - counter); //see comment a
        TimeSpan ret = TimeSpan.FromMinutes(minLeft);
        return ret;
    }
}

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
            _ => Direction.None
        };
    }
}




