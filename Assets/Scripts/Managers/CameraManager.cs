using Game.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game.Behaviors
{
    public class CameraWorldSpace
    {
        public Vector3 TopLeft;
        public Vector3 TopRight;
        public Vector3 BottomRight;
        public Vector3 BottomLeft;

        public CameraWorldSpace()
        {
            TopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        }
    }


    public class CameraLocalSpace
    {
        public Vector3 TopLeft;
        public Vector3 TopRight;
        public Vector3 BottomRight;
        public Vector3 BottomLeft;

        public CameraLocalSpace()
        {
            TopRight = new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane);
        }
    }

    public class CameraManager : MonoBehaviour
    {
        public RectFloat viewBounds;
        public Game.Models.RectInt screenBounds;

        public CameraWorldSpace world;
        public CameraLocalSpace local;



        private void Awake()
        {
            world = new CameraWorldSpace();
            local = new CameraLocalSpace();
        }


        void Start()
        {
            //var topLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));
            //var topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            //var bottomRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
            //var bottomLeftt = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));

            //var p = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            //Gizmos.color = color.yellow;
            //Gizmos.DrawSphere(p, 0.1F);


            //viewBounds = new RectFloat();
            //viewBounds.Left = worldVector3.x;
            //viewBounds.Top = worldVector3.y;
            //viewBounds.Left = worldVector3.w;


        }


        void Update()
        {

        }



        public Vector2 ScreenToViewport(Vector2 point, int pixelWidth, int pixelHeight)
        {
            float x = point.x / Camera.main.pixelWidth;
            float y = point.y / Camera.main.pixelHeight;
            return new float2(x, y);
        }


        private void OnDrawGizmos()
        {
            var p = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p, 0.1F);
        }
    }
}