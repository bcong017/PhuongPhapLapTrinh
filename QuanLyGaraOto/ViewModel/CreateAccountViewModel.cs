using QuanLyGaraOto.AddingClasses;
using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class CreateAccountViewModel : BaseViewModel
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        private string password = "12345";
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        
        public ICommand CloseCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand ShowAccountListCommand { get; set; }
        public CreateAccountViewModel()
        {
            CloseCommand = new RelayCommand<Window>((p) =>{ return true; }, (p) => { p.Close(); });
            CreateAccountCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
                    return false;
                if (ValidateEmail.EmailIsValid(Email) == false)
                    return false;
                if (DataProvider.Instance.DB.TAIKHOANs.Any(x => x.TenTaiKhoan == UserName) == true)
                    return false;
                return true;
            }, (p) =>
            {
                string passEncode = Encryptor.CreateMD5(Encryptor.Base64Encode(Password.Trim()));
                VAITRO vaitro = DataProvider.Instance.DB.VAITROes.FirstOrDefault(x => x.MaVaiTro == 0);
                TAIKHOAN taikhoan = new TAIKHOAN() { TenTaiKhoan = UserName.Trim(), MatKhau = passEncode, Email = Email.Trim(), QuyenHan = vaitro.MaVaiTro, VAITRO = vaitro };
                DataProvider.Instance.DB.TAIKHOANs.Add(taikhoan);
                DataProvider.Instance.DB.SaveChanges();
                NotificationWindow.Notify("Tạo tài khoản thành công!");
            });

            ShowAccountListCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ShowAccountListWindow showAccountListWindow = new ShowAccountListWindow(); showAccountListWindow.Show(); });

        }
    }
}
