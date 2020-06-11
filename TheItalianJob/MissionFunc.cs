using GTA.Native;

namespace TheItalianJob
{
    public static class MissionFunc
    {
        public static void SendLesterText(string text)
        {
            Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
            Function.Call(Hash._SET_NOTIFICATION_MESSAGE, "CHAR_LESTER", "CHAR_LESTER", false, 1, "Lester", "Message");
        }
    }
}
