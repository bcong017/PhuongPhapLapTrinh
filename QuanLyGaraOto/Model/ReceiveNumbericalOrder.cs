using QuanLyGaraOto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.Model
{
    public class ReceiveNumbericalOrder : BaseViewModel
    {
        private int _Number;
        public int Number { get => _Number; set { _Number = value; OnPropertyChanged(); } }

        private XE _Car;
        public XE Car { get => _Car; set { _Car = value; OnPropertyChanged(); } }
    }
}
