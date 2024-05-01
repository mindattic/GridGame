using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : ExtendedMonoBehavior
{
    RectTransform rectTransform;
    Image back;
    Image profile;
    TextMeshProUGUI title;
    TextMeshProUGUI details;

    private void Awake()
    {
        Init();
    }
    void Start() { }
    void Update() { }
    void FixedUpdate() { }


    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        back = GameObject.Find("Card/Back").GetComponent<Image>();
        profile = GameObject.Find("Card/Profile").GetComponent<Image>();
        title = GameObject.Find("Card/Title").GetComponent<TextMeshProUGUI>();
        details = GameObject.Find("Card/Details").GetComponent<TextMeshProUGUI>();

        //TODO: Calculate dimensions based on device properties


        Clear();

        
    }

    public void Set(ActorBehavior actor)
    {
        back.enabled = true;
        profile.sprite = resourceManager.ActorPortrait(actor.archetype.ToString());
        profile.enabled = true;
        title.text = actor.name;


        var stats 
            = $"HP: {actor.HP}/{actor.MaxHP}{Environment.NewLine}" 
            + $""
            + $"{Environment.NewLine}" 
            + resourceManager.ActorDetails(actor.archetype.ToString());



        details.text = resourceManager.ActorDetails(actor.archetype.ToString()) ;
    }

    public void Clear()
    {
        back.enabled = false;
        profile.enabled = false;
        title.text = "";
        details.text = "";

        //Clear selection from actor
        actors.ForEach(x => x.render.SetFocus(false));
    }
}
