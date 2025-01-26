using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class DebugButtonPanel : MonoBehaviour
    {
        protected BoardInstance board => GameManager.instance.board;

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
            //// Get the screen position for the BottomCenter of the grid
            Vector2 bottomCenterScreenPosition = board.ScreenCoordinates(ScreenPoint.MiddleCenter);


            var position = Camera.main.ScreenToWorldPoint(bottomCenterScreenPosition);
            //// Convert the screen coordinates to anchored position
            PanelRect.transform.position = position;

            //// Set the panel's pivot to the bottom-center
            //PanelRect.pivot = new Vector2(0, 0);

            //// Anchor the panel to the bottom-center of the screen
            //PanelRect.anchorMin = new Vector2(0.5f, 0);
            //PanelRect.anchorMax = new Vector2(0.5f, 0);

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
