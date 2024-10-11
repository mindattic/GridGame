using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    public AnimationCurve xCurve; // Defines the curve for the X movement
    public AnimationCurve yCurve; // Defines the curve for the Y movement
    public float duration = 2.0f; // Time to complete the movement
    private float elapsedTime = 0.0f;

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        // Set the starting position as the current position of the SpriteRenderer
        startPos = transform.position;

        // Set the end position as the upper right of the screen
        // Assuming 2D orthographic camera
        Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height, 0));
        endPos = new Vector3(screenTopRight.x, screenTopRight.y, transform.position.z);
    }

    void Update()
    {
        // Move the sprite along the curve over the specified duration
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / duration); // Normalized time (0 to 1)

        // Interpolate between start and end positions using the curves
        float xPos = Mathf.Lerp(startPos.x, endPos.x, xCurve.Evaluate(t));
        float yPos = Mathf.Lerp(startPos.y, endPos.y, yCurve.Evaluate(t));

      
        // Optional: Reset or loop after reaching the end
        if (t >= 1.0f)
        {
            elapsedTime = 0f;
            xPos = startPos.x;
            yPos = startPos.y;

        }

        transform.position = new Vector3(xPos, yPos, transform.position.z);

        //Destroy(gameObject);
    }
}
