using QuanLyGaraOto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.Model
{
    public class AccessoriesNumbericalOrder : BaseViewModel
    {
        private int number;
        public int Number { get => number; set { number = value; OnPropertyChanged(); } }

        private string tenVatTu;
        public string TenVatTu { get => tenVatTu; set { tenVatTu = value; OnPropertyChanged(); } }

        private decimal donGiaNhap;
        public decimal DonGiaNhap { get => donGiaNhap; set { donGiaNhap = value; OnPropertyChanged(); } }

        private decimal donGiaBanDeNghi;
        public decimal DonGiaBanDeNghi { get => donGiaBanDeNghi; set { donGiaBanDeNghi = value; OnPropertyChanged(); } }

        private int soLuong;
        public int SoLuong { get => soLuong; set { soLuong = value; OnPropertyChanged(); } }

        private decimal thanhTien;
        public decimal ThanhTien { get => thanhTien; set { thanhTien = value; OnPropertyChanged(); } }

    }
}
