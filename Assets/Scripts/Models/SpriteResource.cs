using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    using UnityEngine;
    using UnityEngine.UI;

    [CreateAssetMenu()]
    public class SpriteResource : ScriptableObject
    {
        public string id;
        public Sprite sprite;
    }

}
