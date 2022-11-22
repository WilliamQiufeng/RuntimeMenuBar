using System.Collections.Generic;
using UnityEngine;

namespace RMB.SO
{
    public class MenuItemSO : ScriptableObject
    {
        public string menuName;
        public MenuItemElementsArrangement elementsArrangement;
        public List<MenuItemSO> elements;

        public delegate void OnClick();

        public event OnClick onClickEvent;
    }
}