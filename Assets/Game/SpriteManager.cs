using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActorArt
{
    public Sprite portrait;
    public Sprite full;
}

public class SpriteManager : ExtendedMonoBehavior
{
    [SerializeField] public Sprite paladin;
    [SerializeField] public Sprite barbarian;
    [SerializeField] public Sprite ninja;
    [SerializeField] public Sprite cleric;
    [SerializeField] public Sprite sentinel;
    [SerializeField] public Sprite pandagirl;
    
    [SerializeField] public Sprite slime;
    [SerializeField] public Sprite bat;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
