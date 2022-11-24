using System;
using System.Collections.Generic;
using System.Linq;
using RMB.SO;
using RMB.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RMB.UI
{
    public class MenuItem : MonoBehaviour, IPointerEnterHandler
    {
        public MenuItemSO menuItemSO;
        public TMP_Text itemName;
        public TMP_Text shortcutText;
        public GameObject iconContainer;
        public GameObject container;
        public GameObject rootMenuItemPrefab;
        public GameObject menuSubItemPrefab;
        public UnityEvent additionalOnClick;
        public bool closeOnClick;
        public string inputBinding; // https://docs.unity3d.com/ScriptReference/Event.KeyboardEvent.html
        [NonSerialized] internal readonly List<MenuItem> SubItems = new();
        internal MenuBar MenuBar;
        internal MenuItem ParentMenuItem;
        internal Event ShortcutEvent;

        public bool ContainerEnabled
        {
            get => container.activeSelf;
            set => container.SetActive(value);
        }

        public bool IsRootMenuItem => ParentMenuItem == null;


        private void OnGUI()
        {
            if (!string.IsNullOrEmpty(inputBinding))
                if (Event.current.Equals(ShortcutEvent))
                    OnClick();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!MenuBar.IsMenuOpen) return;
            if (eventData.hovered.Any(h =>
                    h.TryGetComponent(out MenuItem menuItem) && SubItems.Contains(menuItem))) return;
            MenuBar.GotoItem(this);
            ContainerEnabled = true;
            // Debug.Log(string.Join(", ", MenuBar.Path.Select(s => s.itemName.text)));
        }


        public void OnClick(bool fromGUI = false)
        {
            if (fromGUI && (container.transform.childCount > 0 ||
                            (menuItemSO != null && menuItemSO.elements.Count > 0)))
            {
                ContainerEnabled = !ContainerEnabled;
                if (ContainerEnabled)
                {
                    MenuBar.IsMenuOpen = true;
                    MenuBar.Path.Push(this);
                }
                else if (IsRootMenuItem)
                {
                    MenuBar.IsMenuOpen = false;
                    MenuBar.HideAll();
                }
            }

            Debug.Log($"Click: {itemName.text}");
            additionalOnClick?.Invoke();
            if (menuItemSO != null) menuItemSO.onClickEvent?.Invoke();
            if (closeOnClick) MenuBar.HideAll();
        }

        public void UpdateInfo()
        {
            ContainerEnabled = false;
            SubItems.Clear();
            if (menuItemSO == null)
            {
                SubItems.AddRange(container.transform.GetChildrenComponent<MenuItem>());
                if (iconContainer != null) iconContainer.SetActive(SubItems.Count != 0);
                if (!string.IsNullOrEmpty(inputBinding)) ShortcutEvent = Event.KeyboardEvent(inputBinding);
            }
            else
            {
                itemName.text = menuItemSO.menuName;
                if (shortcutText != null) ShortcutEvent = Event.KeyboardEvent(menuItemSO.shortcutEvent);

                container.transform.ClearChildren();

                if (iconContainer != null) iconContainer.SetActive(menuItemSO.elements.Count != 0);
                foreach (var itemSO in menuItemSO.elements)
                {
                    var instance = Instantiate(itemSO.menuItemType is MenuItemType.Child
                        ? menuSubItemPrefab
                        : rootMenuItemPrefab, container.transform);
                    var menuItem = instance.GetComponent<MenuItem>();
                    menuItem.menuItemSO = itemSO;
                    SubItems.Add(menuItem);
                }
            }

            if (shortcutText != null) shortcutText.text = ShortcutEvent == null ? "" : ShortcutEvent.ToKeybindString();
            foreach (var subItem in SubItems)
            {
                Debug.Log($"Update Info: {subItem.itemName.text}");
                subItem.UpdateInfo();
            }
        }

        internal void UpdateSubItemsParent()
        {
            foreach (var subItem in SubItems)
            {
                Debug.Log($"Update: {subItem.itemName.text}");
                subItem.ParentMenuItem = this;
                subItem.MenuBar = MenuBar;
                subItem.UpdateSubItemsParent();
            }
        }
    }
}