using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public Color opaqueColor = Colors.Solid.White;
    public Color qualityColor = Colors.Solid.White;
    public float qualityAlphaMax = 1.0f;
    public Color glowColor = Colors.Solid.White;
    public Color parallaxColor = Colors.Solid.White;
    public float parallaxAlphaMax = 0.5f;
    public Color thumbnailColor = Colors.Solid.White;
    public Color frameColor = Colors.Solid.White;
    public Color healthBarColor = Colors.HealthBar.Green;
    public Color actionBarColor = Colors.ActionBar.Blue;
    public Color turnDelayColor = Colors.Solid.Red;
    public Color weaponIconColor = Colors.Solid.White;
    public Color selectionColor = Colors.Solid.White;

    public SpriteRenderer opaque;
    public SpriteRenderer quality;
    public SpriteRenderer glow;
    public SpriteRenderer parallax;
    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;
    public SpriteRenderer statusIcon;
    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBarDrain;
    public SpriteRenderer healthBar;    
    public TextMeshPro healthText;
    public SpriteRenderer actionBarBack;
    public SpriteRenderer actionBar;
    public TextMeshPro actionText;
    public SpriteRenderer skillRadialBack;
    public SpriteRenderer skillRadial;
    public TextMeshPro skillRadialText;
    public SpriteMask mask;
    public TextMeshPro turnDelayText;
    public TextMeshPro nameTagText;
    public SpriteRenderer weaponIcon;
    public SpriteRenderer selectionBox;

    public void SetAlpha(float alpha)
    {
        SetOpaqueAlpha(alpha);
        SetQualityAlpha(alpha);
        SetGlowAlpha(alpha);
        //SetParallaxAlpha(alpha);
        SetThumbnailAlpha(alpha);
        SetFrameAlpha(alpha);
        //statusIcon.color = new color(1, 1, 1, alpha);
        SetHealthBarAlpha(alpha);
        SetActionBarAlpha(alpha);
        SetSkillRadialAlpha(alpha);
        SetTurnDelayTextAlpha(alpha);
        SetNameTagTextAlpha(alpha);
        SetWeaponIconAlpha(alpha);
        SetSelectionAlpha(alpha);
    }

    public void SetOpaqueAlpha(float alpha)
    {
        opaqueColor.a = Mathf.Clamp(alpha, 0, 1);
        opaque.color = opaqueColor;
    }

    public void SetQualityColor(Color color)
    {
        qualityColor = new Color(color.r, color.g, color.b, Mathf.Clamp(color.a, 0, qualityAlphaMax));
        quality.color = qualityColor;
    }


    public void SetQualityAlpha(float alpha)
    {
        qualityColor.a = Mathf.Clamp(alpha, 0, qualityAlphaMax);
        this.quality.color = qualityColor;
    }

    public void SetGlowColor(Color color)
    {
        glowColor = new Color(color.r, color.g, color.b, color.a);
        this.glow.color = glowColor;
    }

    public void SetGlowAlpha(float alpha)
    {
        glowColor.a = Mathf.Clamp(alpha, 0, 0.5f);
        this.glow.color = qualityColor;
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
        parallaxColor.a = Mathf.Clamp(alpha, 0, parallaxAlphaMax);
        this.parallax.color = parallaxColor;
    }

    public void SetParallaxSpeed(float xScroll, float yScroll)
    {

        this.parallax.material.SetFloat("_XScroll", xScroll);
        this.parallax.material.SetFloat("_YScroll", yScroll);
    }

    public void SetThumbnailAlpha(float alpha)
    {
        thumbnailColor.a = Mathf.Clamp(alpha, 0, 1);
        thumbnail.color = thumbnailColor;
    }

    public void SetThumbnailMaterial(Material material)
    {
        thumbnail.material = material;
    }

    public void SetThumbnailSprite(Sprite sprite)
    {
        thumbnail.sprite = sprite;
    }

    public void SetFrameAlpha(float alpha)
    {
        frameColor.a = Mathf.Clamp(alpha, 0, 1);
        frame.color = frameColor;
    }

    public void SetFrameEnabled(bool isEnabled)
    {
        frame.enabled = isEnabled;
    }

    public void SetHealthBarAlpha(float alpha)
    {
        healthBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        healthBarDrain.color = new Color(1, 0, 0, alpha);
        healthBarColor.a = alpha;
        healthBar.color = healthBarColor;   
        healthText.color = new Color(1, 1, 1, alpha);
    }

    public void SetActionBarAlpha(float alpha)
    {
        actionBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        actionBarColor.a = alpha;
        actionBar.color = actionBarColor;
        actionText.color = new Color(1, 1, 1, alpha);
    }

    public void SetSkillRadialAlpha(float alpha)
    {
        skillRadialBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        skillRadial.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.5f));
        skillRadialText.color = new Color(1, 1, 1, alpha);
    }

    //public void SetBloomColor(color color)
    //{
    //    bloomColor = color;
    //    this.bloom.color = color;

    //    var intensity = (bloomColor.r + bloomColor.g + bloomColor.b) / 3f;
    //    var factor = 1f / intensity;
    //    var emissionColor = new color(bloomColor.r * factor, bloomColor.g * factor, bloomColor.b * factor, bloomColor.a);
    //    this.bloom.material.SetColor("_EmissionColor", emissionColor);
    //}

    //public void SetBloomAlpha(float alpha)
    //{
    //    bloomColor.a = alpha;
    //    this.bloom.color = bloomColor;

    //    var intensity = (bloomColor.r + bloomColor.g + bloomColor.b) / 3f;
    //    var factor = 1f / intensity;
    //    var emissionColor = new color(bloomColor.r * factor, bloomColor.g * factor, bloomColor.b * factor, alpha);
    //    this.bloom.material.SetColor("_EmissionColor", emissionColor);
    //}

    public void SetFrameColor(Color color)
    {
        frameColor = color;
        this.frame.color = frameColor;
    }


    public void SetSelectionBoxEnabled(bool isEnabled = true)
    {
        selectionBox.enabled = isEnabled;
    }

    public void SetTurnDelayFontSize(int key)
    {
        var fontSizeKeyValueMap = new Dictionary<int, float>() {
            { 9, 1.0000f },
            { 8, 1.3750f },
            { 7, 1.7500f },
            { 6, 2.1250f },
            { 5, 2.5000f },
            { 4, 2.8750f },
            { 3, 3.2500f },
            { 2, 3.6250f },
            { 1, 4.0000f },
        };

        turnDelayText.fontSize = key > 9 ? 1f : fontSizeKeyValueMap[key];
    }

    public void SetTurnDelayText(string text)
    {
        turnDelayText.text = text;
    }

    public void SetTurnDelayTextEnabled(bool isEnabled)
    {
        turnDelayText.enabled = isEnabled;
    }

    public void SetTurnDelayTextAlpha(float alpha)
    {
        turnDelayColor.a = Mathf.Clamp(alpha, 0, 1);
        turnDelayText.color = turnDelayColor;
    }

    public void SetTurnDelayTextColor(Color color)
    {
        turnDelayColor = color;
        turnDelayText.color = turnDelayColor;
    }

    public void SetNameTagText(string text)
    {
        nameTagText.text = text;
    }

    public void SetNameTagTextAlpha(float alpha)
    {
        nameTagText.color = new Color(1, 1, 1, alpha);
    }

    public void SetNameTagEnabled(bool isEnabled)
    {
        nameTagText.enabled = isEnabled;
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


    public void SetSelectionAlpha(float alpha)
    {
        selectionColor = new Color(1, 1, 1, alpha);
        selectionBox.color = new Color(1, 1, 1, alpha);
    }


    public void SetWeaponIconAlpha(float alpha)
    {
        weaponIconColor = new Color(1, 1, 1, alpha);
        weaponIcon.color = weaponIconColor;
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

