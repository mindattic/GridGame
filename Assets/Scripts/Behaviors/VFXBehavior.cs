using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //Startup
    public void Awake()
    {
        isLoop = GetComponent<ParticleSystem>().main.loop;
    }

    //Variables
    float elapsed = 0;
    float duration;
    bool isLoop;
    float coroutineStart = -1;
    IEnumerator coroutine = null;
    bool hasCoroutineStarted;

    //Properties
    private bool hasCoroutine => coroutine != null && coroutineStart != -1;

    public void Spawn(VisualEffect vfx, Vector3 position, IEnumerator coroutine = null)
    {
        //Translate, rotate, and relativeScale relative to tile dimensions (determined by device)
        var offset = Geometry.Tile.Relative.Translation(vfx.relativeOffset);
        var scale = Geometry.Tile.Relative.Scale(vfx.relativeScale);
        var rotation = Geometry.Rotation(vfx.angularRotation);

        this.position = position + offset;
        this.rotation = rotation;
        this.scale = scale;
        this.duration = vfx.duration;
        this.isLoop = vfx.isLoop;
        this.coroutineStart = vfx.coroutineStart;
        this.coroutine = coroutine;

        //Toggle looping programatically by assigning flag in all child ParticleSystem components
        var particleSystems = new List<ParticleSystem> { GetComponent<ParticleSystem>() };
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>().ToList());
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.loop = isLoop;
        }
    }

    public void FixedUpdate()
    {
        elapsed += Time.deltaTime;

        if (hasCoroutine && !hasCoroutineStarted && elapsed >= coroutineStart)
        {
            hasCoroutineStarted = true;
            StartCoroutine(coroutine);
        }

        if (elapsed > duration)
            Despawn(name);
    }

    private void Despawn(string name)
    {
        vfxManager.Despawn(name);
    }

}
