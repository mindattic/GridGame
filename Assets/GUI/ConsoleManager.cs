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
    private Vector3 mousePosition3D => GameManager.instance.mousePosition3D;

    private void Awake()
    {
        console = GetComponent<Text>();
        //console.font = new Font("Consolas");
    }

    void Start()
    {



    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        var name = activeActor ? activeActor.name : "-";
        var location = activeActor ? $@"({activeActor?.location.x},{activeActor?.location.y})" : "-";
        var position = activeActor ? $@"({activeActor?.transform.position.x},{activeActor?.transform.position.y})" : "-";
        var mouse2D = $@"({mousePosition2D.x.ToString("N0").Replace(", ", "")},{mousePosition2D.y.ToString("N0").Replace(",", ""):N0})";
        var mouse3D = $@"({mousePosition3D.x},{mousePosition3D.y},{mousePosition3D.z})";

        console.text = ""
            + $@"Statistics{Environment.NewLine}"
            + $@"    Runtime: {Time.time}{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            + $@"Actor{Environment.NewLine}"
            + $@"       Name: {name}{Environment.NewLine}"
            + $@"   Location: {location}{Environment.NewLine}"
            + $@"   Position: {position}{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            + $@"Mouse{Environment.NewLine}"
            + $@"         2D: {mouse2D}{Environment.NewLine}"
            + $@"         3D: {mouse3D}{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            + $@"";
    }
}
