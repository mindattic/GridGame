using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CoinBehavior : ExtendedMonoBehavior
{
    public AnimationCurve xCurve; // Defines the curve for the X movement
    public AnimationCurve yCurve; // Defines the curve for the Y movement
    public float duration = 1.0f; // Time to complete the movement
    private float elapsedTime = 0.0f;

    private Vector3 start;
    private Vector3 end;

    private bool started = false;

    SpriteRenderer spriteRenderer;



    private void Awake()
    {
        transform.localScale = tileScale * 0.025f;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Spawn(Vector3 position)
    {
        // Set the starting position as the current position of the SpriteRenderer
        start = position;

        // Set the end position as the upper right of the screen
        // Assuming 2D orthographic camera
        Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height, 0));
        end = new Vector3(screenTopRight.x, screenTopRight.y + tileSize, transform.position.z);

        transform.position = start;
        //spriteRenderer.enabled = true;
        started = true;
    }





    void Update()
    {
        if (!started)
            return;

        // Move the sprite along the curve over the specified duration
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / duration); // Normalized time (0 to 1)

        // Interpolate between start and end positions using the curves
        float xPos = Mathf.Lerp(start.x, end.x, xCurve.Evaluate(t));
        float yPos = Mathf.Lerp(start.y, end.y, yCurve.Evaluate(t));

        transform.position = new Vector3(xPos, yPos, transform.position.z);

        // Optional: Reset or loop after reaching the end
        if (t >= 1.0f)
        {
            //Destroy(gameObject);
            //elapsedTime = 0f;
            //xPos = start.x;
            //yPos = start.y;

        }

        if(elapsedTime > duration * 4f)
        {
            Destroy(gameObject);
        }

    }
}
