using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class DebugDropdown : MonoBehaviour
    {

        [SerializeField] private TMP_Dropdown Dropdown;

        public void Run()
        {
            int index = Dropdown.value;
            if (index < 1)
                return;

            //switch (index)
            //{
            //    case 1: PortraitTest(); break;

            //}
        }


    }
}
