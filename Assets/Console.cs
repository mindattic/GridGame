using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private Text text;
  
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //Vector2 UnscalePosition(Vector2 vec)
    //{
    //    Vector2 referenceResolution = canvas..referenceResolution;
    //    Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

    //    float widthRatio = currentResolution.x / referenceResolution.x;
    //    float heightRatio = currentResolution.y / referenceResolution.y;

    //    float ratio = Mathf.Lerp(heightRatio, widthRatio, canvasScaler.matchWidthOrHeight);

    //    return vec / ratio;
    //}


    // Update is called once per frame
    void Update()
    {
     
    
        text.text 
            = $@"Mouse: {Input.mousePosition.x.ToString("N0").Replace(", ", "")} , {Input.mousePosition.y.ToString("N0").Replace(",", ""):N0}{Environment.NewLine}" 
            + $@"Stat 2";





    }
}
