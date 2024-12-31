using Game.Behaviors.Actor;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviors
{
    public class CardManager : MonoBehaviour
    {
        protected float cardPortraitSize => GameManager.instance.cardPortraitSize;
        protected ResourceManager resourceManager => GameManager.instance.resourceManager;
        protected List<ActorInstance> actors => GameManager.instance.actors;



        RectTransform rectTransform;
        Image backdrop;
        Image portrait;
        TextMeshProUGUI title;
        TextMeshProUGUI details;
        Vector3 destination;
        Vector3 offscreenPosition;
        AnimationCurve slideInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float slideDuration = 0.5f;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            backdrop = GameObject.Find(Constants.CardBackdrop).GetComponent<Image>();
            portrait = GameObject.Find(Constants.CardPortrait).GetComponent<Image>();
            title = GameObject.Find(Constants.CardTitle).GetComponent<TextMeshProUGUI>();
            details = GameObject.Find(Constants.CardDetails).GetComponent<TextMeshProUGUI>();
            Reset();
        }

        private void Start()
        {
            portrait.rectTransform.sizeDelta = new Vector2(cardPortraitSize, cardPortraitSize);

            var position = portrait.rectTransform.localPosition;
            float width = portrait.rectTransform.rect.width;
            float height = portrait.rectTransform.rect.height;
            destination = new Vector3(position.x + width / 2, position.y + height / 4, position.z);

            offscreenPosition = new Vector3(Screen.width + width, destination.y, destination.z);
            portrait.rectTransform.localPosition = offscreenPosition;
        }

        public void Assign(ActorInstance actor)
        {
            backdrop.enabled = true;
            portrait.sprite = resourceManager.ActorSprite(actor.character.ToString()).portrait;
            portrait.enabled = true;

            title.text = actor.name;

            var hp = actor.stats.HP;
            var mhp = actor.stats.MaxHP;
            var str = actor.stats.Strength;
            var vit = actor.stats.Vitality;
            var agi = actor.stats.Agility;
            var spd = actor.stats.Speed;
            var lck = actor.stats.Luck;

            var stats
                = $" HP: {hp}/{mhp}    {Environment.NewLine}"
                + $"STR: {str}         {Environment.NewLine}"
                + $"VIT: {vit}         {Environment.NewLine}"
                + $"AGI: {agi}         {Environment.NewLine}"
                + $"SPD: {spd}         {Environment.NewLine}"
                + $"LCK: {lck}         {Environment.NewLine}"
                + $"{Environment.NewLine}"
                + resourceManager.ActorDetails(actor.character.ToString());
            details.text = stats;

            TriggerSlideIn();
        }

        private void TriggerSlideIn()
        {
            StartCoroutine(SlideIn());
        }

        private IEnumerator SlideIn()
        {
            float elapsedTime = 0f;

            while (elapsedTime < slideDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / slideDuration);
                float curveValue = slideInCurve.Evaluate(progress);

                // Interpolate between offscreen and initial position
                portrait.rectTransform.localPosition = Vector3.Lerp(offscreenPosition, destination, curveValue);

                yield return Wait.OneTick();
            }

            // Ensure the portrait reaches the final position
            portrait.rectTransform.localPosition = destination;
        }

        public void Reset()
        {
            backdrop.enabled = false;
            portrait.enabled = false;
            title.text = "";
            details.text = "";

            // Despawn all selection boxes from actors
            actors.ForEach(x => x.render.SetSelectionBoxEnabled(false));

            // Reset portrait position
            portrait.rectTransform.localPosition = offscreenPosition;
        }
    }
}
