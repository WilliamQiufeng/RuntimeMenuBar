using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RMB.SO
{
    [CreateAssetMenu(menuName = "RMB/Menu Item")]
    public class MenuItemSO : ScriptableObject
    {
        public string menuName;
        public MenuItemElementsArrangement elementsArrangement;
        public MenuItemType menuItemType;
        public List<MenuItemSO> elements;
        public string shortcutEvent;
        public UnityEvent onClickEvent;
    }
}