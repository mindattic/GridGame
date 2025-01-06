using UnityEngine;

public class PauseManager : MonoBehaviour
{
    #region Properties
    protected CanvasOverlay canvasOverlay => GameManager.instance.canvasOverlay;

    #endregion


    public bool IsPaused { get; private set; } = false;

    public void Toggle()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        canvasOverlay.TriggerFadeIn("Pause");
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        canvasOverlay.TriggerFadeOut();
    }
}
