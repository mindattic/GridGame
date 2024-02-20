using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : ExtendedMonoBehavior
{
    [SerializeField] private Canvas canvas;

    private Text console;
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
        string name = HasSelectedPlayer ? selectedPlayer.name : "-";
        string location = HasSelectedPlayer ? $@"({selectedPlayer.location.x},{selectedPlayer.location.y})" : "-";
        string position = HasSelectedPlayer ? $@"({selectedPlayer.transform.position.x},{selectedPlayer.transform.position.y})" : "-";
        string mouse2D = mousePosition2D.x >= 0 ? $@"({mousePosition2D.x.ToString("N0").Replace(",", ""):N0},{mousePosition2D.y.ToString("N0").Replace(",", ""):N0})" : "-";
        string mouse3D = mousePosition3D.x >= -4 ? $@"({mousePosition3D.x.ToString("N0").Replace(",", ""):N0},{mousePosition3D.y.ToString("N0").Replace(",", ""):N0},{mousePosition3D.z.ToString("N0").Replace(", ", ""):N0})" : "-";
        string attackers = battle.attackers.Any() ? $"[{string.Join(",", battle.attackers.Select(x => x.name))}]" : "-";
        string supports = battle.supports.Any() ? $"[{string.Join(",", battle.supports.Select(x => x.name))}]" : "-";
        string defenders = battle.defenders.Any() ? $"[{string.Join(",", battle.defenders.Select(x => x.name))}]" : "-";

        
        console.text = ""
            + $@"  Runtime: {Time.time}"
            + $@"{Environment.NewLine}"
            + $@"      FPS: {fps}"
            + $@"{Environment.NewLine}"
            + $@" Selected: {name}"
            + $@"{Environment.NewLine}"
            + $@" Location: {location}"
            + $@"{Environment.NewLine}"
            + $@"    Mouse: {mouse2D}"
            + $@"{Environment.NewLine}"
            + $@"Attackers: {attackers}"
            + $@"{Environment.NewLine}"
            + $@" Supports: {supports}"
            + $@"{Environment.NewLine}"
            + $@"Defenders: {defenders}"
            + $@"";
    }
}
