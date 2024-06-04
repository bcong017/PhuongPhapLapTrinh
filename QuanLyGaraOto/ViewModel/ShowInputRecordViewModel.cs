using QuanLyGaraOto.AddingClasses;
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
    
    public class ShowInputRecordViewModel : BaseViewModel
    {
        private int number;
        private MonthList months = new MonthList();
        public MonthList Months
        {
            get { return months; }
            set { months = value; OnPropertyChanged(); }
        }
        private string year;
        public string Year
        {
            get { return year; }
            set { year = value; OnPropertyChanged(); }
        }

        private Month selectedItem;
        public Month SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }

        private Decimal totalMoney;
        public Decimal TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AccessoriesNumbericalOrder> inputList;
        public ObservableCollection<AccessoriesNumbericalOrder> InputList
        { 
            get { return inputList; }
            set { inputList = value; OnPropertyChanged(); }
        }

        public ICommand ShowCommand { get; set; }
        public ShowInputRecordViewModel()
        {
            InputList = new ObservableCollection<AccessoriesNumbericalOrder>();
            ShowCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null || Int32.TryParse(Year, out _) == false)
                    return false;
                return true;
            }, (p) =>
            {
                InputList.Clear();
                TotalMoney = 0;
                number = Convert.ToInt32(Year);
                PHIEUNHAP phieunhap = DataProvider.Instance.DB.PHIEUNHAPs.FirstOrDefault(x => x.NgayNhap.Value.Month == SelectedItem.Number && x.NgayNhap.Value.Year == number);
                if (phieunhap == null)
                {
                    NotificationWindow.Notify("Không có phiếu nhập vật tư trùng khớp với thời gian");
                    return;
                }
                TotalMoney = phieunhap.ThanhTien ?? 0;
                List<CT_PHIEUNHAP> list = DataProvider.Instance.DB.CT_PHIEUNHAP.Where(x => x.MaPhieuNhap == phieunhap.MaPhieuNhap).ToList();
                LoadInputList(list);
            });
        }

        private void LoadInputList(List<CT_PHIEUNHAP> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                AccessoriesNumbericalOrder accessoriesNumbericalOrder = new AccessoriesNumbericalOrder();
                accessoriesNumbericalOrder.Number = i + 1;
                accessoriesNumbericalOrder.TenVatTu = list[i].VATTU.TenVatTu;
                accessoriesNumbericalOrder.DonGiaNhap = list[i].DonGiaNhap ?? 0;
                accessoriesNumbericalOrder.DonGiaBanDeNghi = list[i].DonGiaBan ?? 0;
                accessoriesNumbericalOrder.SoLuong = list[i].SoLuong ?? 0;
                accessoriesNumbericalOrder.ThanhTien = list[i].ThanhTien ?? 0;
                InputList.Add(accessoriesNumbericalOrder);
            }    
        }
    }

    
}
