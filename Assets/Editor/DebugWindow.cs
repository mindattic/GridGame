using Game.Behaviors;
using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] // This attribute ensures that the static constructor is called on load
public class DebugWindow : EditorWindow
{
    private static DebugWindow instance;
    private static bool isWindowOpen = false;

    private Vector2 scrollPosition;
    private DateTime lastUpdateTime;
    private float updateInterval = 1.0f;

    private GameManager gameManager;
    private DebugManager debugManager;
    private ConsoleManager consoleManager;
    private TurnManager turnManager;
    private StageManager stageManager;
    private LogManager logManager;
    private SaveFileManager saveFileManager;

    private DebugOptions selectedOption = DebugOptions.None;
    private VFXOptions selectedVfx = VFXOptions.None;

    static DebugWindow()
    {
        //Subscribe to the play mode state change event
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem("Window/Debug Window")]
    public static void ShowWindow()
    {
        instance = GetWindow<DebugWindow>("Debug Window");
        isWindowOpen = true;
    }

    public static void CloseWindow()
    {
        if (instance != null)
        {
            instance.Close();
            instance = null;
            isWindowOpen = false;
        }
    }


    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // Close the window when entering play mode
            if (isWindowOpen)
            {
                CloseWindow();
            }

            // Reopen after 3 seconds
            EditorApplication.delayCall += () =>
            {
                EditorApplication.delayCall += () =>
                {
                    if (EditorApplication.isPlaying) // Ensure still in play mode
                    {
                        ShowWindow();
                    }
                };
            };
        }
    }

    private void OnEnable()
    {
        instance = this;
        isWindowOpen = true;

        //Create references to manager instances
        gameManager = GameManager.instance;
        debugManager = GameManager.instance.debugManager;
        consoleManager = GameManager.instance.consoleManager;
        turnManager = GameManager.instance.turnManager;
        stageManager = GameManager.instance.stageManager;
        logManager = GameManager.instance.logManager;
        saveFileManager = GameManager.instance.saveFileManager;

        //Set initial flags
        gameManager.showActorNameTag = false;
        gameManager.showActorFrame = false;

        //Register the update method
        EditorApplication.update += OnEditorUpdate;
        lastUpdateTime = DateTime.Now;
    }

    private void OnDisable()
    {
        isWindowOpen = false;
        instance = null;

        // Unregister the update method
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
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
            || logManager == null 
            || saveFileManager == null)
            return;

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        RenderStats();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        RenderCheckboxes();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        RenderDebugOptionsDropdown();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        RenderVFXDropdown();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        RenderLevelControls();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Save File", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        RenderSaveControls();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        RenderCombatLog();
        GUILayout.EndVertical();
    }


    private void RenderStats()
    {
        GUILayout.Label($"FPS: {consoleManager.fpsMonitor.currentFps}");
        GUILayout.Label($"Turn: {(turnManager.IsPlayerTurn ? "Player" : "Enemy")}");
        GUILayout.Label($"Phase: {turnManager.currentPhase}");
        GUILayout.Label($"Runtime: {Time.time:F2}");
    }

    private void RenderCheckboxes()
    {
        //Show Actor Name Tag checkbox
        var isEnabled = EditorGUILayout.Toggle("Show Actor Name Tag", gameManager.showActorNameTag);
        if (gameManager.showActorNameTag != isEnabled)
        {
            gameManager.showActorNameTag = isEnabled;
            gameManager.actors.ForEach(x => x.renderers.SetNameTagEnabled(isEnabled));
        }

        //Show Actor Frame checkbox
        isEnabled = EditorGUILayout.Toggle("Show Actor Frame", gameManager.showActorFrame);
        if (gameManager.showActorFrame != isEnabled)
        {
            gameManager.showActorFrame = isEnabled;
            gameManager.actors.ForEach(x => x.renderers.SetFrameEnabled(isEnabled));
        }

    }

    private void RenderDebugOptionsDropdown()
    {
        GUILayout.Label("Debug Options", GUILayout.Width(Screen.width * 0.25f));
        selectedOption = (DebugOptions)EditorGUILayout.EnumPopup(selectedOption, GUILayout.Width(Screen.width * 0.5f));
        if (GUILayout.Button("Execute", GUILayout.Width(Screen.width * 0.25f)))
            OnRunClick();
    }

    private void RenderVFXDropdown()
    {
        GUILayout.Label("VFX", GUILayout.Width(Screen.width * 0.25f));
        selectedVfx = (VFXOptions)EditorGUILayout.EnumPopup(selectedVfx, GUILayout.Width(Screen.width * 0.5f));
        if (GUILayout.Button("Play", GUILayout.Width(Screen.width * 0.25f)))
            OnPlayVFXClick();
    }

    private void RenderLevelControls()
    {

        if (GUILayout.Button("Reset", GUILayout.Width(Screen.width * 0.3333f)))
            OnResetClick();

        if (GUILayout.Button("< Previous", GUILayout.Width(Screen.width * 0.3333f)))
            OnPreviousLevelClick();

        if (GUILayout.Button("Next >", GUILayout.Width(Screen.width * 0.3333f)))
            OnNextLevelClick();
    }

    private void RenderSaveControls()
    {
        if (GUILayout.Button("Save", GUILayout.Width(Screen.width * 0.5f)))
            OnSaveClick();

        if (GUILayout.Button("LoadSaveFiles", GUILayout.Width(Screen.width * 0.5f)))
            OnReloadClick();
    }

    private void RenderCombatLog()
    {

        // 5. Display the logs (scrollable)
        //var backgroundColor = new Color(0.15f, 0.15f, 0.15f); // Darker background color
        var style = new GUIStyle { richText = true };
        var y = position.height - 200;
        //EditorGUI.DrawRect(new Rect(0, y, boardPosition.width, boardPosition.height), backgroundColor);

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
            case DebugOptions.DodgeTest: debugManager.DodgeTest(); break;
            case DebugOptions.SpinTest: debugManager.SpinTest(); break;
            case DebugOptions.AlignTest: debugManager.AlignTest(); break;
            case DebugOptions.CoinTest: debugManager.CoinTest(); break;
            case DebugOptions.PortraitTest: debugManager.PortraitTest(); break;
            case DebugOptions.DamageTextTest: debugManager.DamageTextTest(); break;
            case DebugOptions.BumpTest: debugManager.BumpTest(); break;
            case DebugOptions.SupportLineTest: debugManager.SupportLineTest(); break;
            case DebugOptions.AttackLineTest: debugManager.AttackLineTest(); break;

            case DebugOptions.EnemyAttackTest: debugManager.EnemyAttackTest(); break;
            case DebugOptions.TitleTest: debugManager.TitleTest(); break;
            case DebugOptions.TooltipTest: debugManager.TooltipTest(); break;
            default: Debug.LogWarning("OnRunClick failed."); break;
        }
    }

    // Blank click events for the buttons
    private void OnResetClick()
    {
        stageManager.Load();
    }

    private void OnPreviousLevelClick()
    {
        stageManager.PreviousStage();
    }

    private void OnNextLevelClick()
    {
        stageManager.NextStage();
    }

    private void OnSaveClick()
    {
        saveFileManager.QuickSave();
    }

    private void OnReloadClick()
    {
        var success = saveFileManager.Reload();
        if (!success)
            return;


    }



}
