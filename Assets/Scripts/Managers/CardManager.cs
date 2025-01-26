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
        #region Properties
        protected float cardPortraitSize => GameManager.instance.cardPortraitSize;
        protected DataManager dataManager => GameManager.instance.dataManager;
        protected ResourceManager resourceManager => GameManager.instance.resourceManager;
        protected List<ActorInstance> actors => GameManager.instance.actors;
        #endregion

        //Variables
        RectTransform rectTransform;
        Image backdrop;
        Image portrait;
        TextMeshProUGUI title;
        TextMeshProUGUI details;
        Vector3 destination;
        Vector3 offscreenPosition;
        AnimationCurve slideInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float slideDuration = 0.5f;

        //Method which is used for initialization tasks that need to occur before the game starts 
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            backdrop = GameObject.Find(Constants.CardBackdrop).GetComponent<Image>();
            portrait = GameObject.Find(Constants.CardPortrait).GetComponent<Image>();
            title = GameObject.Find(Constants.CardTitle).GetComponent<TextMeshProUGUI>();
            details = GameObject.Find(Constants.CardDetails).GetComponent<TextMeshProUGUI>();
            Reset();
        }

        //Method which is automatically called before the first frame update  
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
            portrait.sprite = resourceManager.Portrait(actor.character.ToString()).Value.ToSprite();
            portrait.enabled = true;

            title.text = actor.name;

            var hp = $"{actor.stats.HP,2}/{actor.stats.MaxHP,-3}"; // HP/MaxHP with dynamic padding
            var str = $"{actor.stats.Strength,4}";                // Right-align Stats to 4 characters
            var vit = $"{actor.stats.Vitality,4}";
            var agi = $"{actor.stats.Agility,4}";
            var spd = $"{actor.stats.Speed,4}";
            var lck = $"{actor.stats.Luck,4}";

            // Create the Stats table
            var stats
                = $"HP       STR  VIT  AGI  SPD  LCK{Environment.NewLine}"
                + $"{hp}   {str}{vit}{agi}{spd}{lck}{Environment.NewLine}";

            details.text = stats + dataManager.GetDetails(actor.character.ToString()).Card;

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

            // TriggerDespawn all selection boxes from actors
            actors.ForEach(x => x.render.SetSelectionBoxEnabled(false));

            // Initialize portrait position
            portrait.rectTransform.localPosition = offscreenPosition;
        }
    }
}
