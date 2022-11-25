using System;
using RMB.Util;
using UnityEngine;
using UnityEngine.UI;

namespace RMB.UI
{
    public class CheckBoxItem : MonoBehaviour
    {
        public Image checkBoxImage;
        public bool state;
        public MenuItem menuItem;

        private void Start()
        {
            checkBoxImage.gameObject.SetActive(state);
            menuItem.additionalOnClick.AddListener(OnClick);
        }

        private void OnClick(MenuItem item)
        {
            state = !state;
            checkBoxImage.gameObject.SetActive(state);
        }
    }
}