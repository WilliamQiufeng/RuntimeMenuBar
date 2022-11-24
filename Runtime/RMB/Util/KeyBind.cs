using UnityEngine;

namespace RMB.Util
{
    public static class KeyBind
    {
        private static string S(bool b, string s) => b ? s : string.Empty;

        public static string ToKeybindString(this Event kbEvent)
        {
            return S(kbEvent.command, "<cmd>") +
                   S(kbEvent.control, "<ctrl>") +
                   S(kbEvent.functionKey, "<fn>") +
                   S(kbEvent.alt, "<alt>") +
                   S(kbEvent.shift, "<shift>") +
                   kbEvent.keyCode;
        }
    }
}