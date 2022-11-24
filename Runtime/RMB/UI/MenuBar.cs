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
        public readonly Stack<MenuItem> Path = new();
        internal readonly List<MenuItem> RootMenuItems = new();
        internal bool IsFocused;
        internal bool IsMenuOpen;

        private void Start()
        {
            UpdateInfo();
        }

        private void Update()
        {
            if (IsMenuOpen && Input.GetMouseButtonDown(0) && !IsFocused) HideAll();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsFocused = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsFocused = false;
        }

        public void GotoItem(MenuItem item)
        {
            if (Path.Count > 0 && Path.Peek() == item) return;
            while (Path.Count > 0 && !Path.Peek().SubItems.Contains(item))
            {
                var popped = Path.Pop();
                popped.ContainerEnabled = false;
            }

            Path.Push(item);
        }

        public void HideAll()
        {
            while (Path.Count > 0)
            {
                var popped = Path.Pop();
                popped.ContainerEnabled = false;
                Debug.Log($"Hide {popped.itemName.text}");
            }

            IsMenuOpen = false;
        }

        public void UpdateInfo()
        {
            RootMenuItems.Clear();
            if (menuBarSO == null)
            {
                RootMenuItems.AddRange(rootMenuItemContainer.transform.GetChildrenComponent<MenuItem>());
            }
            else
            {
                rootMenuItemContainer.transform.ClearChildren();
                foreach (var rootMenuItem in menuBarSO.rootMenuItems)
                {
                    var instance = Instantiate(rootMenuItemPrefab, transform);
                    var menuItem = instance.GetComponent<MenuItem>();
                    menuItem.menuItemSO = rootMenuItem;
                    RootMenuItems.Add(menuItem);
                }
            }

            UpdateItemsInfo();
        }

        private void UpdateItemsInfo()
        {
            foreach (var rootMenuItem in RootMenuItems)
            {
                rootMenuItem.UpdateInfo();
                rootMenuItem.MenuBar = this;
                Debug.Log($"Update Parent: {rootMenuItem.itemName.text}");
                rootMenuItem.UpdateSubItemsParent();
            }
        }
    }
}