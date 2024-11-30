using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Behaviors
{
    public class LogManager : ExtendedMonoBehavior
    {
        private string log;
        private List<string> messages = new List<string>();

        const int MaxMessages = 10;

        #region Components

        public string text
        {
            get => log;
            set => log = value;
        }

        #endregion

        private void Awake()
        {
     
        }

        public void info(string message)
        {
            Debug.Log(message);
            messages.Add($@"<color=""white"">{message}</color>");
            print();
        }

        public void success(string message)
        {
            Debug.Log(message);
            messages.Add($@"<color=""green"">{message}</color>");
            print();
        }

        public void warning(string message)
        {
            Debug.LogWarning(message);
            messages.Add($@"<color=""orange"">{message}</color>");
            print();
        }


        public void error(string message)
        {
            Debug.LogError(message);
            messages.Add($@"<color=""red"">{message}</color>");
            print();
        }


        private void print()
        {
            //Truncate messages (if neccessary)
            if (messages.Count > MaxMessages)
                messages.RemoveAt(0);

            //Print in descending order
            log = string.Join(Environment.NewLine, messages.OrderByDescending(x => x.ToString()));
        }

    }
}