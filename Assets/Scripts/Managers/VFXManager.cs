using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : ExtendedMonoBehavior
{
    //Variables
    Dictionary<string, VFXBehavior> visualEffects = new Dictionary<string, VFXBehavior>();

    public void SpawnAsync(VisualEffect vfx, Vector3 position, IEnumerator triggeredEvent = null)
    {
        var prefab = Instantiate(vfx.prefab, Vector2.zero, Quaternion.identity);
        var visualEffect = prefab.GetComponent<VFXBehavior>();
        visualEffect.name = $"VFX_{vfx.id}_{Guid.NewGuid()}";
        visualEffects.Add(visualEffect.name, visualEffect);
        visualEffect.SpawnAsync(vfx, position, triggeredEvent);
    }


    public IEnumerator Spawn(VisualEffect vfx, Vector3 position, IEnumerator triggeredEvent = null)
    {
        var prefab = Instantiate(vfx.prefab, Vector2.zero, Quaternion.identity);
        var visualEffect = prefab.GetComponent<VFXBehavior>();
        visualEffect.name = $"VFX_{vfx.id}_{Guid.NewGuid()}";
        visualEffects.Add(visualEffect.name, visualEffect);

        if (triggeredEvent == null)
            yield break; 

        yield return visualEffect.Spawn(vfx, position, triggeredEvent);
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
