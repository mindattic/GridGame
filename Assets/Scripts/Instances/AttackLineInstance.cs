using System.Collections;
using UnityEngine;

namespace Game.Instances
{
    public class AttackLineInstance : ExtendedMonoBehavior
    {
        private const string NameFormat = "AttackLine_{0}+{1}";

        //Variables
        public float alpha;
        private Vector3 highestActor;
        private Vector3 lowestActor;
        private float thickness;
        private float maxAlpha;
        private Color baseColor;
        private Color color;
        private LineRenderer lineRenderer;

        #region Components

        public Transform parent
        {
            get => gameObject.transform.parent;
            set => gameObject.transform.SetParent(value, true);
        }

        public Vector3 position
        {
            get => gameObject.transform.position;
            set => gameObject.transform.position = value;
        }

        public int sortingOrder
        {
            get => lineRenderer.sortingOrder;
            set => lineRenderer.sortingOrder = value;
        }

        #endregion


        private void Awake()
        {

            thickness = tileSize * 0.1f;
            alpha = 0f;
            maxAlpha = 1f;
            baseColor = Shared.RGBA(100, 195, 200, 0);


            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.sortingOrder = SortingOrder.AttackLine;
        }

        void Start()
        {
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
        }

        public void Spawn(ActorPair pair)
        {
            parent = board.transform;
            name = NameFormat.Replace("{0}", pair.highestActor.name).Replace("{1}", pair.lowestActor.name);

            highestActor = pair.highestActor.position;
            lowestActor = pair.lowestActor.position;

            lineRenderer.sortingOrder = SortingOrder.AttackLine;
            Vector3[] points = { };

            Vector3 upperLeft;
            Vector3 upperRight;
            Vector3 lowerRight;
            Vector3 lowerLeft;
            float offset = tileSize / 2;

            if (pair.axis == Axis.Vertical)
            {
                upperLeft = new Vector3(highestActor.x - offset, highestActor.y - offset, 0);
                upperRight = new Vector3(highestActor.x + offset, highestActor.y - offset, 0);
                lowerRight = new Vector3(lowestActor.x + offset, lowestActor.y + offset, 0);
                lowerLeft = new Vector3(lowestActor.x - offset, lowestActor.y + offset, 0);

                points = new Vector3[] {
                    upperLeft,
                    upperRight,
                    lowerRight,
                    lowerLeft,
                    upperLeft // Close the loop
                };
            }
            else if (pair.axis == Axis.Horizontal)
            {

                upperLeft = new Vector3(lowestActor.x - offset, lowestActor.y - offset, 0);
                upperRight = new Vector3(highestActor.x + offset, highestActor.y - offset, 0);
                lowerRight = new Vector3(highestActor.x + offset, highestActor.y + offset, 0);
                lowerLeft = new Vector3(lowestActor.x - offset, lowestActor.y + offset, 0);

                points = new Vector3[] {
                    upperLeft,
                    upperRight,
                    lowerRight,
                    lowerLeft,
                    upperLeft // Close the loop
                };
            }

            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);

            IEnumerator _()
            {
                //Before:
                alpha = 0f;
                color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

                //During:
                while (alpha < maxAlpha)
                {
                    alpha += Increment.OnePercent;
                    alpha = Mathf.Clamp(alpha, 0, maxAlpha);
                    color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                    lineRenderer.startColor = color;
                    lineRenderer.endColor = color;

                    yield return Wait.OneTick();
                }

                //After:
                alpha = maxAlpha;
                color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

            }

            StartCoroutine(_());
        }

        public IEnumerator Despawn()
        {
            //Before:
            alpha = maxAlpha;
            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //During:
            while (alpha > 0)
            {
                alpha -= Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, maxAlpha);
                color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

                yield return Wait.OneTick();
            }

            //After:
            alpha = 0;
            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        public void DespawnAsync()
        {
            StartCoroutine(Despawn());
        }


    }
}


