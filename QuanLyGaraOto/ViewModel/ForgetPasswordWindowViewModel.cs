using QuanLyGaraOto.AddingClasses;
using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class ForgetPasswordWindowViewModel : BaseViewModel
    {
        private string userName;
        private string gmail;

        public string UserName
        {
            get { return userName; }
            set 
            { 
                userName = value;
            }
        }
        public string Gmail
        {
            get { return gmail; }
            set 
            { 
                gmail = value;
            }
        }

        public ICommand CloseCommand { get; set; }
        public ICommand SendMailCommand { get; set; }
        
        public ForgetPasswordWindowViewModel()
        {
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                //if (p == null)
                //    return;
                LoginWindow loginWindow = new LoginWindow();
                Application.Current.MainWindow = loginWindow;
                Application.Current.MainWindow.Show();
                p.Close();
            });

            SendMailCommand = new RelayCommand<Window>((p) => 
            {
                if (string.IsNullOrEmpty(Gmail) == false)
                    return true;
                else
                    return false;
            }, ((p) => { SendMail(p); }));
        }
        void SendMail(Window p)
        {
            if (CheckValidUsername() && ValidateEmail.EmailIsValid(Gmail))
            {
                string fromEmail = ConfigurationManager.AppSettings.Get("EmailAddress");
                string fromPassword = ConfigurationManager.AppSettings.Get("GeneratedPassword");
                EmailSender.Instance.Gmail = Gmail;
                EmailSender.Instance.SendEmail(fromEmail, fromPassword);
                NotificationWindow.Notify("Gửi mã xác thực thành công!");
                RecoverPasswordWindow recoverPasswordWindow = new RecoverPasswordWindow();
                Application.Current.MainWindow = recoverPasswordWindow;
                Application.Current.MainWindow.Show();
                p.Close();
            }
            else
            {
                NotificationWindow.Notify("Gửi mã xác nhận thất bại");
            }    

        }

        bool CheckValidUsername()
        {
            // hàm check tài khoản nè và lấy cái password đó ra luôn
            if (DataProvider.Instance.DB.TAIKHOANs.Any(x => x.Email == Gmail))
                return true; 
            else
                return false;
        }

    }
}
