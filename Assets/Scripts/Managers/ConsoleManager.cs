using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : ExtendedMonoBehavior
{
    private TextMeshProUGUI label;
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
        label = GetComponent<TextMeshProUGUI>();
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
        string fps = $@"{fpsMonitor.current}";
        label.text = $"{fps} FPS" + Environment.NewLine + $"Runtime: {Time.time}";


        //string archetype = HasSelectedPlayer ? selectedPlayer.archetype : "-";
        //string location = HasSelectedPlayer ? $@"({selectedPlayer.location.x},{selectedPlayer.location.y})" : "-";
        //string position = HasSelectedPlayer ? $@"({selectedPlayer.transform.position.x},{selectedPlayer.transform.position.y})" : "-";
        //string mouse2D = mousePosition2D.x >= 0 ? $@"({mousePosition2D.x.ToString("N0").Replace(",", ""):N0},{mousePosition2D.y.ToString("N0").Replace(",", ""):N0})" : "-";
        //string mouse3D = mousePosition3D.x >= -4 ? $@"({mousePosition3D.x.ToString("N0").Replace(",", ""):N0},{mousePosition3D.y.ToString("N0").Replace(",", ""):N0},{mousePosition3D.z.ToString("N0").Replace(", ", ""):N0})" : "-";
        //string attackers = battle.attackers.Any() ? $"[{string.Join(",", battle.attackers.Pickup(x => x.archetype))}]" : "-";
        //string supports = battle.supporters.Any() ? $"[{string.Join(",", battle.supporters.Pickup(x => x.archetype))}]" : "-";
        //string defenders = battle.defenders.Any() ? $"[{string.Join(",", battle.defenders.Pickup(x => x.archetype))}]" : "-";
        //string currentTeam = turnManager != null ? turnManager.currentTeam.ToString() : "-";
        //string currentPhase = turnManager != null ? turnManager.currentPhase.ToString() : "-";


        //string a0 = actors[0] != null ? $"{actors[0].name}: {actors[0].HP}{Environment.NewLine}": $"{Environment.NewLine}";
        //string a1 = actors[1] != null ? $"{actors[1].name}: {actors[1].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a2 = actors[2] != null ? $"{actors[2].name}: {actors[2].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a3 = actors[3] != null ? $"{actors[3].name}: {actors[3].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a4 = actors[4] != null ? $"{actors[4].name}: {actors[4].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a5 = actors[5] != null ? $"{actors[5].name}: {actors[5].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a6 = actors[6] != null ? $"{actors[6].name}: {actors[6].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a7 = actors[7] != null ? $"{actors[7].name}: {actors[7].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a8 = actors[8] != null ? $"{actors[8].name}: {actors[8].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a9 = actors[9] != null ? $"{actors[9].name}: {actors[9].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a10 = actors[10] != null ? $"{actors[10].name}: {actors[10].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a11 = actors[11] != null ? $"{actors[11].name}: {actors[11].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a12 = actors[12] != null ? $"{actors[12].name}: {actors[12].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a13 = actors[13] != null ? $"{actors[13].name}: {actors[13].HP}{Environment.NewLine}" : $"{Environment.NewLine}";


        //label.text = a0 + a1+ a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13;

    }
}
