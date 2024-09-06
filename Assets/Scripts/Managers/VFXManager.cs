using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : ExtendedMonoBehavior
{
    //public List<VFXBehavior> visualEffects = new List<VFXBehavior>();

    public void Spawn(VisualEffect vfx)
    {
        var prefab = Instantiate(vfx.gameObject, Vector2.zero, Quaternion.identity);
        var visualEffect = prefab.GetComponent<VFXBehavior>();
        visualEffect.name = $"VFX_{vfx.id}_{Guid.NewGuid()}";
        //visualEffect.parent = board.transform;
        //visualEffects.Add(visualEffect);
        visualEffect.Spawn(vfx);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.VFX).ToList().ForEach(x => Destroy(x));
    }


}
