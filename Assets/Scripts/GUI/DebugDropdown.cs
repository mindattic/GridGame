using TMPro;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class DebugDropdown : MonoBehaviour
    {
        protected StageManager stageManager => GameManager.instance.stageManager;
        protected DebugManager debugManager => GameManager.instance.debugManager;

        [SerializeField] private TMP_Dropdown dropdown;

        // Method signature compatible with TMP_Dropdown's OnValueChanged event
        public void OnSelectionChanged(int index)
        {
            if (index < 1)
                return;

            switch (index)
            {
                case 1: stageManager.Load(); break;
                case 2: stageManager.PreviousStage(); break;
                case 3: stageManager.NextStage(); break;
                case 4: debugManager.SpawnRandomEnemy(); break;
            }

            //Don't select, just execute
            dropdown.SetValueWithoutNotify(0);
        }
    }
}
