using UnityEngine;

namespace RMB.Util
{
    [CreateAssetMenu(menuName = "RMB/Key Bind")]
    public class KeyBind : ScriptableObject
    {
        public KeyCode keyCode;
        public bool cmd, ctrl, fn, alt, win, shift;

        public bool IsJustPressed()
        {
            return IsFunctionKeysPressed() && Input.GetKeyDown(keyCode);
        }

        public bool IsFunctionKeysPressed()
        {
            if (cmd && (!Input.GetKey(KeyCode.LeftCommand) && (!Input.GetKey(KeyCode.RightCommand)))) return false;
            if (ctrl && (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.RightControl)))) return false;
            if (fn && (!Input.GetKey(KeyCode.LeftMeta) && (!Input.GetKey(KeyCode.RightMeta)))) return false;
            if (shift && (!Input.GetKey(KeyCode.LeftShift) && (!Input.GetKey(KeyCode.RightShift)))) return false;
            if (alt && (!Input.GetKey(KeyCode.LeftAlt) && (!Input.GetKey(KeyCode.RightAlt)))) return false;
            if (win && (!Input.GetKey(KeyCode.LeftWindows) && (!Input.GetKey(KeyCode.RightWindows)))) return false;
            return true;
        }

        private static string S(bool b, string s) => b ? s : string.Empty;

        public override string ToString()
        {
            return S(cmd, "<cmd>") + S(ctrl, "<ctrl>") + S(fn, "<fn>") + S(alt, "<alt>") + S(win, "<win>")
                    + S(shift, "<shift>") + keyCode;
        }
    }
}