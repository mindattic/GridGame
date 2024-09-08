using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : ExtendedMonoBehavior
{

    Dictionary<string, VFXBehavior> visualEffects = new Dictionary<string, VFXBehavior>();

    public void Spawn(VisualEffect vfx, Vector3 position, IEnumerator coroutine = null)
    {
        var prefab = Instantiate(vfx.prefab, Vector2.zero, Quaternion.identity);
        var visualEffect = prefab.GetComponent<VFXBehavior>();
        visualEffect.name = $"VFX_{vfx.id}_{Guid.NewGuid()}";
        visualEffects.Add(visualEffect.name, visualEffect);
        visualEffect.Spawn(vfx, position, coroutine);
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
