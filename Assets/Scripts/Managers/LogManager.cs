using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Behaviors
{
    public class LogManager : ExtendedMonoBehavior
    {
        private TextMeshProUGUI textMesh;
        private List<string> messages = new List<string>();

        const int MaxMessages = 10;

        #region Components

        public string text
        {
            get => textMesh.text;
            set => textMesh.text = value;
        }


        public Color color
        {
            get => textMesh.color;
            set => textMesh.color = value;
        }

        #endregion



        private void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        public void info(string message)
        {
            messages.Add($@"<color=""white"">{message}</color>");
  
            print();
        }

        public void success(string message)
        {
            messages.Add($@"<color=""green"">{message}</color>");
            print();
        }

        public void warning(string message)
        {
            messages.Add($@"<color=""orange"">{message}</color>");
            print();
        }


        public void error(string message)
        {
            messages.Add($@"<color=""red"">{message}</color>");
            print();
        }


        private void print()
        {

            //Truncate messages (if neccessary)
            if (messages.Count > MaxMessages)
                messages.RemoveAt(0);

            //Print in descending order
            textMesh.text = string.Join(Environment.NewLine, messages.OrderByDescending(x => x.ToString()));
        }


    }
}