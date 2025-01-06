using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXInstance : MonoBehaviour
{
    #region Properties
    protected VFXManager vfxManager => GameManager.instance.vfxManager;

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

    public IEnumerator Spawn(VisualEffect vfx, Vector3 position, Trigger trigger = default)
    {
        if (trigger == default)
            trigger = new Trigger();

        float delay = trigger.GetAttribute("delay", 0f);
        float duration = trigger.GetAttribute("duration", 0f);

        //Translate, rotate, and relativeScale relative to tile dimensions (determined by device)
        var offset = Geometry.Tile.Relative.Translation(vfx.relativeOffset);
        var scale = Geometry.Tile.Relative.Scale(vfx.relativeScale);
        var rotation = Geometry.Rotation(vfx.angularRotation);

        this.position = position + offset;
        this.rotation = rotation;
        this.scale = scale;

        //Toggle looping programatically by assigning flag in all child ParticleSystem components
        var ps = new List<ParticleSystem> { GetComponent<ParticleSystem>() };
        ps.AddRange(GetComponentsInChildren<ParticleSystem>().ToList());
        foreach (var x in ps)
        {
            var main = x.main;
            main.loop = vfx.isLoop;
        }

        //Wait until delay is over
        if (vfx.delay != 0f)
            yield return new WaitForSeconds(delay);

        //Trigger coroutine (if applicable)
        yield return trigger.StartCoroutine(this);

        //Wait until VFX duration completes
        if (vfx.duration != 0f)
            yield return Wait.For(duration);

        //Despawn VFX
        Despawn(name);
    }

    private void Despawn(string name)
    {
        vfxManager.Despawn(name);
    }

}
