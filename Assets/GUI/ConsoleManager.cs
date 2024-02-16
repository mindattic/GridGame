using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private Text console;

    private ActorBehavior activeActor => GameManager.instance.selectedPlayer;
    private Vector3 mousePosition2D => GameManager.instance.mousePosition2D;
    private Vector3 mousePosition3D => GameManager.instance.mousePosition3D;

    private FpsMonitor fpsMonitor = new FpsMonitor();

    private void Awake()
    {
        console = GetComponent<Text>();
        //console.font = new Font("Consolas");
    }

    void Start()
    {
        fpsMonitor.Start();


    }

    void Update()
    {
        fpsMonitor.Update();
    }

    private void FixedUpdate()
    {
        string fps = $@"{fpsMonitor.current}";
        string name = activeActor ? activeActor.name : "-";
        string location = activeActor ? $@"({activeActor?.location.x},{activeActor?.location.y})" : "-";
        string position = activeActor ? $@"({activeActor?.transform.position.x},{activeActor?.transform.position.y})" : "-";
        string mouse2D = $@"({mousePosition2D.x.ToString("N0").Replace(", ", "")},{mousePosition2D.y.ToString("N0").Replace(",", ""):N0})";
        string mouse3D = $@"({mousePosition3D.x},{mousePosition3D.y},{mousePosition3D.z})";
        string attackers = string.Join(Environment.NewLine, GameManager.instance.attackerNames.Select(x => $"   * {x}"));
        string defenders = string.Join(Environment.NewLine, GameManager.instance.defenderNames.Select(x => $"   * {x}"));

        console.text = ""
            + $@"Runtime: {Time.time} seconds"
            //+ $@"        FPS: {fps}{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            //+ $@"       Name: {name}{Environment.NewLine}"
            //+ $@"   Location: {location}{Environment.NewLine}"
            //+ $@"{Environment.NewLine}"
            //+ $@"   Mouse 2D: {mouse2D}{Environment.NewLine}"
            //+ $@"    Mouse3D: {mouse3D}{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            + $@"Attackers:"
            + $@"{Environment.NewLine}"
            + $@"{attackers}"
            + $@"{Environment.NewLine}"
            + $@"{Environment.NewLine}"
            + $@"Defenders:"
            + $@"{Environment.NewLine}"
            + $@"{defenders}"



            + $@"";
    }
}
