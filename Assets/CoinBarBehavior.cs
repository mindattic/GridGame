using TMPro;
using UnityEngine;

public class CoinBarBehavior : ExtendedMonoBehavior
{
    [HideInInspector] public GameObject icon;
    [HideInInspector] public TextMeshPro textMesh;
    [HideInInspector] public GameObject glow;

    public AnimationCurve glowCurve;
    private float scaleMultiplier = 0.05f;

    private float maxGlowScale = 2.5f;

    void Awake()
    {
        icon = gameObject.transform.GetChild(0).gameObject;
        textMesh = gameObject.transform.GetChild(1).GetComponent<TextMeshPro>();
        glow = gameObject.transform.GetChild(2).gameObject;     
    }

    void Start()
    {
        icon.transform.localScale = tileScale * scaleMultiplier;
        glow.transform.localScale = tileScale * scaleMultiplier;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        UpdateGlow();
    }

    private void UpdateGlow()
    {
        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            icon.transform.localScale.x * maxGlowScale * glowCurve.Evaluate(Time.time % glowCurve.length),
            icon.transform.localScale.y * maxGlowScale * glowCurve.Evaluate(Time.time % glowCurve.length),
            1.0f);
        glow.transform.localScale = scale;
    }

}
