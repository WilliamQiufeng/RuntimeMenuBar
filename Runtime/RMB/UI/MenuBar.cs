using System;
using System.Collections.Generic;
using RMB.SO;
using RMB.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RMB.UI
{
    public class MenuBar : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        public GameObject rootMenuItemContainer;
        public MenuBarSO menuBarSO;
        public GameObject rootMenuItemPrefab;
        private readonly Stack<MenuItem> _path = new();
        private readonly List<MenuItem> _rootMenuItems = new();
        internal readonly Dictionary<Event, MenuItem> Shortcuts = new();

        private int _focusCount;
        internal bool IsMenuOpen;

        public int FocusCount
        {
            get => _focusCount;
            set => _focusCount = Math.Max(value, 0);
        }

        private void Start()
        {
            UpdateInfo();
        }

        private void Update()
        {
            // Hide the menu if the user clicks outside the menu
            if (IsMenuOpen && Input.GetMouseButtonDown(0) && FocusCount == 0) HideAll();
        }

        private void OnGUI()
        {
            // Process any shortcut events
            foreach (var (shortcutEvent, menuItem) in Shortcuts)
                if (Event.current.Equals(shortcutEvent))
                    menuItem.OnClick();
        }

        // Handle focused
        public void OnPointerEnter(PointerEventData eventData)
        {
            FocusCount++;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            FocusCount--;
        }

        public void GotoItem(MenuItem item)
        {
            // Current item is already expanded: skip
            if (_path.Count > 0 && _path.Peek() == item) return;
            // Current item not contained in the top of the path: pop and contract
            while (_path.Count > 0 && !_path.Peek().SubItems.Contains(item))
            {
                var popped = _path.Pop();
                popped.ContainerEnabled = false;
            }

            // Push the item to the path. This will not expand its container
            _path.Push(item);
        }

        public void GotoItemRecursive(MenuItem item)
        {
            HideAll(); // Hide (essentially clearing the path)
            GotoItemRecursiveInternal(item);
        }

        internal void GotoItemRecursiveInternal(MenuItem item)
        {
            // Start from root
            if (item.ParentMenuItem != null) GotoItemRecursiveInternal(item.ParentMenuItem);
            // Expands upwards
            GotoItem(item);
            // This expands the container
            item.ContainerEnabled = true;
        }

        public void HideAll()
        {
            // Contract and pop all items in path
            while (_path.Count > 0)
            {
                var popped = _path.Pop();
                popped.ContainerEnabled = false;
            }

            IsMenuOpen = false; // Set state
        }

        public void UpdateInfo()
        {
            // Clear
            _rootMenuItems.Clear();
            Shortcuts.Clear();
            if (menuBarSO == null)
            {
                _rootMenuItems.AddRange(rootMenuItemContainer.transform.GetChildrenComponent<MenuItem>());
            }
            else
            {
                rootMenuItemContainer.transform.ClearChildren();
                foreach (var rootMenuItem in menuBarSO.rootMenuItems)
                {
                    var instance = Instantiate(rootMenuItemPrefab, transform);
                    var menuItem = instance.GetComponent<MenuItem>();
                    menuItem.menuItemSO = rootMenuItem;
                    _rootMenuItems.Add(menuItem);
                }
            }

            UpdateItemsInfo();
            RegisterShortcutEvents();
        }

        internal void RegisterShortcutEvents()
        {
            foreach (var subItem in _rootMenuItems) subItem.RegisterShortcutEvents();
        }

        private void UpdateItemsInfo()
        {
            foreach (var rootMenuItem in _rootMenuItems)
            {
                rootMenuItem.UpdateInfo();
                rootMenuItem.MenuBar = this;
                Debug.Log($"Update Parent: {rootMenuItem.itemName.text}");
                rootMenuItem.UpdateSubItemsParent();
            }
        }
    }
}