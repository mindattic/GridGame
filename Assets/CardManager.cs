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
        rectTransform = GetComponent<RectTransform>();
        back = GameObject.Find("Card/Back").GetComponent<Image>();
        profile = GameObject.Find("Card/Profile").GetComponent<Image>();
        title = GameObject.Find("Card/Title").GetComponent<TextMeshProUGUI>();
        details = GameObject.Find("Card/Details").GetComponent<TextMeshProUGUI>();

        Clear();
    }

    void Start()
    {

    }


    void Update()
    {

    }


    public void Set(ActorBehavior actor)
    {
        back.enabled = true;
        profile.sprite = resourceManager.ActorPortrait(actor.id);
        profile.enabled = true;
        title.text = actor.name;
        details.text = resourceManager.ActorDetails(actor.id);
    }

    public void Clear()
    {
        back.enabled = false;
        profile.enabled = false;
        title.text = "";
        details.text = "";
    }
}
