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



    void Awake()
    {
        GameObject pauseButton = GameObject.Find("PauseButton");
        buttonImage = pauseButton.GetComponent<Image>();
        buttonImage.sprite = resourceManager.Sprite("Pause").sprite;
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
        buttonImage.sprite = resourceManager.Sprite("Paused").sprite;
        canvasOverlay.Show("Pause");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        buttonImage.sprite = resourceManager.Sprite("Pause").sprite;
        canvasOverlay.Hide();
        Time.timeScale = 1f;
    }
}
