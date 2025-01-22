using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    #region Properties
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected CanvasOverlay canvasOverlay => GameManager.instance.canvasOverlay;
    public bool IsPaused => Time.timeScale == 0f;
    #endregion

    private Image buttonImage;
    private Sprite pause;
    private Sprite paused;


    void Awake()
    {
        pause = resourceManager.Sprite("Pause").sprite;
        paused = resourceManager.Sprite("Paused").sprite;

        GameObject pauseButton = GameObject.Find("PauseButton");
        buttonImage = pauseButton.GetComponent<Image>();
        buttonImage.sprite = pause;
    }


    public void Toggle()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        buttonImage.sprite = paused;
        canvasOverlay.Show("Paused");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        buttonImage.sprite = pause;
        canvasOverlay.Reset();
        Time.timeScale = 1f;
    }
}
