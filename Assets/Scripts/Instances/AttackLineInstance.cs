using System.Collections;
using UnityEngine;

namespace Game.Instances
{
    public class AttackLineInstance : MonoBehaviour
    {
        protected float tileSize => GameManager.instance.tileSize;
        protected BoardInstance board => GameManager.instance.board;


        private const string NameFormat = "AttackLine_{0}+{1}";

        //Variables
        public float alpha;
        private Vector3 originActor;
        private Vector3 terminalActor;
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

            thickness = tileSize * 0.02f;
            alpha = 0f;
            maxAlpha = 1f;
            baseColor = ColorHelper.RGBA(100, 195, 200, 0);

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
            name = NameFormat.Replace("{0}", pair.originActor.name).Replace("{1}", pair.terminalActor.name);

            originActor = pair.originActor.position;
            terminalActor = pair.terminalActor.position;

            
            Vector3[] points = { };

            Vector3 upperLeft;
            Vector3 upperRight;
            Vector3 lowerRight;
            Vector3 lowerLeft;
            float offset = tileSize / 2;

            if (pair.axis == Axis.Vertical)
            {
                upperLeft = new Vector3(originActor.x - offset, originActor.y - offset, 0);
                upperRight = new Vector3(originActor.x + offset, originActor.y - offset, 0);
                lowerRight = new Vector3(terminalActor.x + offset, terminalActor.y + offset, 0);
                lowerLeft = new Vector3(terminalActor.x - offset, terminalActor.y + offset, 0);

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

                upperLeft = new Vector3(terminalActor.x - offset, terminalActor.y - offset, 0);
                upperRight = new Vector3(originActor.x + offset, originActor.y - offset, 0);
                lowerRight = new Vector3(originActor.x + offset, originActor.y + offset, 0);
                lowerLeft = new Vector3(terminalActor.x - offset, terminalActor.y + offset, 0);

                points = new Vector3[] {
                    upperLeft,
                    upperRight,
                    lowerRight,
                    lowerLeft,
                    upperLeft // Close the loop
                };
            }

            lineRenderer.sortingOrder = SortingOrder.AttackLine;
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
                    alpha = Mathf.Clamp(alpha, Opacity.Transparent, maxAlpha);
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

        public void TriggerDespawn()
        {
            StartCoroutine(Despawn());
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
                alpha = Mathf.Clamp(alpha, Opacity.Transparent, maxAlpha);
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

        


    }
}


