using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class UpdateParameterViewModel : BaseViewModel
    {
        private string[] name = { "Số xe tối đa", "Tỉ lệ nhập - bán", "Số tiền thu > số tiền nợ" };
        private string percentage;
        public string Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged(nameof(Percentage)); }
        }

        private string carNumber;
        public string CarNumber
        {
            get { return carNumber; }
            set { carNumber = value; OnPropertyChanged(nameof(CarNumber)); }
        }

        private bool? checker;
        public bool? Checker
        {
            get { return checker; }
            set { checker = value; OnPropertyChanged(); }
        }

        private CustomTHAMSO selectedItem;
        public CustomTHAMSO SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value; 
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    CarNumber = SelectedItem.CarNumber.ToString();
                    Checker = SelectedItem.Checker;
                    Percentage = (SelectedItem.Percentage * 100).ToString();
                }    
            }
        }
        private ObservableCollection<CustomTHAMSO> list;
        public ObservableCollection<CustomTHAMSO> List
        {
            get { return list; }
            set { list = value; OnPropertyChanged(); }
        }

        public ICommand UpdateCheckerCommand { get; set; }
        public ICommand UpdatePercentageCommand { get; set; }
        public ICommand UpdateCarCommand { get; set; }
        public UpdateParameterViewModel()
        {
            LoadThamSo();
            UpdateCheckerCommand = new RelayCommand<object>((p) => { return SelectedItem != null; }, (p) =>
            {
                THAMSO thamso = DataProvider.Instance.DB.THAMSOes.FirstOrDefault();
                thamso.SoTienThu = Checker;
                DataProvider.Instance.DB.SaveChanges();

                SelectedItem.Checker = Checker;
            });

            UpdateCarCommand = new RelayCommand<object>((p) =>
            {
                if (Int32.TryParse(CarNumber, out _) == false || Convert.ToInt32(CarNumber) < 1)
                    return false;
                if (SelectedItem == null)
                    return false;   
                return true;
            }, (p) =>
            {
                THAMSO thamso = DataProvider.Instance.DB.THAMSOes.FirstOrDefault();
                thamso.XeToiDa = Convert.ToInt32(CarNumber);
                DataProvider.Instance.DB.SaveChanges();

                SelectedItem.CarNumber = Convert.ToInt32(CarNumber);
            });

            UpdatePercentageCommand = new RelayCommand<object>((p) =>
            {
                if (Double.TryParse(Percentage, out _) == false)
                    return false;
                if (Convert.ToDouble(Percentage) < 1)
                    return false;
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                THAMSO thamso = DataProvider.Instance.DB.THAMSOes.FirstOrDefault();
                thamso.PhanTram = Convert.ToDouble(Percentage) / 100;
                DataProvider.Instance.DB.SaveChanges();

                SelectedItem.Percentage = Convert.ToDouble(Percentage) / 100;
            });
        }

        private void LoadThamSo()
        {
            THAMSO thamso= DataProvider.Instance.DB.THAMSOes.FirstOrDefault();
            CustomTHAMSO customthamso = new CustomTHAMSO() { CarNumber = thamso.XeToiDa, Checker = thamso.SoTienThu, Percentage = thamso.PhanTram };
            List = new ObservableCollection<CustomTHAMSO>();
            List.Add(customthamso);
            
        }
    }

    public class CustomTHAMSO : BaseViewModel
    {
        private int? carNumber;
        public int? CarNumber 
        { 
            get => carNumber; 
            set { carNumber = value; OnPropertyChanged(); }
        }

        private bool? checker;
        public bool? Checker 
        { 
            get => checker;
            set { checker = value; OnPropertyChanged(); }
        }

        private double? percentage;
        public double? Percentage 
        { 
            get => percentage;
            set { percentage = value; OnPropertyChanged(); } 
        }
    }
}
