using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : ExtendedMonoBehavior
{
    private TextMeshProUGUI Label;
    private FpsMonitor FpsMonitor = new FpsMonitor();

    #region Components

    public string Text
    {
        get => Label.text;
        set => Label.text = value;
    }


    public Color Color
    {
        get => Label.color;
        set => Label.color = value;
    }

    #endregion

    private void Awake()
    {
        Label = GetComponent<TextMeshProUGUI>();
        //Label.font = new Font("Consolas");
    }

    void Start()
    {
        FpsMonitor.Start();


    }

    void Update()
    {
        FpsMonitor.Update();
    }

    private void FixedUpdate()
    {
        string fps = $@"{FpsMonitor.Current}";
        Label.text = $"{fps} FPS" + Environment.NewLine + $"Runtime: {Time.time}";


        //string Archetype = HasSelectedPlayer ? FocusedPlayer.Archetype : "-";
        //string Location = HasSelectedPlayer ? $@"({FocusedPlayer.Location.x},{FocusedPlayer.Location.y})" : "-";
        //string Position = HasSelectedPlayer ? $@"({FocusedPlayer.transform.Position.x},{FocusedPlayer.transform.Position.y})" : "-";
        //string mouse2D = MousePosition2D.x >= 0 ? $@"({MousePosition2D.x.ToString("N0").Replace(",", ""):N0},{MousePosition2D.y.ToString("N0").Replace(",", ""):N0})" : "-";
        //string mouse3D = MousePosition3D.x >= -4 ? $@"({MousePosition3D.x.ToString("N0").Replace(",", ""):N0},{MousePosition3D.y.ToString("N0").Replace(",", ""):N0},{MousePosition3D.z.ToString("N0").Replace(", ", ""):N0})" : "-";
        //string attackers = battle.attackers.Any() ? $"[{string.Join(",", battle.attackers.Pickup(x => x.Archetype))}]" : "-";
        //string supports = battle.supporters.Any() ? $"[{string.Join(",", battle.supporters.Pickup(x => x.Archetype))}]" : "-";
        //string defenders = battle.defenders.Any() ? $"[{string.Join(",", battle.defenders.Pickup(x => x.Archetype))}]" : "-";
        //string currentTeam = TurnManager != null ? TurnManager.currentTeam.ToString() : "-";
        //string currentPhase = TurnManager != null ? TurnManager.currentPhase.ToString() : "-";


        //string a0 = Actors[0] != null ? $"{Actors[0].name}: {Actors[0].HP}{Environment.NewLine}": $"{Environment.NewLine}";
        //string a1 = Actors[1] != null ? $"{Actors[1].name}: {Actors[1].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a2 = Actors[2] != null ? $"{Actors[2].name}: {Actors[2].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a3 = Actors[3] != null ? $"{Actors[3].name}: {Actors[3].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a4 = Actors[4] != null ? $"{Actors[4].name}: {Actors[4].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a5 = Actors[5] != null ? $"{Actors[5].name}: {Actors[5].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a6 = Actors[6] != null ? $"{Actors[6].name}: {Actors[6].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a7 = Actors[7] != null ? $"{Actors[7].name}: {Actors[7].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a8 = Actors[8] != null ? $"{Actors[8].name}: {Actors[8].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a9 = Actors[9] != null ? $"{Actors[9].name}: {Actors[9].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a10 = Actors[10] != null ? $"{Actors[10].name}: {Actors[10].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a11 = Actors[11] != null ? $"{Actors[11].name}: {Actors[11].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a12 = Actors[12] != null ? $"{Actors[12].name}: {Actors[12].HP}{Environment.NewLine}" : $"{Environment.NewLine}";
        //string a13 = Actors[13] != null ? $"{Actors[13].name}: {Actors[13].HP}{Environment.NewLine}" : $"{Environment.NewLine}";


        //Label.Text = a0 + a1+ a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13;

    }
}
