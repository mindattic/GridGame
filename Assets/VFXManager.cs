using System;
using UnityEngine;

public class VFXManager : ExtendedMonoBehavior
{
    public void Spawn(string id, Vector3 position, Vector3 scale)
    {
        var gameObject = resourceManager.VisualEffect(id);
        var prefab = Instantiate(gameObject, Vector2.zero, Quaternion.identity);
        var vfxBehavior = prefab.GetComponent<VFXBehavior>();
        vfxBehavior.name = $"VFX_{id}_{Guid.NewGuid()}";
        vfxBehavior.parent = board.transform;
        vfxBehavior.Spawn(position, scale);
    }

}
