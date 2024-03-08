using TMPro;
using UnityEngine.UI;

public class CardManager : ExtendedMonoBehavior
{

    Image profile;
    TextMeshProUGUI title;
    TextMeshProUGUI details;

    private void Awake()
    {
        profile = GetComponentInChildren<Image>();
        title = GetComponentsInChildren<TextMeshProUGUI>()[0];
        details = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    void Start()
    {

    }


    void Update()
    {

    }


    public void Set(ActorBehavior actor)
    {
        profile.sprite = resourceManager.ActorPortrait(actor.id);
        profile.enabled = true;
        title.text = actor.name;
        details.text = resourceManager.ActorDetails(actor.id);
    }

    public void Clear()
    {
        profile.enabled = false;
        title.text = "";
        details.text = "";
    }
}
