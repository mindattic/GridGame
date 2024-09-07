using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : ExtendedMonoBehavior
{
    public void Spawn(VisualEffect vfx, Vector3 position)
    {
        var prefab = Instantiate(vfx.prefab, Vector2.zero, Quaternion.identity);
        var visualEffect = prefab.GetComponent<VFXBehavior>();
        visualEffect.name = $"VFX_{vfx.id}_{Guid.NewGuid()}";
        visualEffect.Spawn(vfx, position);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.VFX).ToList().ForEach(x => Destroy(x));
    }

}
