using UnityEngine;
using UnityEditor;
using Game.Behaviors;
using System;

[InitializeOnLoad] // This attribute ensures that the static constructor is called on load
public class DebugWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private string logText = "";
    private DateTime lastUpdateTime;
    private float updateInterval = 1.0f; // Startup interval in seconds

    

    //Properties

    private bool showActorNameTag
    {
        get { return GameManager.instance.showActorNameTag; }
        set { GameManager.instance.showActorNameTag = value; }
    }


    // Static constructor to open the Debug Window when Unity loads
    static DebugWindow()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredPlayMode)
            return;

        EditorApplication.update += startup;

        void startup()
        {
            float startupDelay = 3f;
            if (EditorApplication.timeSinceStartup < startupDelay)
                return;

            ShowWindow();
            EditorApplication.update -= startup;
        }     
    }

    private enum VFXOptions
    {
        None,
        VFXTest_Blue_Slash_01,
        VFXTest_Blue_Slash_02,
        VFXTest_Blue_Slash_03,
        VFXTest_Blue_Sword,
        VFXTest_Blue_Sword_4X,
        VFXTest_Blood_Claw,
        VFXTest_Level_Up,
        VFXTest_Yellow_Hit,
        VFXTest_Double_Claw,
        VFXTest_Lightning_Explosion,
        VFXTest_Buff_Life,
        VFXTest_Rotary_Knife,
        VFXTest_Air_Slash,
        VFXTest_Fire_Rain,
        VFXTest_Ray_Blast,
        VFXTest_Lightning_Strike,
        VFXTest_Puffy_Explosion,
        VFXTest_Red_Slash_2X,
        VFXTest_God_Rays,
        VFXTest_Acid_Splash,
        VFXTest_Green_Buff,
        VFXTest_Gold_Buff,
        VFXTest_Hex_Shield,
        VFXTest_Toxic_Cloud,
        VFXTest_Orange_Slash,
        VFXTest_Moon_Feather,
        VFXTest_Pink_Spark,
        VFXTest_BlueYellow_Sword,
        VFXTest_BlueYellow_Sword_3X,
        VFXTest_Red_Sword
    }

    private enum Options
    {
        None,
        DodgeTest,
        SpinTest,
        AlignTest,
        CoinTest,
        PortraitTest,
        DamageTextTest,
        BumpTest,
        SupportLineTest,
        EnemyAttackTest,
        TitleTest,
        TooltipTest
    }

    private VFXOptions selectedVfx = VFXOptions.None;
    private Options selectedOption = Options.None;


    private DebugManager debugManager;
    private ConsoleManager consoleManager;
    private TurnManager turnManager;
    private StageManager stageManager;
    private LogManager logManager;

    [MenuItem("Window/Debug Window")]
    public static void ShowWindow()
    {
        GetWindow<DebugWindow>("Debug Window");
    }

    private void OnEnable()
    {
        // Find the DebugManager, ConsoleManager, and TurnManager in the scene
        debugManager = GameManager.instance.debugManager;
        consoleManager = GameManager.instance.consoleManager;
        turnManager = GameManager.instance.turnManager;
        stageManager = GameManager.instance.stageManager;
        logManager = GameManager.instance.logManager;

        // Register the update method
        EditorApplication.update += OnEditorUpdate;
        lastUpdateTime = DateTime.Now;
    }

    private void OnDisable()
    {
        // Unregister the update method
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {

        if ((DateTime.Now - lastUpdateTime).TotalSeconds >= updateInterval)
        {
            lastUpdateTime = DateTime.Now;
            Repaint(); // Repaint the window
        }



    }

    private void OnGUI()
    {
        //Check abort state
        if (debugManager == null
            || consoleManager == null
            || turnManager == null
            || stageManager == null
            || logManager == null)
            return;

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        RenderLabels();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        RenderCheckboxes();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        RenderVFXDropdown();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        RenderOptionDropdown();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        RenderLevelControls();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        RenderCombatLog();

        GUILayout.EndVertical();
    }


    private void RenderLabels()
    {
        GUILayout.Label($"FPS: {consoleManager.fpsMonitor.currentFps}");
        GUILayout.Label($"Turn: {(turnManager.IsPlayerTurn ? "Player" : "Enemy")}");
        GUILayout.Label($"Phase: {turnManager.currentPhase}");
        GUILayout.Label($"Runtime: {Time.time:F2}");
    }


    private void RenderCheckboxes()
    {
        var previousFlag = showActorNameTag;
        showActorNameTag = EditorGUILayout.Toggle("Show Actor Name Tag", showActorNameTag);
        if (showActorNameTag != previousFlag)
        {
            GameManager.instance.actors.ForEach(x => x.renderers.SetNameTagEnabled(showActorNameTag));
        }
    }



    private void RenderVFXDropdown()
    {
        GUILayout.Label("VFX", GUILayout.Width(Screen.width * 0.25f));
        selectedVfx = (VFXOptions)EditorGUILayout.EnumPopup(selectedVfx, GUILayout.Width(Screen.width * 0.5f));
        if (GUILayout.Button("Play", GUILayout.Width(Screen.width * 0.15f)))
            OnPlayVFXClick();
    }

    private void RenderOptionDropdown()
    {
        GUILayout.Label("Options", GUILayout.Width(Screen.width * 0.25f));
        selectedOption = (Options)EditorGUILayout.EnumPopup(selectedOption, GUILayout.Width(Screen.width * 0.5f));
        if (GUILayout.Button("Run", GUILayout.Width(Screen.width * 0.15f)))
            OnRunClick();
    }

    private void RenderLevelControls()
    {

        if (GUILayout.Button("Reset", GUILayout.Width(Screen.width * 0.3f)))
            OnResetClick();

        if (GUILayout.Button("< Previous", GUILayout.Width(Screen.width * 0.3f)))
            OnPreviousClick();

        if (GUILayout.Button("Next >", GUILayout.Width(Screen.width * 0.3f)))
            OnNextClick();
    }


    private void RenderCombatLog()
    {

        // 5. Display the logs (scrollable)
        //var backgroundColor = new Color(0.15f, 0.15f, 0.15f); // Darker background color
        var style = new GUIStyle { richText = true };
        var y = position.height - 200;
        //EditorGUI.DrawRect(new Rect(0, y, position.width, position.height), backgroundColor);

        scrollPosition = EditorGUILayout.BeginScrollView(
            scrollPosition,
            GUILayout.Height(y),
            GUILayout.ExpandHeight(true));

        GUILayout.Label(logManager.text, style);
        EditorGUILayout.EndScrollView();

    }


    private void OnPlayVFXClick()
    {
        switch (selectedVfx)
        {
            case VFXOptions.VFXTest_Blue_Slash_01: debugManager.VFXTest_Blue_Slash_01(); break;
            case VFXOptions.VFXTest_Blue_Slash_02: debugManager.VFXTest_Blue_Slash_02(); break;
            case VFXOptions.VFXTest_Blue_Slash_03: debugManager.VFXTest_Blue_Slash_03(); break;
            case VFXOptions.VFXTest_Blue_Sword: debugManager.VFXTest_Blue_Sword(); break;
            case VFXOptions.VFXTest_Blue_Sword_4X: debugManager.VFXTest_Blue_Sword_4X(); break;
            case VFXOptions.VFXTest_Blood_Claw: debugManager.VFXTest_Blood_Claw(); break;
            case VFXOptions.VFXTest_Level_Up: debugManager.VFXTest_Level_Up(); break;
            case VFXOptions.VFXTest_Yellow_Hit: debugManager.VFXTest_Yellow_Hit(); break;
            case VFXOptions.VFXTest_Double_Claw: debugManager.VFXTest_Double_Claw(); break;
            case VFXOptions.VFXTest_Lightning_Explosion: debugManager.VFXTest_Lightning_Explosion(); break;
            case VFXOptions.VFXTest_Buff_Life: debugManager.VFXTest_Buff_Life(); break;
            case VFXOptions.VFXTest_Rotary_Knife: debugManager.VFXTest_Rotary_Knife(); break;
            case VFXOptions.VFXTest_Air_Slash: debugManager.VFXTest_Air_Slash(); break;
            case VFXOptions.VFXTest_Fire_Rain: debugManager.VFXTest_Fire_Rain(); break;
            case VFXOptions.VFXTest_Ray_Blast: debugManager.VFXTest_Ray_Blast(); break;
            case VFXOptions.VFXTest_Lightning_Strike: debugManager.VFXTest_Lightning_Strike(); break;
            case VFXOptions.VFXTest_Puffy_Explosion: debugManager.VFXTest_Puffy_Explosion(); break;
            case VFXOptions.VFXTest_Red_Slash_2X: debugManager.VFXTest_Red_Slash_2X(); break;
            case VFXOptions.VFXTest_God_Rays: debugManager.VFXTest_God_Rays(); break;
            case VFXOptions.VFXTest_Acid_Splash: debugManager.VFXTest_Acid_Splash(); break;
            case VFXOptions.VFXTest_Green_Buff: debugManager.VFXTest_Green_Buff(); break;
            case VFXOptions.VFXTest_Gold_Buff: debugManager.VFXTest_Gold_Buff(); break;
            case VFXOptions.VFXTest_Hex_Shield: debugManager.VFXTest_Hex_Shield(); break;
            case VFXOptions.VFXTest_Toxic_Cloud: debugManager.VFXTest_Toxic_Cloud(); break;
            case VFXOptions.VFXTest_Orange_Slash: debugManager.VFXTest_Orange_Slash(); break;
            case VFXOptions.VFXTest_Moon_Feather: debugManager.VFXTest_Moon_Feather(); break;
            case VFXOptions.VFXTest_Pink_Spark: debugManager.VFXTest_Pink_Spark(); break;
            case VFXOptions.VFXTest_BlueYellow_Sword: debugManager.VFXTest_BlueYellow_Sword(); break;
            case VFXOptions.VFXTest_BlueYellow_Sword_3X: debugManager.VFXTest_BlueYellow_Sword_3X(); break;
            case VFXOptions.VFXTest_Red_Sword: debugManager.VFXTest_Red_Sword(); break;
            default: Debug.LogWarning("OnPlayVFXClick failed."); break;
        }
    }

    private void OnRunClick()
    {
        switch (selectedOption)
        {
            case Options.DodgeTest: debugManager.DodgeTest(); break;
            case Options.SpinTest: debugManager.SpinTest(); break;
            case Options.AlignTest: debugManager.AlignTest(); break;
            case Options.CoinTest: debugManager.CoinTest(); break;
            case Options.PortraitTest: debugManager.PortraitTest(); break;
            case Options.DamageTextTest: debugManager.DamageTextTest(); break;
            case Options.BumpTest: debugManager.BumpTest(); break;
            case Options.SupportLineTest: debugManager.SupportLineTest(); break;
            case Options.EnemyAttackTest: debugManager.EnemyAttackTest(); break;
            case Options.TitleTest: debugManager.TitleTest(); break;
            case Options.TooltipTest: debugManager.TooltipTest(); break;
            default: Debug.LogWarning("OnRunClick failed."); break;
        }
    }

    // Blank click events for the buttons
    private void OnResetClick()
    {
        stageManager.Load();
    }

    private void OnPreviousClick()
    {
        stageManager.PreviousStage();
    }

    private void OnNextClick()
    {
        stageManager.NextStage();
    }
}
