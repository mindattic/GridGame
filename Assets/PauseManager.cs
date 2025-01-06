using UnityEngine;

public class PauseManager : MonoBehaviour
{
    #region Properties
    protected CanvasOverlay canvasOverlay => GameManager.instance.canvasOverlay;
    public bool IsPaused => Time.timeScale == 0f;
    #endregion




    public void Toggle()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {       
        canvasOverlay.Show("Pause");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        canvasOverlay.Hide();
        Time.timeScale = 1f;
    }
}
