using System.Collections;
using UnityEngine;

public class VFXBehavior : ExtendedMonoBehavior
{
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

    public Quaternion rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
    }

    #endregion


    float elapsed = 0;
    readonly float duration = Interval.OneSecond;

    public void Spawn(Vector3 position, Vector3 scale)
    {
        this.position = position;
        this.scale = scale;

        IEnumerator _()
        {
            //Before:


            //During:
            while (elapsed < duration)
            {
                elapsed += Interval.OneTick;
                yield return Wait.OneTick();
            }

            //After:
            Destroy(gameObject);

        }

        StartCoroutine(_());


    }

    private void Play()
    {

    }
}
