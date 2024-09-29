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
        Vector3 initialPosition;
        [SerializeField] AnimationCurve verticalCurve;
        [SerializeField] AnimationCurve horizontalCurve;
        [SerializeField] Vector2 range;
        [SerializeField] Vector2 speed;
        private float elapsedTime = 0f;

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
            initialPosition = portrait.rectTransform.localPosition;
        }

        public void Set(ActorBehavior actor)
        {
            backdrop.enabled = true;
            portrait.sprite = resourceManager.ActorPortrait(actor.archetype.ToString());
            portrait.enabled = true;
           
            title.text = actor.name;

            var stats
                = $" HP: {actor.hp}/{actor.maxHp}{Environment.NewLine}"
                + $"ATK: { actor.attack}    DEF: {actor.endurance}{Environment.NewLine}"
                + $"ACC: {actor.focus}     DEX: {actor.dexterity}{Environment.NewLine}"
                + $"SPD: {actor.speed}    LCK: {actor.luck}{Environment.NewLine}"
                + $""
                + $"{Environment.NewLine}"
                + resourceManager.ActorDetails(actor.archetype.ToString());
            details.text = stats;
        }

        public void FixedUpdate()
        {
            elapsedTime += Time.deltaTime;

            //var hover = new Vector3(
            //    initialPosition.x + horizontalCurve.Evaluate(elapsedTime / speed.x) * range.x * gameSpeed,
            //    initialPosition.y + verticalCurve.Evaluate(elapsedTime / speed.y) * range.y * gameSpeed,
            //    initialPosition.z);

            //var rot = Geometry.Rotation(new Vector3(0, 5 * verticalCurve.Evaluate(Time.time % verticalCurve.length), 0));
            //portrait.rectTransform.localPosition = hover;
            //portrait.rectTransform.localRotation = rot;

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