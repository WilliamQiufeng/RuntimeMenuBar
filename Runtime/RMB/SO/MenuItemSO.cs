using System.Collections.Generic;
using RMB.Util;
using UnityEngine;

namespace RMB.SO
{
    [CreateAssetMenu(menuName = "RMB/Menu Item")]
    public class MenuItemSO : ScriptableObject
    {
        public string menuName;
        public MenuItemElementsArrangement elementsArrangement;
        public MenuItemType menuItemType;
        public List<MenuItemSO> elements;
        public KeyBind keyBind;

        public delegate void OnClick();

        public event OnClick onClickEvent;
    }
}