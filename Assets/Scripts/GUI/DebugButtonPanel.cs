using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class DebugButtonPanel : MonoBehaviour
    {
        // Serialize fields to assign buttons in the Inspector
        [SerializeField] private RectTransform PanelRect;
        [SerializeField] private Button ResetButton;
        [SerializeField] private Button PreviousStageButton;
        [SerializeField] private Button NextStageButton;
        [SerializeField] private Button SpawnRandomEnemyButton;

        // Dependencies
        protected StageManager stageManager => GameManager.instance.stageManager;
        protected DebugManager debugManager => GameManager.instance.debugManager;

        private void Start()
        {
            // Ensure the panel RectTransform is properly assigned
            if (PanelRect == null)
            {
                Debug.LogError("PanelRect is not assigned in the Inspector!");
                return;
            }

            PanelRect.transform.position = new Vector3(100, 32, 0);
            PanelRect.anchoredPosition = new Vector2(100, 32);

            // Set the panel's pivot to the bottom-left corner
            PanelRect.pivot = new Vector2(0, 0);

            // Anchor the panel to the bottom-left corner of the screen
            PanelRect.anchorMin = new Vector2(0, 0);
            PanelRect.anchorMax = new Vector2(0, 0);

            // Assign button click listeners
            ResetButton.onClick.AddListener(OnLoadButtonClicked);
            PreviousStageButton.onClick.AddListener(OnPreviousStageButtonClicked);
            NextStageButton.onClick.AddListener(OnNextStageButtonClicked);
            SpawnRandomEnemyButton.onClick.AddListener(OnSpawnRandomEnemyButtonClicked);
        }

        private void OnLoadButtonClicked()
        {
            stageManager.Load();
        }

        private void OnPreviousStageButtonClicked()
        {
            stageManager.PreviousStage();
        }

        private void OnNextStageButtonClicked()
        {
            stageManager.NextStage();
        }

        private void OnSpawnRandomEnemyButtonClicked()
        {
            debugManager.SpawnRandomEnemy();
        }
    }
}
