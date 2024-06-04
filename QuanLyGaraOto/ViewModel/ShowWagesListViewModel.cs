using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class ShowWagesListViewModel : BaseViewModel
    {
        private Visibility btnVisibility;
        public Visibility BtnVisibility
        {
            get { return btnVisibility; }
            set { btnVisibility = value; }
        }
        
        private string wageName;
        public string WageName
        {
            get { return wageName; }
            set { wageName = value; OnPropertyChanged(nameof(WageName)); }
        }

        private string price;
        public string Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(nameof(Price)); }
        }



        private TIENCONG selectedItem;
        public TIENCONG SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    WageName = SelectedItem.TenTienCong;
                    Price = Convert.ToInt32(SelectedItem.GiaTienCong).ToString();
                }
            }
        }

        private ObservableCollection<TIENCONG> tiencongList;
        public ObservableCollection<TIENCONG> TienCongList
        {
            get { return tiencongList; }
            set { tiencongList = value; OnPropertyChanged(nameof(TienCongList)); }
        }
        public ICommand DeSelectedItemCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ShowWagesListViewModel()
        {
            TienCongList = new ObservableCollection<TIENCONG>(DataProvider.Instance.DB.TIENCONGs.ToList());

            if (MainViewModel.User.QuyenHan == 0)
            {
                BtnVisibility = Visibility.Collapsed;
            }
            else
            {
                BtnVisibility = Visibility.Visible;
            }

            DeSelectedItemCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                SelectedItem = null;
                WageName = Price = "";
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(WageName) || string.IsNullOrEmpty(Price))
                    return false;
                if (Decimal.TryParse(Price, out _) == false)
                    return false;
                if (DataProvider.Instance.DB.TIENCONGs.Any(x => x.TenTienCong == WageName))
                    return false;

                return true;
            }, (p) =>
            {
                var tiencong = new TIENCONG() { TenTienCong = WageName.Trim(), GiaTienCong = Convert.ToDecimal(Price) };

                DataProvider.Instance.DB.TIENCONGs.Add(tiencong);
                DataProvider.Instance.DB.SaveChanges();

                TienCongList.Add(tiencong);
                WageName = Price = "";
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null || Decimal.TryParse(Price, out _) == false)
                    return false;
                return true;
            }, (p) =>
            {
                var tiencong = DataProvider.Instance.DB.TIENCONGs.SingleOrDefault(x => x.MaTienCong == SelectedItem.MaTienCong);
                if (tiencong == null)
                    return;

                tiencong.TenTienCong = WageName.Trim();
                tiencong.GiaTienCong = Convert.ToDecimal(Price);

                DataProvider.Instance.DB.SaveChanges();

                TienCongList = new ObservableCollection<TIENCONG>(DataProvider.Instance.DB.TIENCONGs.ToList());

            });
        }
    }
}
