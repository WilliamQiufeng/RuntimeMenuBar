using System;
using RMB.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RMB.UI
{
    public class CheckBoxItem : MonoBehaviour
    {
        public Image checkBoxImage;
        public bool state;
        public MenuItem menuItem;
        public UnityEvent<CheckBoxItem, bool> onClickEvent;

        private void Start()
        {
            checkBoxImage.gameObject.SetActive(state);
            menuItem.additionalOnClick.AddListener(OnClick);
        }

        private void OnClick(MenuItem item)
        {
            SetStateWithoutNotify(!state);
            onClickEvent.Invoke(this, state);
        }

        public void SetStateWithoutNotify(bool newState)
        {
            state = newState;
            checkBoxImage.gameObject.SetActive(state);
        }
    }
}