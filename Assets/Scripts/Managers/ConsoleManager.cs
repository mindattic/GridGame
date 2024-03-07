using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : ExtendedMonoBehavior
{
    private Text label;
    private FpsMonitor fpsMonitor = new FpsMonitor();

    #region Components

    public string text
    {
        get => label.text;
        set => label.text = value;
    }


    public Color color
    {
        get => label.color;
        set => label.color = value;
    }

    #endregion

    private void Awake()
    {
        label = GetComponent<Text>();
        //label.font = new Font("Consolas");
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
        //string fps = $@"{fpsMonitor.current}";
        //string id = HasSelectedPlayer ? selectedPlayer.id : "-";
        //string location = HasSelectedPlayer ? $@"({selectedPlayer.location.x},{selectedPlayer.location.y})" : "-";
        //string position = HasSelectedPlayer ? $@"({selectedPlayer.transform.position.x},{selectedPlayer.transform.position.y})" : "-";
        //string mouse2D = mousePosition2D.x >= 0 ? $@"({mousePosition2D.x.ToString("N0").Replace(",", ""):N0},{mousePosition2D.y.ToString("N0").Replace(",", ""):N0})" : "-";
        //string mouse3D = mousePosition3D.x >= -4 ? $@"({mousePosition3D.x.ToString("N0").Replace(",", ""):N0},{mousePosition3D.y.ToString("N0").Replace(",", ""):N0},{mousePosition3D.z.ToString("N0").Replace(", ", ""):N0})" : "-";
        //string attackers = battle.attackers.Any() ? $"[{string.Join(",", battle.attackers.Select(x => x.id))}]" : "-";
        //string supports = battle.supporters.Any() ? $"[{string.Join(",", battle.supporters.Select(x => x.id))}]" : "-";
        //string defenders = battle.defenders.Any() ? $"[{string.Join(",", battle.defenders.Select(x => x.id))}]" : "-";
        string currentTeam = turnManager != null ? turnManager.currentTurn.ToString() : "-";
        string currentPhase = turnManager != null ? turnManager.currentPhase.ToString() : "-";


            label.text = ""
              //    + $@"  Runtime: {Time.time}"
              //    + $@"{Environment.NewLine}"
              //    + $@"      FPS: {fps}"
              //    + $@"{Environment.NewLine}"
              //    + $@" Selected: {id}"
              //    + $@"{Environment.NewLine}"
              //    + $@" Location: {location}"
              //    + $@"{Environment.NewLine}"
              //    + $@"    Mouse: {mouse2D}"
              //    + $@"{Environment.NewLine}"
              //    + $@"Attackers: {attackers}"
              //    + $@"{Environment.NewLine}"
              //    + $@" Supports: {supporters}"
              //    + $@"{Environment.NewLine}"
              //    + $@"Defenders: {defenders}"
              //+ $@"";
              + $@"      Turn: {currentTeam}"
              + $@"{Environment.NewLine}"
              + $@"      Phase: {currentPhase}"
              + $@"{Environment.NewLine}";


    }
}
