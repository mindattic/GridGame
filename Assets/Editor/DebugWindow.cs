using Game.Behaviors;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] //This attribute ensures that the static constructor is called on load
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
    private ProfileManager profileManager;

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
        if (instance == null)
            return;

        instance.Close();
        instance = null;
        isWindowOpen = false;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredPlayMode)
            return;

        //Close the window when entering play mode
        if (isWindowOpen)
            CloseWindow();

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
        profileManager = GameManager.instance.profileManager;

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
            Repaint(); //Repaint the window
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
            || profileManager == null)
            return;

        GUILayout.BeginVertical();

        RenderStats();
        RenderCheckboxes();
        RenderDebugOptionsDropdown();
        RenderVFXDropdown();
        RenderLevelControls();
        RenderDataControls();
        //RenderActorStats();
        RenderSpawnControls();
        RenderLog();

        GUILayout.EndVertical();
    }


    private void RenderStats()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label($"FPS: {consoleManager.fpsMonitor.currentFps}", GUILayout.Width(Screen.width * 0.25f));
        GUILayout.Label($"Turn: {(turnManager.IsPlayerTurn ? "Player" : "Enemy")}", GUILayout.Width(Screen.width * 0.25f));
        GUILayout.Label($"Phase: {turnManager.currentPhase}", GUILayout.Width(Screen.width * 0.25f));
        GUILayout.Label($"Runtime: {Time.time:F2}", GUILayout.Width(Screen.width * 0.25f));

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void RenderCheckboxes()
    {
        bool isChecked;

        GUILayout.BeginHorizontal();

        //Show Actor Name Tag checkbox
        isChecked = EditorGUILayout.Toggle("Show Actor Name Tag", gameManager.showActorNameTag);
        if (gameManager.showActorNameTag != isChecked)
        {
            gameManager.showActorNameTag = isChecked;
            gameManager.actors.ForEach(x => x.renderers.SetNameTagEnabled(isChecked));
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        //Show Actor Frame checkbox
        isChecked = EditorGUILayout.Toggle("Show Actor Frame", gameManager.showActorFrame);
        if (gameManager.showActorFrame != isChecked)
        {
            gameManager.showActorFrame = isChecked;
            gameManager.actors.ForEach(x => x.renderers.SetFrameEnabled(isChecked));
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void RenderDebugOptionsDropdown()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Debug Options", GUILayout.Width(Screen.width * 0.25f));
        selectedOption = (DebugOptions)EditorGUILayout.EnumPopup(selectedOption, GUILayout.Width(Screen.width * 0.5f));
        if (GUILayout.Button("Execute", GUILayout.Width(Screen.width * 0.25f)))
            OnRunClick();

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void RenderVFXDropdown()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("VFX", GUILayout.Width(Screen.width * 0.25f));
        selectedVfx = (VFXOptions)EditorGUILayout.EnumPopup(selectedVfx, GUILayout.Width(Screen.width * 0.5f));
        if (GUILayout.Button("Play", GUILayout.Width(Screen.width * 0.25f)))
            OnPlayVFXClick();

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void RenderLevelControls()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset", GUILayout.Width(Screen.width * 0.3333f)))
            OnResetClick();

        if (GUILayout.Button("< Previous", GUILayout.Width(Screen.width * 0.3333f)))
            OnPreviousLevelClick();

        if (GUILayout.Button("Next >", GUILayout.Width(Screen.width * 0.3333f)))
            OnNextLevelClick();

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void RenderDataControls()
    {
        bool isClicked;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Data", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        isClicked = GUILayout.Button("Erase Database", GUILayout.Width(Screen.width * 0.5f));
        if (isClicked)
            OnEraseDatabaseClick();

        isClicked = GUILayout.Button("Erase Profiles", GUILayout.Width(Screen.width * 0.5f));

        if (isClicked)
            OnEraseProfilesClick();

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }



    private void RenderActorStats()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Actors", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();

        foreach(var x in gameManager.players.OrderBy(x => x.name))
        {
            GUILayout.BeginHorizontal();
            string stats = $"{x.name}: HP: {x.HP}, IsActive: {x.isActiveAndEnabled}, IsAlive: {x.IsAlive}, IsDying: {x.IsDying}";
            GUILayout.Label(stats, GUILayout.Width(Screen.width));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        foreach (var x in gameManager.enemies.OrderBy(x => x.name))
        {
            GUILayout.BeginHorizontal();
            string stats = $"{x.name}: HP: {x.HP}, IsActive: {x.isActiveAndEnabled}, IsAlive: {x.IsAlive}, IsDying: {x.IsDying}";
            GUILayout.Label(stats, GUILayout.Width(Screen.width));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
    }





    private void RenderSpawnControls()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Spawn", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();

        bool isClicked;

        GUILayout.BeginHorizontal();

        isClicked = GUILayout.Button("Slime", GUILayout.Width(Screen.width * 0.25f));
        if (isClicked)
            debugManager.SpawnSlime();


        isClicked = GUILayout.Button("Bat", GUILayout.Width(Screen.width * 0.25f));
        if (isClicked)
            debugManager.SpawnBat();

        isClicked = GUILayout.Button("Scorpion", GUILayout.Width(Screen.width * 0.25f));
        if (isClicked)
            debugManager.SpawnScorpion();


        isClicked = GUILayout.Button("Yeti", GUILayout.Width(Screen.width * 0.25f));
        if (isClicked)
            debugManager.SpawnYeti();

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }






    private void RenderLog()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Log", GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
  
        //Background color setup
        var backgroundColor = new Color(0.5f, 0.15f, 0.15f);
        var style = new GUIStyle { richText = true, padding = new RectOffset(10, 10, 10, 10) };

        //Calculate the background area
        float logHeight = position.height - 170;
        Rect backgroundRect = new Rect(0, GUILayoutUtility.GetLastRect().yMax, Screen.width, logHeight);

        //Draw the background
        Color originalColor = GUI.color;
        GUI.color = backgroundColor;
        GUI.Box(backgroundRect, GUIContent.none); //Draw the background box
        GUI.color = originalColor;

        //Make the log scrollable
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition,
            GUILayout.Height(logHeight),
            GUILayout.ExpandHeight(true));

        //Display the logs
        GUILayout.Label(logManager.text, style);

        GUILayout.EndScrollView();
        GUILayout.Space(10);
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
            case DebugOptions.ShakeTest: debugManager.ShakeTest(); break;
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

    //Blank click events for the buttons
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

    private void OnEraseDatabaseClick()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "MyDatabase.db");

        if (!File.Exists(filePath))
        {
            logManager.Error("MyDatabase.db does not exist at the specified path.");
            return;
        }

        File.Delete(filePath);
        logManager.Info("MyDatabase.db has been deleted.");
    }

    private void OnEraseProfilesClick()
    {
        logManager.Info("Not yet implemented.");
    }

}
