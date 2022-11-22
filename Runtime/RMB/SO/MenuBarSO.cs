using System.Collections.Generic;
using UnityEngine;

namespace RMB.SO
{
    [CreateAssetMenu(menuName = "RMB/Menu Bar")]
    public class MenuBarSO : ScriptableObject
    {
        public List<RootMenuItemSO> rootMenuItems;
    }
}