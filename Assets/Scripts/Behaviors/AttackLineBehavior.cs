using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Behaviors
{
    public class AttackLineBehavior : ExtendedMonoBehavior
    {
        //Variables
        public ActorPair pair;
        public float alpha = 0;
        private  Vector3 start;
        private Vector3 end;
        private float thickness = 1.2f;
        private float maxAlpha = 0.5f;
        private Color baseColor = Colors.RGBA(100, 195, 200, 0);
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


        #endregion


        private void Awake()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
        }

        void Start()
        {
            lineRenderer.startWidth = tileSize * thickness;
            lineRenderer.endWidth = tileSize * thickness;
        }

     
        public void Spawn()
        {
            if (pair == null)
                return;

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

            lineRenderer.sortingOrder = ZAxis.Half;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            IEnumerator _()
            {
                alpha = 0f;
                Color color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

                while (alpha < maxAlpha)
                {
                    alpha += Increment.OnePercent;
                    alpha = Mathf.Clamp(alpha, 0, maxAlpha);
                    color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                    lineRenderer.startColor = color;
                    lineRenderer.endColor = color;

                    yield return Wait.OneTick();
                }
            }

            StartCoroutine(_());
        }

        public IEnumerator Despawn()
        {
            alpha = maxAlpha;
            var color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            while (alpha > 0)
            {
                alpha -= Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, maxAlpha);
                color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

                yield return Wait.OneTick();
            }
        }

        public void DespawnAsync()
        {
            StartCoroutine(Despawn());
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }


    }
}


