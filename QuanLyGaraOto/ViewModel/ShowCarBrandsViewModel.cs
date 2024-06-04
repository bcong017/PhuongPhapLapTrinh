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
    public class ShowCarBrandsViewModel : BaseViewModel
    {
        private Visibility btnVisibility;
        public Visibility BtnVisibility
        {
            get { return btnVisibility; }
            set { btnVisibility = value; OnPropertyChanged(); }
        }
        private string carBrandName;
        public string CarBrandName
        {
            get { return carBrandName; }
            set { carBrandName = value; OnPropertyChanged(nameof(CarBrandName)); }
        }

        private HIEUXE selectedItem;
        public HIEUXE SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value;
                OnPropertyChanged();
                if (selectedItem != null)
                    CarBrandName = SelectedItem.TenHieuXe;
            }
        }

        private ObservableCollection<HIEUXE> hieuxeList;
        public ObservableCollection<HIEUXE> HieuXeList
        {
            get { return hieuxeList; }
            set { hieuxeList = value; OnPropertyChanged(); }
        }
        public ICommand DeSelectedItemCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ShowCarBrandsViewModel()
        {
            HieuXeList = new ObservableCollection<HIEUXE>(DataProvider.Instance.DB.HIEUXEs.ToList());
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
                CarBrandName = "";
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(CarBrandName))
                    return false;
                if (DataProvider.Instance.DB.HIEUXEs.Any(x => x.TenHieuXe == CarBrandName))
                    return false;

                return true;
            }, (p) =>
            {
                var hieuxe = new HIEUXE() { TenHieuXe = CarBrandName.Trim() };

                DataProvider.Instance.DB.HIEUXEs.Add(hieuxe);
                var bcds = DataProvider.Instance.DB.BAOCAODOANHSOes.Where(x=>x.ThoiGian.Value.Month == DateTime.Now.Month && x.ThoiGian.Value.Year == DateTime.Now.Year).FirstOrDefault();
                if (bcds != null)
                {
                    var ct_bcds = new CT_BCDS() { MaHieuXe = hieuxe.MaHieuXe, SoLuotSua = 0, ThanhTien = 0, HIEUXE = hieuxe, BAOCAODOANHSO = bcds, MaBCDS = bcds.MaBCDS };
                    DataProvider.Instance.DB.CT_BCDS.Add(ct_bcds);
                }
                DataProvider.Instance.DB.SaveChanges();
                HieuXeList.Add(hieuxe);


            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var hieuxe = DataProvider.Instance.DB.HIEUXEs.SingleOrDefault(x => x.MaHieuXe == SelectedItem.MaHieuXe);
                if (hieuxe == null)
                    return;

                hieuxe.TenHieuXe = CarBrandName.Trim();

                DataProvider.Instance.DB.SaveChanges();

                HieuXeList = new ObservableCollection<HIEUXE>(DataProvider.Instance.DB.HIEUXEs.ToList());

            });
        }
    }
}
