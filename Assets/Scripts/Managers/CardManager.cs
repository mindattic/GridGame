using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : ExtendedMonoBehavior
{
    RectTransform RectTransform;
    Image BackImage;
    Image ProfileImage;
    TextMeshProUGUI Title;
    TextMeshProUGUI Details;

    private void Awake()
    {
        Init();
    }
    void Start() { }
    void Update() { }
    void FixedUpdate() { }


    public void Init()
    {
        RectTransform = GetComponent<RectTransform>();
        BackImage = GameObject.Find("Card/Back").GetComponent<Image>();
        ProfileImage = GameObject.Find("Card/Profile").GetComponent<Image>();
        Title = GameObject.Find("Card/Title").GetComponent<TextMeshProUGUI>();
        Details = GameObject.Find("Card/Details").GetComponent<TextMeshProUGUI>();

        //TODO: Calculate dimensions based on device properties


        Clear();

        
    }

    public void Set(ActorBehavior actor)
    {
        BackImage.enabled = true;
        ProfileImage.sprite = ResourceManager.ActorPortrait(actor.Archetype.ToString());
        ProfileImage.enabled = true;
        Title.text = actor.name;


        var stats 
            = $"HP: {actor.HP}/{actor.MaxHP}{Environment.NewLine}" 
            + $""
            + $"{Environment.NewLine}" 
            + ResourceManager.ActorDetails(actor.Archetype.ToString());



        Details.text = ResourceManager.ActorDetails(actor.Archetype.ToString()) ;
    }

    public void Clear()
    {
        BackImage.enabled = false;
        ProfileImage.enabled = false;
        Title.text = "";
        Details.text = "";

        //Clear selection from Actor
        Actors.ForEach(x => x.Renderers.SetFocus(false));
    }
}
