using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyGaraOto.Model;

namespace QuanLyGaraOto.Builder
{
    public abstract class IBuilder
    {
        abstract public void reset();
        abstract public void buildBienSo(string bs);
        abstract public void buildHoTen(string t);
        abstract public void buildDT(string dt);
        abstract public void buildDiaChi(string dc);
        abstract public void buildEmail(string email);
        abstract public void buildNgayTiepNhan(DateTime? ntn);
        abstract public void buildHieuXe(HIEUXE hx);

    }

    public class CarBuilder : IBuilder
    {
        private XE result;

        public override void reset()
        {
            result = new XE();
        }
        public CarBuilder()
        {
            reset();
            result.TienNo = 0;
        }

        public XE getResult()
        {
            return result;
        }
        public override void buildBienSo(string bs)
        {
            result.BienSo = bs;
        }
        public override void buildHoTen(string t)
        {
            result.TenChuXe = t;
        }
        public override void buildDT(string dt)
        {
            result.DienThoai = dt;
        }
        public override void buildDiaChi(string dc)
        {
            result.DiaChi = dc;
        }
        public override void buildEmail(string email)
        {
            result.Email = email;
        }
        public override void buildNgayTiepNhan(DateTime? ntn)
        {
            result.NgayTiepNhan = ntn;
        }
        public override void buildHieuXe(HIEUXE hx)
        {
            result.HIEUXE = hx;

            result.MaHieuXe = result.HIEUXE.MaHieuXe;
        }
    }

}
