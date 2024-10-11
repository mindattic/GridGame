using UnityEngine;

public class CoinBehavior : ExtendedMonoBehavior
{
    public AnimationCurve xCurve; 
    public AnimationCurve yCurve;
    public float scaleMultiplier = 0.05f;

    private float duration = 1.0f;
    private float lifetime = 5.0f;
    private float elapsedTime = 0.0f;
    private Vector3 start;
    private Vector3 end;
    private CoinState state;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;

    private void Awake()
    {
        transform.localScale = tileScale * scaleMultiplier;
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
    }

    public void Spawn(Vector3 position)
    {
        start = position.RandomizeOffset(tileSize / 4);
        transform.position = start;

        end = new Vector3(
            cameraManager.world.TopRight.x - tileSize + Random.Float(0, tileSize), 
            cameraManager.world.TopRight.y + tileSize / 2, 
            transform.position.z);

        state = CoinState.Move;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        switch (state)
        {
            case CoinState.Move:
                float t = Mathf.Clamp01(elapsedTime / duration);
                float x = Mathf.Lerp(start.x, end.x, xCurve.Evaluate(t));
                float y = Mathf.Lerp(start.y, end.y, yCurve.Evaluate(t));
                float z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                if (elapsedTime >= duration)
                    state = CoinState.Stop;
                break;

            case CoinState.Stop:
                spriteRenderer.enabled = false;
                particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                state = CoinState.Wait;
                break;

            case CoinState.Wait:
                if (elapsedTime >= lifetime)
                    Destroy(gameObject);
                break;
        }

    }
}
