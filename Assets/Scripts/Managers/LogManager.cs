using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Behaviors
{
    public class LogManager : ExtendedMonoBehavior
    {
        private TextMeshProUGUI log;
        private List<string> messages = new List<string>();

        const int MaxMessages = 10;

        private void Awake()
        {
            log = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {

        }

        float delay = 0f;

        void Update()
        {
            //const float duration = 3f;
            //delay += Time.deltaTime / duration;
            //if (delay >= 1f)
            //{
            //    delay = 0f;
            //    info($"Ticks: {DateTime.Now.Ticks}");
            //}
        }

        private void FixedUpdate()
        {

            log.text = string.Join(Environment.NewLine, messages.OrderByDescending(x => x.ToString()));
        }


        public void info(string message)
        {
            messages.Add($@"<color=""white"">{message}</color>");
            truncate();
        }

        public void success(string message)
        {
            messages.Add($@"<color=""green"">{message}</color>");
            truncate();
        }

        public void error(string message)
        {
            messages.Add($@"<color=""red"">{message}</color>");
            truncate();
        }


        private void truncate()
        {
            if (messages.Count > MaxMessages)
                messages.RemoveAt(0);
        }
    }
}