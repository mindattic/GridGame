using UnityEngine;

namespace Game.Models
{
    public class RectInt
    {
        //Variables
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }

        //Properties
        public int Width => Right - Left;
        public int Height => Top - Bottom;
        public Vector2Int Center => new Vector2Int(Width / 2, Height / 2);
        public Vector2Int Size => new Vector2Int(Width, Height);

    }
}
