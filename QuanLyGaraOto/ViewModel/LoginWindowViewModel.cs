using QuanLyGaraOto.AddingClasses;
using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class LoginWindowViewModel : BaseViewModel
    {
        public bool isLoggedIn = false;
        public bool isAdmin;
        public bool enableLoginBtn;
        private string userName;
        private string password;

        
        public string UserName
        {
            get { return userName; }
            set 
            { 
                userName = value;
                EnableLoginButtonChecker();
            }
        }

        public string Password
        {
            get { return password; }
            set 
            { 
                password = value; 
                
                EnableLoginButtonChecker();
            } 
        }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand OpenForgetPassWindowCommand { get; set; }
        public LoginWindowViewModel()
        {
            UserName = "";
            Password = "";
            LoginCommand = new RelayCommand<Window>((p) => { return EnableLoginButtonChecker(); }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ExitCommand = new RelayCommand<object>((p) => { return true; }, (p) => { Application.Current.Shutdown(); });
            OpenForgetPassWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                OpenWindow(p);
            });
        }

        void Login(Window p)
        {
            if (p == null)
                return;

            string passEncode = Encryptor.CreateMD5(Encryptor.Base64Encode(Password));

            // truy vấn dữ liệu sql ở đây, chưa có nên chưa làm lmao :v
            // chỗ này để tạm vô là login đc thôi nhá còn bao giờ truy vấn đc rồi thì cho điều kiện vào đây
            // sử dụng hàm "Notification.Notify" để thông báo nếu đăng nhập chưa ok
            TAIKHOAN user = DataProvider.Instance.DB.TAIKHOANs.Where(x => x.TenTaiKhoan == UserName && x.MatKhau == passEncode).FirstOrDefault();
            if (user != null)
            {
                MainViewModel.User = user;
                MainWindow mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                Application.Current.MainWindow.Show();
                p.Close();
            }
            else
            {
                NotificationWindow.Notify("Tài khoản hoặc mật khẩu không đúng!");
            }    

        }

        void OpenWindow(Window p)
        {
            ForgetPasswordWindow forgetPasswordWindow = new ForgetPasswordWindow();
            Application.Current.MainWindow = forgetPasswordWindow;
            Application.Current.MainWindow.Show();
            p.Close();
        }

        bool EnableLoginButtonChecker()
        {
            if (string.IsNullOrEmpty(userName) == false && string.IsNullOrEmpty(password) == false)
                return true;
            else
                return false;
        }
    }
}
