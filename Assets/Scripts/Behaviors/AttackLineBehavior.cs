using System.Collections;
using UnityEngine;

namespace Game.Behaviors
{
    public class AttackLineBehavior : ExtendedMonoBehavior
    {
        private const string NameFormat = "AttackLine_{0}+{1}";

        //Variables
        public float alpha = 0;
        private Vector3 start;
        private Vector3 end;
        private float thickness = 1.2f;
        private float maxAlpha = 0.5f;
        private Color baseColor = Shared.RGBA(100, 195, 200, 0);
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
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.sortingOrder = SortingOrder.Min;
        }

        void Start()
        {
            lineRenderer.startWidth = tileSize * thickness;
            lineRenderer.endWidth = tileSize * thickness;
        }

        public void Spawn(ActorPair pair)
        {
            parent = board.transform;
            name = NameFormat.Replace("{0}", pair.highestActor.name).Replace("{1}", pair.lowestActor.name);

            start = pair.highestActor.position;
            end = pair.lowestActor.position;

            if (pair.axis == Axis.Vertical)
            {
                start += new Vector3(0, -(tileSize / 2) + -(tileSize * 0.1f), 0);
                end += new Vector3(0, tileSize / 2 + (tileSize * 0.1f), 0);
            }
            else if (pair.axis == Axis.Horizontal)
            {
                start += new Vector3(tileSize / 2 + (tileSize * 0.1f), 0, 0);
                end += new Vector3(-(tileSize / 2) + -(tileSize * 0.1f), 0, 0);
            }

            lineRenderer.sortingOrder = SortingOrder.AttackLine;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

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


