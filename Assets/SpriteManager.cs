using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public Sprite sentinel;
    public Sprite corsair;
    public Sprite oracle;
   
    void Awake()
    {
        var sprites = GameObject.Find(Constants.Sprites);
       
        sentinel = sprites.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        corsair = sprites.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
        oracle = sprites.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite;

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
