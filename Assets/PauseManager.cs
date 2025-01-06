using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    #region Properties
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected CanvasOverlay canvasOverlay => GameManager.instance.canvasOverlay;
    public bool IsPaused => Time.timeScale == 0f;
    #endregion

    public Image pauseButtonImage;



    void Awake()
    {
        GameObject pauseButton = GameObject.Find("PauseButton");
        pauseButtonImage = pauseButton.GetComponent<Image>();
        pauseButtonImage = GetComponent<Image>();
        pauseButtonImage.sprite = resourceManager.Sprite("Pause").sprite;
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
        pauseButtonImage.sprite = resourceManager.Sprite("Paused").sprite;
        canvasOverlay.Show("Pause");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseButtonImage.sprite = resourceManager.Sprite("Pause").sprite;
        canvasOverlay.Hide();
        Time.timeScale = 1f;
    }
}
