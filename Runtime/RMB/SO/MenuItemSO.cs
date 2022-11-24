using System.Collections.Generic;
using RMB.UI;
using UnityEngine;
using UnityEngine.Events;

namespace RMB.SO
{
    [CreateAssetMenu(menuName = "RMB/Menu Item")]
    public class MenuItemSO : ScriptableObject
    {
        public string menuName;
        public MenuItemType menuItemType;
        public List<MenuItemSO> elements;
        public string shortcutEvent;
        public UnityEvent<MenuItem> onClickEvent;
    }
}