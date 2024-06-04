using QuanLyGaraOto.AddingClasses;
using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class RecoverPasswordViewModel : BaseViewModel
    {
        private string verifiedNumberString;
        private string password;
        private string configuration;
        private int verifiedNumber;
        public string VerifiedNumberString
        { 
            get { return verifiedNumberString; } 
            set { verifiedNumberString = value; OnPropertyChanged(); } 
        }

        public string Password 
        { 
            get { return password; } 
            set { password = value; OnPropertyChanged(); }
        }

        public string Configuration
        {
            get { return configuration; }
            set { configuration = value; OnPropertyChanged(); }
        }

        public ICommand PasswordChanged { get; set; }
        public ICommand ConfigurationChanged { get; set; }
        public ICommand ResendEmailCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand RecoverPasswordCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public RecoverPasswordViewModel()
        {
            PasswordChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ConfigurationChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Configuration = p.Password; });

            ResendEmailCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                string fromEmail = ConfigurationManager.AppSettings.Get("EmailAddress");
                string fromPassword = ConfigurationManager.AppSettings.Get("GeneratedPassword");

                EmailSender.Instance.SendEmail(fromEmail, fromPassword);
                NotificationWindow.Notify("Đã gửi lại mã xác nhận!");

            });
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => 
            { 
                LoginWindow loginWindow = new LoginWindow();
                Application.Current.MainWindow = loginWindow;
                Application.Current.MainWindow.Show();
                p.Close(); 
            });
            RecoverPasswordCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Configuration) || string.IsNullOrEmpty(VerifiedNumberString))
                    return false;
                else if (Password != Configuration)
                    return false;
                else if (Int32.TryParse(VerifiedNumberString, out verifiedNumber) == false)
                    return false;
                else return true;
            }, (p) =>
            {
                if (verifiedNumber == EmailSender.Instance.VerifiedNumber)
                {
                    var user = DataProvider.Instance.DB.TAIKHOANs.Where(x => x.Email == EmailSender.Instance.Gmail).FirstOrDefault();
                    if (user != null)
                    {
                        user.MatKhau = Encryptor.CreateMD5(Encryptor.Base64Encode(Password));
                        DataProvider.Instance.DB.SaveChanges();
                        NotificationWindow.Notify("Cập nhật mật khẩu thành công!");
                        LoginWindow loginWindow = new LoginWindow();
                        Application.Current.MainWindow = loginWindow;
                        Application.Current.MainWindow.Show();
                        p.Close();
                    }
                }
                else
                    NotificationWindow.Notify("Mã xác nhận không đúng!");
            });
            BackCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                ForgetPasswordWindow forgetPasswordWindow = new ForgetPasswordWindow();
                Application.Current.MainWindow = forgetPasswordWindow;
                Application.Current.MainWindow.Show();
                p.Close();
            });
        }
    }
}
