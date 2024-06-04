using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyGaraOto;
using QuanLyGaraOto.AddingClasses;

namespace QuanLyGaraOto.ViewModel
{
    public class ShowRepairReceptListViewModel : BaseViewModel
    {
        private List<XE> carList = DataProvider.Instance.DB.XEs.ToList();
        public List<XE> CarList
        {
            get { return carList; }
            set { carList = value; }
        }

        private XE selectedItem;
        public XE SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ReceiptNumbericalOrder> list;
        public ObservableCollection<ReceiptNumbericalOrder> List
        {
            get { return list; }
            set { list = value; OnPropertyChanged(); }
        }
        public ICommand ShowCommand { get; set; }

        public ShowRepairReceptListViewModel()
        {
            ShowCommand = new RelayCommand<object>((p) => { return SelectedItem != null; }, (p) =>
            {
                var list = DataProvider.Instance.DB.PHIEUSUACHUAs.Where(x => x.BienSo == SelectedItem.BienSo).ToList();
                List = new ObservableCollection<ReceiptNumbericalOrder>();

                if (list == null)
                {
                    NotificationWindow.Notify("Biển số chưa lập phiếu sữa chữa!");
                    return;
                }    
                for (int i = 0; i < list.Count; i++)
                {
                    ReceiptNumbericalOrder item = new ReceiptNumbericalOrder()
                    {
                        Number = i + 1,
                        Phieu = list[i]
                    };
                    List.Add(item);
                }    
            });
        }
    }
}
