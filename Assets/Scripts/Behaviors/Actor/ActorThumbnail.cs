using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ActorThumbnail
{

    GameObject GameObject;

    SpriteRenderer SpriteRenderer;

    Sprite Sprite;


    public ActorThumbnail(GameObject gameObject)
    {
        this.GameObject = gameObject;
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Sprite = SpriteRenderer.sprite;
    }

    public void Awake()
    {

    }
    public void Start()
    {



    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {

    }



}