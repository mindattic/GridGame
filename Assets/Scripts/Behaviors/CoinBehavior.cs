using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinBehavior : ExtendedMonoBehavior
{
    public AnimationCurve curve1;
    public AnimationCurve curve2;
    public AnimationCurve curve3;

    public float scaleMultiplier = 0.05f;

    private float duration1 = 0.2f;
    private float duration2 = 0.6f;

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

        duration1 += Random.Float(0, 0.2f);
        duration2 += Random.Float(0, 0.2f);
    }

    public void Spawn(Vector3 position)
    {
        start = position.RandomizeOffset(tileSize * 0.25f);
        end = position.RandomizeOffset(tileSize * 1.5f);

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

                var c = Random.Int(1, 6) == 1 ? curve2: curve1;

                t = Mathf.Clamp01(elapsedTime / duration1);
                x = Mathf.Lerp(start.x, end.x, c.Evaluate(t));
                y = Mathf.Lerp(start.y, end.y, c.Evaluate(t));
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                if (elapsedTime >= duration1)
                {
                    elapsedTime = 0;
                    start = position;
                    end = coinBar.icon.transform.position;
                    state = CoinState.Move;
                }
                break;

            case CoinState.Move:
                t = Mathf.Clamp01(elapsedTime / duration2);
                x = Mathf.Lerp(start.x, end.x, curve3.Evaluate(t));
                y = Mathf.Lerp(start.y, end.y, curve3.Evaluate(t));
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                if (elapsedTime >= duration2)
                {
                    elapsedTime = 0;
                    state = CoinState.Stop;
                }
                break;

            case CoinState.Stop:
                spriteRenderer.enabled = false;
                particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                coinCount++;
                coinBar.textMesh.text = coinCount.ToString("D5");
                state = CoinState.Wait;
                break;

            case CoinState.Wait:
                Destroy(gameObject);
                break;
        }

    }
}
