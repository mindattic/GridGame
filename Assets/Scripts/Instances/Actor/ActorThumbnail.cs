using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ActorThumbnail
{

    //Variables
    GameObject gameObject;
    SpriteRenderer spriteRenderer;
    Sprite sprite;


    public ActorThumbnail(GameObject gameObject)
    {
        this.gameObject = gameObject;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
    }
}