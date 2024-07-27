using UnityEngine;

namespace Game.Models
{
    public class RectFloat
    {
        //Variables
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }

        //Properties
        public float Width => Right - Left;
        public float Height => Top - Bottom;
        public Vector2 Center => new Vector2(Width / 2, Height / 2);
        public Vector2 Size => new Vector2(Width, Height);
    }
}
