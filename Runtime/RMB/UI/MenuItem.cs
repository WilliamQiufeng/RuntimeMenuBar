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
        public string inputBinding; // https://docs.unity3d.com/ScriptReference/Event.KeyboardEvent.html
        public bool closeOnClick;
        public MenuItemSO menuItemSO;
        public TMP_Text itemName;
        public TMP_Text shortcutText;
        public GameObject iconContainer;
        public GameObject container;
        public GameObject rootMenuItemPrefab;
        public GameObject menuSubItemPrefab;
        public UnityEvent<MenuItem> additionalOnClick;
        [NonSerialized] internal readonly List<MenuItem> SubItems = new();
        private Event _shortcutEvent;
        internal MenuBar MenuBar;
        internal MenuItem ParentMenuItem;

        public bool ContainerEnabled
        {
            get => container.activeSelf;
            set => container.SetActive(value);
        }

        private bool IsRootMenuItem => ParentMenuItem == null;


        public void OnPointerEnter(PointerEventData eventData)
        {
            // Skip when menu is not open
            if (!MenuBar.IsMenuOpen) return;
            // If any of its children is ray-casted, skip and leave this event to the ultimate child to process.
            // since that's the target the user is hitting
            if (eventData.hovered.Any(h =>
                    h.TryGetComponent(out MenuItem menuItem) && SubItems.Contains(menuItem))) return;
            // Go to the specific item. It shouldn't need to be recursive since it should be one of the open path
            MenuBar.GotoItem(this);
            // Enable the container.
            ContainerEnabled = true;
        }


        public void OnClick(bool fromGUI = false)
        {
            if (fromGUI)
            {
                ContainerEnabled = !ContainerEnabled; // Toggle container
                if (ContainerEnabled)
                {
                    MenuBar.IsMenuOpen = true; // Open Menu if now enabled
                    MenuBar.GotoItem(this);
                }
                else if (IsRootMenuItem)
                {
                    MenuBar.IsMenuOpen = false; // Close Menu
                    MenuBar.HideAll();
                }
            }

            // Invoke OnClick events
            additionalOnClick?.Invoke(this);
            if (menuItemSO != null) menuItemSO.onClickEvent?.Invoke(this);
            // Hide the menu if the menu item specifies a close when clicked
            if (closeOnClick) MenuBar.HideAll();
            // From this point the logic is dependent on unchanged value of ContainerEnabled, so skip if the call
            // is from GUI since it toggles the state of ContainerEnabled
            if (fromGUI) return;
            // This comes from a shortcut event. Triggering this item when it's open already will close it
            if (ContainerEnabled) MenuBar.HideAll();
            // If it specifies that it will not close when clicked on, we should expand the panel to show this menu item.
            // If it closes when clicked on, it doesn't bring up the menu (e.g. Ctrl + S for save)
            else if (!closeOnClick && !ContainerEnabled) MenuBar.GotoItemRecursive(this);
        }

        public void UpdateInfo()
        {
            // Close the container
            ContainerEnabled = false;
            // Clear all sub-items
            SubItems.Clear();
            if (menuItemSO == null)
            {
                // Add all existing sub-items in its children
                SubItems.AddRange(container.transform.GetChildrenComponent<MenuItem>());
                // Initialize shortcut event from inputBinding
                if (!string.IsNullOrEmpty(inputBinding)) _shortcutEvent = Event.KeyboardEvent(inputBinding);
            }
            else
            {
                // Set name from SO
                itemName.text = menuItemSO.menuName;
                // Initialize shortcut event from SO
                if (shortcutText != null) _shortcutEvent = Event.KeyboardEvent(menuItemSO.shortcutEvent);

                // Clear all container's children
                container.transform.ClearChildren();
                foreach (var itemSO in menuItemSO.elements)
                {
                    // Instantiate its sub-items
                    var instance = Instantiate(itemSO.menuItemType is MenuItemType.Child
                        ? menuSubItemPrefab
                        : rootMenuItemPrefab, container.transform);
                    var menuItem = instance.GetComponent<MenuItem>();
                    menuItem.menuItemSO = itemSO;
                    SubItems.Add(menuItem); // Add to list
                }
            }

            // Enable or disable the expansion icon depending on whether it has sub-items or not
            if (iconContainer != null) iconContainer.SetActive(SubItems.Count != 0);

            // Set shortcut text
            if (shortcutText != null)
                shortcutText.text = _shortcutEvent == null ? "" : _shortcutEvent.ToKeybindString();
            foreach (var subItem in SubItems)
            {
                // Recurse on its children
                subItem.UpdateInfo();
            }
        }

        internal void UpdateSubItemsParent()
        {
            foreach (var subItem in SubItems)
            {
                // Update sub-items' parent
                subItem.ParentMenuItem = this;
                subItem.MenuBar = MenuBar;
                subItem.UpdateSubItemsParent();
            }
        }

        internal void RegisterShortcutEvents()
        {
            // Register shortcut events to the dictionary in menubar.
            if (_shortcutEvent != null) MenuBar.Shortcuts.Add(_shortcutEvent, this);
            foreach (var subItem in SubItems) subItem.RegisterShortcutEvents();
        }
    }
}