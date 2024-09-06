using System.Collections;
using UnityEngine;

public class VFXBehavior : ExtendedMonoBehavior
{

    bool isLooping;


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


    public void Awake()
    {
        isLooping = GetComponent<ParticleSystem>().main.loop;
    }

    float elapsed = 0;
    float duration;

    public void Spawn(VisualEffect vfx)
    {
        this.position = vfx.position + vfx.offset;
        this.rotation = vfx.rotation;
        this.scale = vfx.scale;
        this.duration = vfx.duration;

        if (isLooping)
            return;

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

    public void Despawn()
    {
        Destroy(gameObject);
    }

}
