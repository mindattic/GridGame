using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class VFXInstance : MonoBehaviour
{
    protected VFXManager vfxManager => GameManager.instance.vfxManager;


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
    Trigger trigger = default;

    //Properties
    //private bool hasTrigger => trigger != null && trigger.IsValid && triggerAt != -1;

    private void Initialize(VisualEffect vfx, Vector3 position, Trigger trigger = default)
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
        this.trigger = trigger;

        //Toggle looping programatically by assigning flag in all child ParticleSystem components
        var particleSystems = new List<ParticleSystem> { GetComponent<ParticleSystem>() };
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>().ToList());
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.loop = isLoop;
        }
    }

    public void TriggerSpawn(VisualEffect vfx, Vector3 position, Trigger trigger = default)
    {
        if (trigger == default)
            trigger = new Trigger();

        Initialize(vfx, position, trigger);
        StartCoroutine(Spawn(vfx, position, trigger));
    }

    public IEnumerator Spawn(VisualEffect vfx, Vector3 position, Trigger trigger = default)
    {
        if (trigger == default)
            trigger = new Trigger();

        Initialize(vfx, position, trigger);

        // Wait until delay is over
        if (trigger.Delay > 0)
            yield return new WaitForSeconds(trigger.Delay);

        // Execute the trigger (if any)
        if (trigger.IsValid)
            yield return trigger.Start(this);

        // Wait until VFX duration completes
        yield return new WaitForSeconds(duration);

        // Despawn the VFX
        Despawn(name);
    }



    private void Despawn(string name)
    {
        vfxManager.Despawn(name);
    }

}
