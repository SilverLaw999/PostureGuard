using System.Media;
using Microsoft.Toolkit.Uwp.Notifications;

namespace PostureGuard.Libraries
{
    public static class NotificationLibrary
    {
        public static void Send()
        {
            SystemSounds.Exclamation.Play();

            new ToastContentBuilder()
                .AddText("FIX POSTURE")
                .AddButton(new ToastButton().SetContent("Acknowledged").AddArgument("action", "dismiss"))
                .Show();
        }
    }
}