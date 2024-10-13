using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinBehavior : ExtendedMonoBehavior
{
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;
    public float scaleMultiplier = 0.05f;

    private float d1 = 0.5f;
    private float d2 = 1.0f;
    private float d3 = 5.0f;

    private float elapsedTime = 0.0f;
    private Vector3 start;
    private Vector3 end;
    private CoinState state;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;



    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }


    public Vector3 thumbnailPosition
    {
        get => gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position;
        set => gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position = value;
    }

    public Quaternion rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
    }

    #endregion

    private void Awake()
    {
        transform.localScale = tileScale * scaleMultiplier;
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
    }

    public void Spawn(Vector3 position)
    {
        start = position.RandomizeOffset(tileSize / 4);
        end = start.RandomizeOffset(tileSize * 2);

        transform.position = start;
        state = CoinState.Explode;
    }

    float t;
    float x;
    float y;
    float z;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        switch (state)
        {
            case CoinState.Explode:
                t = Mathf.Clamp01(elapsedTime / d1);
                x = Mathf.Lerp(start.x, end.x, xCurve.Evaluate(t));
                y = Mathf.Lerp(start.y, end.y, yCurve.Evaluate(t));
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                if (elapsedTime >= d1)
                {
                    elapsedTime = 0;
                    
                    start = position;
                    end = new Vector3(
                            cameraManager.world.TopRight.x - tileSize + Random.Float(0, tileSize),
                            cameraManager.world.TopRight.y + tileSize / 2,
                            transform.position.z);


                    state = CoinState.Move;
                }

                break;

            case CoinState.Move:
                t = Mathf.Clamp01(elapsedTime / d2);
                x = Mathf.Lerp(start.x, end.x, xCurve.Evaluate(t));
                y = Mathf.Lerp(start.y, end.y, yCurve.Evaluate(t));
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                if (elapsedTime >= d2)
                {
                    elapsedTime = 0;
                    state = CoinState.Stop;
                }

                break;

            case CoinState.Stop:
                spriteRenderer.enabled = false;
                particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                state = CoinState.Wait;
                break;

            case CoinState.Wait:
                if (elapsedTime >= d3)
                    Destroy(gameObject);
                break;
        }

    }
}
