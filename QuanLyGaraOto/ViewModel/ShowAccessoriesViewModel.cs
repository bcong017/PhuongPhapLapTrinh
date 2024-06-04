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
    public class ShowAccessoriesViewModel : BaseViewModel
    {
        private ObservableCollection<VATTU> vattuList;
        public ObservableCollection<VATTU> VatTuList
        {
            get { return vattuList; }
            set { vattuList = value; OnPropertyChanged(nameof(VatTuList)); }
        }
        public ShowAccessoriesViewModel()
        {
            VatTuList = new ObservableCollection<VATTU>(DataProvider.Instance.DB.VATTUs.ToList());
        }
    }
}
