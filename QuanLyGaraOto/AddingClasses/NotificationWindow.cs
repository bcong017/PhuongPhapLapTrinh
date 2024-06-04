using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.AddingClasses
{
    public static class NotificationWindow
    {
        public static void Notify(string information)
        {
            NotifyWindow notifyWindow = new NotifyWindow(information);
            notifyWindow.ShowDialog();
        }
    }
}
