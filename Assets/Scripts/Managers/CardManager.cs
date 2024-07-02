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
        Image backImage;
        Image profileImage;
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
            backImage = GameObject.Find(Constants.CardBack).GetComponent<Image>();
            profileImage = GameObject.Find(Constants.CardProfile).GetComponent<Image>();
            title = GameObject.Find(Constants.CardTitle).GetComponent<TextMeshProUGUI>();
            details = GameObject.Find(Constants.CardDetails).GetComponent<TextMeshProUGUI>();

            //TODO: Calculate dimensions based on device properties


            Clear();


        }

        public void Set(ActorBehavior actor)
        {
            backImage.enabled = true;
            profileImage.sprite = resourceManager.ActorPortrait(actor.archetype.ToString());
            profileImage.enabled = true;
            title.text = actor.name;


            var stats
                = $"hp: {actor.hp}/{actor.maxHP}{Environment.NewLine}"
                + $""
                + $"{Environment.NewLine}"
                + resourceManager.ActorDetails(actor.archetype.ToString());



            details.text = resourceManager.ActorDetails(actor.archetype.ToString());
        }

        public void Clear()
        {
            backImage.enabled = false;
            profileImage.enabled = false;
            title.text = "";
            details.text = "";

            //Clear selection from Actor
            actors.ForEach(x => x.Renderers.SetFocus(false));
        }
    }
}