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
    public class BillViewModel : BaseViewModel
    {
        private Decimal money;
        private string _BienSo;
        public string BienSo { get => _BienSo; set { _BienSo = value; OnPropertyChanged(); } }

        private DateTime? _NgayThuTien;
        public DateTime? NgayThuTien { get => _NgayThuTien; set { _NgayThuTien = value; OnPropertyChanged(); } }

        private string _SoTienThu;
        public string SoTienThu { get => _SoTienThu; set { _SoTienThu = value; OnPropertyChanged(); } }

        private ObservableCollection<BillsNumbericalOrder> phieuThuTienList;
        public ObservableCollection<BillsNumbericalOrder> PhieuThuTienList
        {
            get { return phieuThuTienList; }
            set { phieuThuTienList = value; OnPropertyChanged(); }
        }
        public ICommand AddCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public BillViewModel()
        {
            NgayThuTien = DateTime.Now;
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(BienSo))
                    return false;
                if (string.IsNullOrEmpty(SoTienThu))
                    return false;
                if (NgayThuTien == null)
                    return false;
                if (Decimal.TryParse(SoTienThu, out _) == false)
                    return false;
                return true;
            }, (p) =>
            {
                var xe = DataProvider.Instance.DB.XEs.FirstOrDefault(x => String.Compare(x.BienSo, BienSo) == 0);

                if (xe == null)
                {
                    NotificationWindow.Notify("Không tìm được xe có biển số xe bạn đã nhập!");
                    return;
                }
                money = Convert.ToDecimal(SoTienThu);
                if (xe.TienNo == 0)
                {
                    NotificationWindow.Notify("Xe bạn nhập không nợ tiền.");
                    return;
                }    
                if (xe.TienNo < money)
                {
                    NotificationWindow.Notify("Số tiền thu không được vượt quá số tiền nợ");
                    return;
                }
                xe.TienNo -= money; 
                PHIEUTHUTIEN phieuthutien = new PHIEUTHUTIEN() {BienSo = BienSo, NgayThuTien = NgayThuTien, SoTienThu = money, XE = xe };
                DataProvider.Instance.DB.PHIEUTHUTIENs.Add(phieuthutien);
                DataProvider.Instance.DB.SaveChanges();
               
                
                NotificationWindow.Notify("Thêm phiếu thu tiền thành công!");
            });
            SearchCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(BienSo))
                    return false;
                return true;
            }, (p) => 
            {
                PhieuThuTienList = new ObservableCollection<BillsNumbericalOrder>();
                
                List<PHIEUTHUTIEN> result = DataProvider.Instance.DB.PHIEUTHUTIENs.Where(x => x.BienSo == BienSo).ToList();
                XE xE = DataProvider.Instance.DB.XEs.Where(x => x.BienSo == BienSo).FirstOrDefault();
                if (xE == null)
                {
                    NotificationWindow.Notify("Không có xe bạn đang tìm!");
                    return;
                }    
                if (result.Count == 0)
                {
                    NotificationWindow.Notify("Xe bạn đang tìm không có phiếu thu tiền!");
                    return;
                }

                for (int i = 0; i < result.Count; i++)
                {
                    BillsNumbericalOrder billsNumbericalOrder = new BillsNumbericalOrder();
                    billsNumbericalOrder.Number = i + 1;
                    billsNumbericalOrder.PhieuThuTien = result[i];
                    PhieuThuTienList.Add(billsNumbericalOrder);
                }
            });
        }
    }
}
