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

    public void Awake()
    {
        isLoop = GetComponent<ParticleSystem>().main.loop;
    }

    float elapsed = 0;
    float duration;
    bool isLoop;
    bool isPlaying;

    public void Spawn(VisualEffect vfx, Vector3 position)
    {
        //Translate, rotate, and relativeScale relative to tile dimensions (determined by device)
        var offset = Geometry.RelativeTo.Tile.Translation(vfx.relativeOffset);
        var scale = Geometry.RelativeTo.Tile.Scale(vfx.relativeScale);
        var rotation = Geometry.Rotation(vfx.angularRotation);

        this.position = position + offset;
        this.rotation = rotation;
        this.scale = scale;
        this.duration = vfx.duration;
        this.isLoop = vfx.isLoop;

        Init();     
    }

    public void Init()
    {
        //Toggle looping programatically by assigning flag in all child ParticleSystem components
        var particleSystems = new List<ParticleSystem> { GetComponent<ParticleSystem>() };
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>().ToList());
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.loop = isLoop;
        }

        //Start playing particle system
        isPlaying = true;
    }


    public void FixedUpdate()
    {
        Play();
    }

    public void Play()
    {
        if (!isPlaying)
            return;

        elapsed += Time.deltaTime;
        if (elapsed > duration)
        {
            DespawnAsync();
        }
    }

    public void DespawnAsync()
    {
        Destroy(gameObject);
    }

}
