using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.Model
{
    public class ContentNumbericalOrder : BaseViewModel
    {
        public static int orderNumber = 1;
        private int number;
        public int Number { get { return number; } set { number = value; OnPropertyChanged(); } }

        private string noiDung;
        public string NoiDung { get { return noiDung; } set { noiDung = value; OnPropertyChanged(); } }

        private int soLan;
        public int SoLan { get { return soLan; } set { soLan = value; OnPropertyChanged(); } }

        private string tenTienCong;
        public string TenTienCong { get { return tenTienCong; } set { tenTienCong = value; OnPropertyChanged(); } }

        private Decimal thanhTien;
        public Decimal ThanhTien { get { return thanhTien; } set { thanhTien = value; OnPropertyChanged(); } }

        public int MaTienCong { get; set; }
        public int itemNumber { get; set; }

        public List<ItemNumbericalOrder> itemList = new List<ItemNumbericalOrder>();
        public List<ItemNumbericalOrder> ItemList { get { return itemList; } set { itemList = value; OnPropertyChanged(); } }
    }

    public class ItemNumbericalOrder : BaseViewModel
    {
        private int order { get; set; }
        public int Order { get { return order; } set { order = value; OnPropertyChanged(); } }

        private int number;
        public int Number { get { return number; } set { number = value; OnPropertyChanged(); } }

        private string tenVatTu;
        public string TenVatTu { get { return tenVatTu; } set { tenVatTu = value; OnPropertyChanged(); } }

        private int soLuong;
        public int SoLuong { get { return soLuong; } set { soLuong = value; OnPropertyChanged(); } }

        private Decimal thanhTien;
        public Decimal ThanhTien { get { return thanhTien;} set { thanhTien = value; OnPropertyChanged(); } }

        public int MaVatTu { get; set; }
    }

    public class ReceiptNumbericalOrder : BaseViewModel
    {
        public int Number { get; set; }
        public PHIEUSUACHUA Phieu { get; set; }
    }
}
