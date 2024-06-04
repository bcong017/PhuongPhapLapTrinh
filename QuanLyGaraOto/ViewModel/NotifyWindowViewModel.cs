using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class NotifyWindowViewModel : BaseViewModel
    {
        private string notification;

        public string Notification
        {
            get { return notification; }
            set { 
                notification = value; 
                OnPropertyChanged(nameof(Notification));
            }
        }

        public ICommand CloseCommand { get; set; }

        public NotifyWindowViewModel(string information)
        {
            Notification = information;
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }    
    }
}
