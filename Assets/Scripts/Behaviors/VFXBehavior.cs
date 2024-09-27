using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
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
    bool isAsync = true;
    float elapsed = 0;
    float duration;
    bool isLoop;
    float triggerEventAt = -1;
    IEnumerator triggeredEvent = null;
    bool hasTriggeredEventStarted;

    //Properties
    private bool hasTriggeredEvent => triggeredEvent != null && triggerEventAt != -1;

    private void Init(VisualEffect vfx, Vector3 position, IEnumerator triggeredEvent = null)
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
        this.triggerEventAt = vfx.triggerEventAt;
        this.triggeredEvent = triggeredEvent;

        //Toggle looping programatically by assigning flag in all child ParticleSystem components
        var particleSystems = new List<ParticleSystem> { GetComponent<ParticleSystem>() };
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>().ToList());
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.loop = isLoop;
        }
    }


    public IEnumerator Spawn(VisualEffect vfx, Vector3 position, IEnumerator triggeredEvent = null)
    {
        isAsync = false;
        Init(vfx, position, triggeredEvent);

        if (!hasTriggeredEvent)
            yield break;

        while (elapsed < triggerEventAt)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return triggeredEvent;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Despawn(name);
    }

    public void SpawnAsync(VisualEffect vfx, Vector3 position, IEnumerator triggeredEvent = null)
    {
        isAsync = true;
        Init(vfx, position, triggeredEvent);
    }

    public void FixedUpdate()
    {
        if (!isAsync)
            return;

        elapsed += Time.deltaTime;

        if (hasTriggeredEvent && !hasTriggeredEventStarted && elapsed >= triggerEventAt)
        {
            hasTriggeredEventStarted = true;
            StartCoroutine(triggeredEvent);
        }

        if (elapsed > duration)
            Despawn(name);
    }

    private void Despawn(string name)
    {
        vfxManager.Despawn(name);
    }

}
