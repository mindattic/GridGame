using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private Text console;
  
    private void Awake()
    {
        console = GetComponent<Text>();
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

        console.text
            = $@"Mouse 2D: {Global.instance.mousePosition2D.x.ToString("N0").Replace(", ", "")} , {Global.instance.mousePosition2D.y.ToString("N0").Replace(",", ""):N0}{Environment.NewLine}"
            + $@"Mouse 3D: {Global.instance.mousePosition3D.x.ToString("N0").Replace(", ", "")} , {Global.instance.mousePosition3D.y.ToString("N0").Replace(",", ""):N0} , {Global.instance.mousePosition3D.z.ToString("N0").Replace(",", ""):N0}{Environment.NewLine}"
            + $@"";





    }
}
