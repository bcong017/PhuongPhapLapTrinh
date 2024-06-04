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
    public class AccountInformationViewModel : BaseViewModel
    {
        private bool enableState;

        public bool EnableState
        {
            get { return enableState; }
            set
            {
                enableState = value;
                OnPropertyChanged(nameof(EnableState));
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private DateTime? birthday;
        public DateTime? Birthday
        {
            get { return birthday; }
            set { birthday = value; OnPropertyChanged(); }
        }



        public ICommand CloseCommand { get; set; }
        public ICommand AllowToChangeCommand { get; set; }
        public ICommand OpenChangePasswordWindowCommand { get; set; }
        public ICommand SaveChangeAccount { get; set; }

        public AccountInformationViewModel()
        {
            LoadInformation();
            EnableState = false;
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            AllowToChangeCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                if (EnableState == false)
                {
                    EnableState = true;
                    p.Content = "Hủy chỉnh sửa";
                }    
                else
                {
                    EnableState = false;
                    p.Content = "Chỉnh sửa";
                }
            });

            OpenChangePasswordWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            { 
                ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(); 
                changePasswordWindow.ShowDialog();
            });

            SaveChangeAccount = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || ValidateEmail.EmailIsValid(Email) == false || Birthday == null)
                    return false;
                return true;
            }, (p) =>
            {
                TAIKHOAN taikhoan = DataProvider.Instance.DB.TAIKHOANs.SingleOrDefault(x => x.MaTaiKhoan == MainViewModel.User.MaTaiKhoan);
                taikhoan.HoTen = Name.Trim();
                taikhoan.Email = Email.Trim();
                taikhoan.NgaySinh = Birthday;

                DataProvider.Instance.DB.SaveChanges();
                NotificationWindow.Notify("Chỉnh sửa thành công");
                p.Close();
            });
        }

        private void LoadInformation()
        {
            Name = MainViewModel.User.HoTen;
            Email = MainViewModel.User.Email;
            Birthday = MainViewModel.User.NgaySinh;
        }
    }
}
