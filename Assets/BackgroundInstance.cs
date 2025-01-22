using System.Collections;
using UnityEngine;

public class BackgroundInstance : MonoBehaviour
{

    [SerializeField] public float floatAmplitude = 2f; // How high and low it floats
    [SerializeField] public float floatSpeed = 0.2f; // How fast it floats
    private Vector3 initialPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
        StartCoroutine(Hover());
    }

    private void Initialize()
    {

        initialPosition = transform.position; // Store the starting position

        // Get screen dimensions in world units
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Get the sprite's size in world units
        var spriteRenderer = GetComponent<SpriteRenderer>();
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector2 spriteSize = spriteBounds.size;

        // Calculate scale factors
        float scaleX = screenWidth / spriteSize.x + (screenWidth * 0.01f);
        float scaleY = screenHeight / spriteSize.y + (screenHeight * 0.01f);

        // Apply the larger scale factor to ensure the sprite covers the entire screen
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    IEnumerator Hover()
    {
        float time = 0f;

        while (true) // Infinite loop for continuous floating
        {
            // Calculate the new Y position using a sine wave
            float x = initialPosition.x + Mathf.Sin(time * floatSpeed) * floatAmplitude;
            float y = initialPosition.y + Mathf.Sin(time * floatSpeed) * floatAmplitude;
           
            // Update the transform's position
            transform.position = new Vector3(x, y, initialPosition.z);

            // Increment time
            time += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
    }

}