using System;
using System.Collections.Generic;
using RMB.SO;
using RMB.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace RMB.UI
{
    public class MenuItem : MonoBehaviour
    {
        public MenuItemSO menuItemSO;
        public TMP_Text itemName;
        public TMP_Text shortcutText;
        public GameObject iconContainer;
        public GameObject container;
        public GameObject rootMenuItemPrefab;
        public GameObject menuSubItemPrefab;
        [NonSerialized]
        private readonly List<GameObject> _subItems = new();


        private void Start()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            if (menuItemSO == null) return;
            itemName.text = menuItemSO.menuName;
            if (shortcutText != null)
            {
                shortcutText.text = menuItemSO.keyBind.ToString();
            }

            iconContainer.transform.GetComponent<Renderer>().enabled = menuItemSO.elements.Count == 0;
            container.transform.ClearChildren();
            _subItems.Clear();
            foreach (var itemSO in menuItemSO.elements)
            {
                var instance = Instantiate(itemSO.menuItemType is MenuItemType.Child
                    ? menuSubItemPrefab
                    : rootMenuItemPrefab, container.transform);
                _subItems.Add(instance);
            }
        }
    }
}