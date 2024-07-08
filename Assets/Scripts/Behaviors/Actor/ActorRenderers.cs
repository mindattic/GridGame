using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public Color glowColor;
    public Color baseColor;
    public Color frameColor;
    public Color healthBarColor;
    public Color actionBarColor;

    public SpriteRenderer @base;
    public SpriteRenderer glow;
    public SpriteRenderer parallax;

    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;

    public SpriteRenderer statusIcon;

    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBar;
    public SpriteRenderer healthBarFront;
    public TextMeshPro healthText;

    public SpriteRenderer actionBarBack;
    public SpriteRenderer actionBar;
    public TextMeshPro actionText;

    public SpriteRenderer skillRadialBack;
    public SpriteRenderer skillRadial;
    public TextMeshPro skillRadialText;

    public SpriteRenderer selection;

    public SpriteMask mask;


    public void SetAlpha(float alpha)
    {
        baseColor.a = Mathf.Clamp(alpha, 0, 0.7f);
        @base.color = baseColor;

        parallax.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

        glowColor.a = Mathf.Clamp(alpha, 0, 0.25f);
        glow.color = glowColor;

        thumbnail.color = new Color(1, 1, 1, alpha);

        frameColor.a = Mathf.Clamp(alpha, 0, 1);
        frame.color = new Color(frameColor.r, frameColor.g, frameColor.b, frameColor.a);

        statusIcon.color = new Color(1, 1, 1, alpha);

        healthBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));


        healthBarColor.a = alpha;
        healthBar.color = healthBarColor;
        healthBarFront.color = new Color(1, 1, 1, alpha);
        healthText.color = new Color(1, 1, 1, alpha);

        actionBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));

        actionBarColor.a = alpha;
        actionBar.color = actionBarColor;
        actionText.color = new Color(1, 1, 1, alpha);

        skillRadialBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        skillRadial.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.5f));
        skillRadialText.color = new Color(1, 1, 1, alpha);

        selection.color = new Color(1, 1, 1, alpha);
    }

    public void SetBaseColor(Color color)
    {
        baseColor = new Color(color.r, color.g, color.b, color.a);
        this.@base.color = baseColor;
    }

    public void SetBaseAlpha(float alpha)
    {
        baseColor.a = Mathf.Clamp(alpha, 0, 0.5f);
        this.@base.color = baseColor;
    }

    public void SetBaseScale(Vector3 scale)
    {
        this.@base.transform.localScale = scale;
    }

    public void SetGlowColor(Color color)
    {
        glowColor = new Color(color.r, color.g, color.b, color.a);
        this.glow.color = glowColor;
    }

    public void SetGlowAlpha(float alpha)
    {
        glowColor.a = Mathf.Clamp(alpha, 0, 0.5f);
        this.glow.color = baseColor;
    }

    public void SetGlowScale(Vector3 scale)
    {
        this.glow.transform.localScale = scale;
    }


    //public void SetBloomColor(Color color)
    //{
    //    bloomColor = color;
    //    this.bloom.color = color;

    //    var intensity = (bloomColor.r + bloomColor.g + bloomColor.b) / 3f;
    //    var factor = 1f / intensity;
    //    var emissionColor = new Color(bloomColor.r * factor, bloomColor.g * factor, bloomColor.b * factor, bloomColor.a);
    //    this.bloom.material.SetColor("_EmissionColor", emissionColor);
    //}

    //public void SetBloomAlpha(float alpha)
    //{
    //    bloomColor.a = alpha;
    //    this.bloom.color = bloomColor;

    //    var intensity = (bloomColor.r + bloomColor.g + bloomColor.b) / 3f;
    //    var factor = 1f / intensity;
    //    var emissionColor = new Color(bloomColor.r * factor, bloomColor.g * factor, bloomColor.b * factor, alpha);
    //    this.bloom.material.SetColor("_EmissionColor", emissionColor);
    //}

    public void SetFrameColor(Color color)
    {
        frameColor = new Color(color.r, color.g, color.b, color.a);
        this.frame.color = frameColor;
    }


    public void SetFocus(bool isActive = true)
    {
        selection.gameObject.SetActive(isActive);
    }


    public void SetParallaxSprite(Sprite sprite)
    {
        parallax.sprite = sprite;
    }

    public void SetParallaxMaterial(Material material)
    {
        parallax.material = material;
    }


    public void SetHealthBarColor()
    {

        healthBarColor = new Color(0f, 0.75f, 0.125f);
        healthBar.color = actionBarColor;
    }

    public void SetHealthBarColor(Color color)
    {

        healthBarColor = color;
        healthBar.color = actionBarColor;
    }


    public void SetActionBarColor()
    {

        actionBarColor = new Color(0, 0.35f, 0.75f);
        actionBar.color = actionBarColor;
    }

    public void SetActionBarColor(Color color)
    {

        actionBarColor = color;
        actionBar.color = actionBarColor;
    }


    float shiftSpeed = 0.01f;
    float red = 0.5f;
    float green = 0.5f;
    float blue = 0.5f;
    float amp = 1f;
    float duration = 3f;

    public void CycleActionBarColor()
    {

        //red = (Mathf.Sin(duration * Time.time) * amp) + 1 * 0.5f;
        //green = (Mathf.Sin(duration * Time.time) * amp) + 1 * 0.5f;
        //blue = (Mathf.Sin(duration * Time.time) * amp) + 1 * 0.5f;

        red = Mathf.Clamp(Random.Float(), 0.75f, 1f);
        green = Mathf.Clamp(Random.Float(), 0.75f, 1f);
        blue = Mathf.Clamp(Random.Float(), 0.75f, 1f);

        actionBarColor = new Color(red, green, blue, 1f);
        actionBar.color = actionBarColor;
    }

}

