using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActorArt
{
    public Sprite portrait;
    public Sprite full;
}




public class SpriteManager : MonoBehaviour
{
    [SerializeField] public Sprite sentinel;
    [SerializeField] public Sprite corsair;
    [SerializeField] public Sprite oracle;
    [SerializeField] public Sprite cleric;
    [SerializeField] public Sprite mechanic;
    [SerializeField] public Sprite mercenary;
    [SerializeField] public Sprite paladin;

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
