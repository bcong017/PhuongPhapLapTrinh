using QuanLyGaraOto.AddingClasses;
using QuanLyGaraOto.Model;
using QuanLyGaraOto.Builder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace QuanLyGaraOto.ViewModel
{
    public class CarCheckInViewModel : BaseViewModel
    {
        private List<HIEUXE> _ListHieuXe;
        public List<HIEUXE> ListHieuXe { get { return _ListHieuXe; } set { _ListHieuXe = value; OnPropertyChanged(); } }

        private string _HoTen;
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged(); } }

        private string _BienSo;
        public string BienSo { get => _BienSo; set { _BienSo = value; OnPropertyChanged(); } }

        private HIEUXE _HieuXe;
        public HIEUXE HieuXe { get => _HieuXe; set { _HieuXe = value; OnPropertyChanged(); } }

        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }

        private DateTime? _NgayTiepNhan;
        public DateTime? NgayTiepNhan { get => _NgayTiepNhan; set { _NgayTiepNhan = value; OnPropertyChanged(); } }

        private string _SoDienThoai;
        public string SoDienThoai { get => _SoDienThoai; set { _SoDienThoai = value; OnPropertyChanged(); } }

        private bool _EditTextboxEnable = true;
        public bool EditTextboxEnable { get => _EditTextboxEnable; set { _EditTextboxEnable = value; OnPropertyChanged();} }

        private ReceiveNumbericalOrder _SelectedItem;
        public ReceiveNumbericalOrder SelectedItem 
        { 
            get => _SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged(); 
                if (SelectedItem!=null)
                {
                    EditTextboxEnable = false;
                    BienSo = SelectedItem.Car.BienSo;
                    DiaChi = SelectedItem.Car.DiaChi;
                    SoDienThoai = SelectedItem.Car.DienThoai;
                    HoTen = SelectedItem.Car.TenChuXe;
                    NgayTiepNhan = SelectedItem.Car.NgayTiepNhan;
                    HieuXe = SelectedItem.Car.HIEUXE;
                }
            } 
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DeSelectedItemCommand { get; set; }

        private ObservableCollection<ReceiveNumbericalOrder> _ListTiepNhanXeSua;
        public ObservableCollection<ReceiveNumbericalOrder> ListTiepNhanXeSua { get => _ListTiepNhanXeSua; set { _ListTiepNhanXeSua = value; OnPropertyChanged(); } } 
        public CarCheckInViewModel()
        {
            LoadInputList();

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(HoTen))
                    return false;
                if (string.IsNullOrEmpty(BienSo))
                    return false;
                if (HieuXe == null)
                    return false;
                if (string.IsNullOrEmpty(DiaChi))
                    return false;
                if (NgayTiepNhan == null)
                    return false;
                if (string.IsNullOrEmpty(SoDienThoai))
                    return false;
                return true;
            }, (p) =>
            {
                var thamso = DataProvider.Instance.DB.THAMSOes.Single();                
                var cars = DataProvider.Instance.DB.XEs.Where(x => DbFunctions.TruncateTime(x.NgayTiepNhan) == DateTime.Today).ToList();
                if (cars.Count > thamso.XeToiDa)
                {
                    NotificationWindow.Notify("Số xe tiếp nhận đã vượt quá tối đa cho phép!");
                    return;
                }
                if (DataProvider.Instance.DB.XEs.Any(x => x.BienSo == BienSo) == true)
                {
                    NotificationWindow.Notify("Đã có xe có biển số trùng với biển số bạn đã nhập!");
                    return;
                }
                //var xe = new XE() { BienSo = BienSo.Trim(), DiaChi = DiaChi.Trim(), DienThoai = SoDienThoai.Trim(), Email = null, 
                //    TenChuXe = HoTen.Trim(), NgayTiepNhan = NgayTiepNhan, TienNo = 0, MaHieuXe = HieuXe.MaHieuXe, HIEUXE = HieuXe };
                CarBuilder carBuilder = new CarBuilder();
                carBuilder.buildBienSo(BienSo.Trim());
                carBuilder.buildDiaChi(DiaChi.Trim());
                carBuilder.buildDT(SoDienThoai.Trim());
                carBuilder.buildEmail(string.Empty);
                carBuilder.buildHoTen(HoTen.Trim());
                carBuilder.buildNgayTiepNhan(NgayTiepNhan);
                carBuilder.buildHieuXe(HieuXe);

                XE xe = carBuilder.getResult();
                ReceiveNumbericalOrder receiveNumbericalOrder = new ReceiveNumbericalOrder();
                receiveNumbericalOrder.Car = xe;
                receiveNumbericalOrder.Number = ListTiepNhanXeSua.Count + 1;
                ListTiepNhanXeSua.Add(receiveNumbericalOrder);
                DataProvider.Instance.DB.XEs.Add(xe);
                DataProvider.Instance.DB.SaveChanges();

                HoTen = DiaChi = SoDienThoai = BienSo = "";
                HieuXe = null; SelectedItem = null;
                NgayTiepNhan = DateTime.Today; 
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var xe = DataProvider.Instance.DB.XEs.Where(x => x.BienSo == SelectedItem.Car.BienSo).FirstOrDefault();
                //CarBuilder carBuilder = new CarBuilder();
                xe.TenChuXe = HoTen;
                xe.MaHieuXe = HieuXe.MaHieuXe;
                xe.DiaChi = DiaChi;
                xe.NgayTiepNhan = NgayTiepNhan;
                xe.DienThoai = SoDienThoai;
                xe.HIEUXE = HieuXe;
                DataProvider.Instance.DB.SaveChanges();
                for (int i = 0; i < ListTiepNhanXeSua.Count; i++)
                {
                    if (SelectedItem.Car.BienSo == ListTiepNhanXeSua[i].Car.BienSo)
                    {
                        ListTiepNhanXeSua[i].Car = xe;
                        break;
                    }
                }
                EditTextboxEnable = true;
                HoTen = BienSo = DiaChi = SoDienThoai = "";
                HieuXe = null;
                NgayTiepNhan = DateTime.Today;
                SelectedItem = null;
            });

            DeSelectedItemCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                HoTen = "";
                HieuXe = null;
                DiaChi = "";
                NgayTiepNhan = DateTime.Today;
                SoDienThoai = "";
                BienSo = "";
                SelectedItem = null;
                EditTextboxEnable = true;
            });
        }

        private void LoadInputList()
        {
            NgayTiepNhan = DateTime.Now;
            ListHieuXe = DataProvider.Instance.DB.HIEUXEs.ToList();
            ListTiepNhanXeSua = new ObservableCollection<ReceiveNumbericalOrder>();
            var Cars = DataProvider.Instance.DB.XEs.ToList();
            for (int i = 0; i < Cars.Count; i++)
            {
                ReceiveNumbericalOrder receiveNumbericalOrder = new ReceiveNumbericalOrder();
                receiveNumbericalOrder.Car = Cars[i];
                receiveNumbericalOrder.Number = i + 1;
                ListTiepNhanXeSua.Add(receiveNumbericalOrder);
            }
        }
    }
}
