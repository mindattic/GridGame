using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    //Variables
    Dictionary<string, VFXInstance> visualEffects = new Dictionary<string, VFXInstance>();

    public void TriggerSpawn(VisualEffect vfx, Vector3 position, Trigger trigger = default)
    {
        if (trigger == default)
            trigger = new Trigger();

        var prefab = Instantiate(vfx.prefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<VFXInstance>();
        instance.name = $"VFX_{vfx.id}Attack{Guid.NewGuid()}";
        visualEffects.Add(instance.name, instance);
        instance.TriggerSpawn(vfx, position, trigger);
    }


    public IEnumerator Spawn(VisualEffect vfx, Vector3 position, Trigger trigger = default)
    {
        if (trigger == default)
            trigger = new Trigger();

        var prefab = Instantiate(vfx.prefab, Vector2.zero, Quaternion.identity);
        var visualEffect = prefab.GetComponent<VFXInstance>();
        visualEffect.name = $"VFX_{vfx.id}_Attack_{Guid.NewGuid()}";
        visualEffects.Add(visualEffect.name, visualEffect);

        yield return visualEffect.Spawn(vfx, position, trigger);
    }


    public void Despawn(string name)
    {
        Destroy(visualEffects[name].gameObject);
        visualEffects.Remove(name);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.VFX).ToList().ForEach(x => Destroy(x));
    }

}
