using Game.Behaviors.Actor;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviors
{
    public class CardManager : ExtendedMonoBehavior
    {
        RectTransform rectTransform;
        Image backdrop;
        Image portrait;
        TextMeshProUGUI title;
        TextMeshProUGUI details;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            backdrop = GameObject.Find(Constants.CardBackdrop).GetComponent<Image>();
            portrait = GameObject.Find(Constants.CardPortrait).GetComponent<Image>();     
            title = GameObject.Find(Constants.CardTitle).GetComponent<TextMeshProUGUI>();
            details = GameObject.Find(Constants.CardDetails).GetComponent<TextMeshProUGUI>();
            Clear();
        }

        private void Start()
        {
            portrait.rectTransform.sizeDelta = new Vector2(cardPortraitSize, cardPortraitSize);
        }

        public void Set(ActorBehavior actor)
        {
            backdrop.enabled = true;
            portrait.sprite = resourceManager.ActorPortrait(actor.archetype.ToString());
            portrait.enabled = true;
            title.text = actor.name;


            var stats
                = $"hp: {actor.hp}/{actor.maxHp}{Environment.NewLine}"
                + $""
                + $"{Environment.NewLine}"
                + resourceManager.ActorDetails(actor.archetype.ToString());



            details.text = resourceManager.ActorDetails(actor.archetype.ToString());
        }

        public void Clear()
        {
            backdrop.enabled = false;
            portrait.enabled = false;
            title.text = "";
            details.text = "";

            //DespawnAll selection from Actor
            actors.ForEach(x => x.renderers.SetSelectionActive(false));
        }
    }
}