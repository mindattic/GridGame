using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActorHealthBar
{

    GameObject healthBarBackGameObject;
    GameObject healthBarGameObject;

    SpriteRenderer healthbarBack;
    SpriteRenderer healthbar;

    public ActorHealthBar(GameObject healthBarBackGameObject, GameObject healthBarGameObject)
    {
        this.healthBarBackGameObject = healthBarBackGameObject;
        this.healthBarGameObject = healthBarGameObject;

        healthbarBack = healthBarBackGameObject.GetComponent<SpriteRenderer>();
        healthbar = healthBarGameObject.GetComponent<SpriteRenderer>();



    }





}
