using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public Color baseColor = Colors.Solid.White;
    public Color glowColor = Colors.Solid.White;
    public Color parallaxColor = Colors.Solid.White;
    public Color frameColor = Colors.Solid.White;
    public Color healthBarColor = Colors.HealthBar.Green;
    public Color actionBarColor = Colors.ActionBar.Blue;

    public SpriteRenderer quality;
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
        quality.color = baseColor;

        //parallaxColor.a = alpha;
        //parallax.color = new Color(parallaxColor.r, parallaxColor.g, parallaxColor.b, Mathf.Clamp(alpha, 0, 0.5f));

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

    public void SetQualityColor(Color color)
    {
        baseColor = new Color(color.r, color.g, color.b, color.a);
        quality.color = baseColor;
    }

    public void SetQualityAlpha(float alpha)
    {
        baseColor.a = Mathf.Clamp(alpha, 0, 0.5f);
        this.quality.color = baseColor;
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


    public void SetParallaxSprite(Sprite sprite)
    {
        parallax.sprite = sprite;
    }

    public void SetParallaxMaterial(Material material)
    {
        parallax.material = material;
    }


    public void SetParallaxAlpha(float alpha)
    {
        parallaxColor.a = Mathf.Clamp(alpha, 0, 0.7f);
        this.parallax.color = parallaxColor;
    }

    public void SetParallaxSpeed(float xScroll, float yScroll)
    {
 
        this.parallax.material.SetFloat("_XScroll", xScroll);
        this.parallax.material.SetFloat("_YScroll", yScroll);
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


    public void SetSelectionActive(bool isActive = true)
    {
        selection.gameObject.SetActive(isActive);
    }




    public void SetHealthBarColor(Color color)
    {

        healthBarColor = color;
        healthBar.color = actionBarColor;
    }

    public void SetActionBarEnabled(bool isEnabled)
    {
        actionBarBack.enabled = isEnabled;
        actionBar.enabled = isEnabled;
    }

    public void SetActionBarColor(Color color)
    {
        actionBarColor = color;
        actionBar.color = actionBarColor;
    }


    float timer = 0.0f;
    ActionBarColorCycle cycle = ActionBarColorCycle.Phase1;

    public void CycleActionBarColor()
    {
        const float duration = 0.2f;
        timer += Time.deltaTime / duration;

        switch (cycle)
        {
            case ActionBarColorCycle.Phase1: actionBarColor = Colors.ActionBar.Yellow; break;
            case ActionBarColorCycle.Phase2: actionBarColor = Colors.ActionBar.Pink; break;
            case ActionBarColorCycle.Phase3: actionBarColor = Colors.ActionBar.White; break;
            case ActionBarColorCycle.Phase4: actionBarColor = Colors.ActionBar.Blue; break;
        }

        if (timer >= 1f)
        {
            timer = 0f;
            cycle = cycle.Next();
        }

        actionBar.color = actionBarColor;
    }

}


public enum ActionBarColorCycle
{
    Phase1,
    Phase2,
    Phase3,
    Phase4
}

