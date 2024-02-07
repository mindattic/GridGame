using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private Text console;

    private ActorBehavior activeActor => GameManager.instance.activeActor;
    private Vector3 mousePosition2D => GameManager.instance.mousePosition2D;

    private void Awake()
    {
        console = GetComponent<Text>();
        //console.font = new Font("Consolas");
    }

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
        //console.text
        //    = $@"Mouse 2D: {GameManager.instance.mousePosition2D.x.ToString("N0").Replace(", ", "")} , {GameManager.instance.mousePosition2D.y.ToString("N0").Replace(",", ""):N0}{Environment.NewLine}"
        //    + $@"Mouse 3D: {GameManager.instance.mousePosition3D.x.ToString("N0").Replace(", ", "")} , {GameManager.instance.mousePosition3D.y.ToString("N0").Replace(",", ""):N0} , {GameManager.instance.mousePosition3D.z.ToString("N0").Replace(",", ""):N0}{Environment.NewLine}"
        //    + $@"";

        console.text
            = $@"Selected Actor: {activeActor?.name ?? ""}{Environment.NewLine}"
            + $@"      Location: {activeActor?.location.x},{activeActor?.location.y}{Environment.NewLine}"
            + $@"      Mouse 2D: {mousePosition2D.x.ToString("N0").Replace(", ", "")},{mousePosition2D.y.ToString("N0").Replace(",", ""):N0}{Environment.NewLine}"
            + $@"";



    }
}
