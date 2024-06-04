using QuanLyGaraOto.AddingClasses;
using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyGaraOto.ViewModel;
using System.Windows.Documents;
using System.Collections.ObjectModel;

namespace QuanLyGaraOto.Facade
{
    public class MakeRepairReceiptFacade
    {
        public void Make(XE SelectedCar, DateTime? SelectedDate, decimal TotalMoney, ObservableCollection<ContentNumbericalOrder> ContentList, List<List<ItemNumbericalOrder>> list)
        {
            PHIEUSUACHUA phieusuachua = new PHIEUSUACHUA() { BienSo = SelectedCar.BienSo, NgaySuaChua = SelectedDate, TongTien = TotalMoney, XE = SelectedCar };
            SelectedCar.TienNo = SelectedCar.TienNo + phieusuachua.TongTien;
            DataProvider.Instance.DB.PHIEUSUACHUAs.Add(phieusuachua);
            DataProvider.Instance.DB.SaveChanges();

            List<CT_PSC> ctpscList = MakeCTPSC(phieusuachua,ContentList, list);
            DataProvider.Instance.DB.CT_PSC.AddRange(ctpscList);

            BAOCAODOANHSO bcds = DataProvider.Instance.DB.BAOCAODOANHSOes.FirstOrDefault(x => x.ThoiGian.Value.Month == phieusuachua.NgaySuaChua.Value.Month && x.ThoiGian.Value.Year == phieusuachua.NgaySuaChua.Value.Year);
            bcds.TongDoanhThu += phieusuachua.TongTien;

            CT_BCDS ctbcds = DataProvider.Instance.DB.CT_BCDS.First(x => x.HIEUXE.MaHieuXe == phieusuachua.XE.HIEUXE.MaHieuXe && x.BAOCAODOANHSO.ThoiGian.Value.Year == phieusuachua.NgaySuaChua.Value.Year && x.BAOCAODOANHSO.ThoiGian.Value.Month == phieusuachua.NgaySuaChua.Value.Month);
            ctbcds.SoLuotSua = ctbcds.SoLuotSua + 1;
            ctbcds.ThanhTien = ctbcds.ThanhTien + phieusuachua.TongTien;
            DataProvider.Instance.DB.SaveChanges();

            NotificationWindow.Notify("Lập phiếu sửa chữa thành công!");
        }
        private List<CT_PSC> MakeCTPSC(PHIEUSUACHUA p, ObservableCollection<ContentNumbericalOrder> ContentList, List<List<ItemNumbericalOrder>> list)
        {
            List<CT_PSC> result = new List<CT_PSC>();
            foreach (var content in ContentList)
            {
                TIENCONG tiencong = DataProvider.Instance.DB.TIENCONGs.Single(x => x.MaTienCong == content.MaTienCong);
                CT_PSC ctpsc = new CT_PSC()
                {
                    MaPhieuSC = p.MaPhieuSC,
                    NoiDung = content.NoiDung,
                    SoLan = content.SoLan,
                    MaTienCong = content.MaTienCong,
                    ThanhTien = content.ThanhTien,
                    TIENCONG = tiencong,
                    PHIEUSUACHUA = p,
                    CT_SUDUNGVATTU = MakeCTSDVT(content.ItemList, p.NgaySuaChua)
                };
                list.Add(content.ItemList);
                result.Add(ctpsc);
            }
            return result;
        }

        private List<CT_SUDUNGVATTU> MakeCTSDVT(List<ItemNumbericalOrder> itemList, DateTime? time)
        {
            List<CT_SUDUNGVATTU> result = new List<CT_SUDUNGVATTU>();
            foreach (var item in itemList)
            {
                VATTU vattu = DataProvider.Instance.DB.VATTUs.Single(x => x.MaVatTu == item.MaVatTu);
                BAOCAOTON baocaoton = DataProvider.Instance.DB.BAOCAOTONs.First(x => x.MaVatTu == item.MaVatTu && x.ThoiGian.Value.Year == time.Value.Year && x.ThoiGian.Value.Month == time.Value.Month);
                CT_SUDUNGVATTU ctsdvt = new CT_SUDUNGVATTU()
                {
                    MaVatTu = vattu.MaVatTu,
                    DonGia = vattu.DonGiaHienTai,
                    SoLuong = item.SoLuong,
                    ThanhTien = item.ThanhTien,
                    VATTU = vattu,
                };
                vattu.SoLuongTon -= ctsdvt.SoLuong;
                baocaoton.PhatSinh = baocaoton.PhatSinh + ctsdvt.SoLuong;
                baocaoton.TonCuoi = baocaoton.TonDau - baocaoton.PhatSinh;
                result.Add(ctsdvt);
            }
            return result;
        }
    }
}
