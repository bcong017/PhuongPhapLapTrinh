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
    public class SearchViewModel : BaseViewModel
    {
        public List<string> Option { get; set; }
        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string inputedItem;
        public string InputedItem
        {
            get { return inputedItem; }
            set
            {
                inputedItem = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<CarNumbericalOrder> xeList;
        public ObservableCollection<CarNumbericalOrder> XeList
        {
            get { return xeList; }
            set { xeList = value; OnPropertyChanged(nameof(XeList)); }
        }
        public ICommand SearchCommand { get; set; }
        public SearchViewModel()
        {
            this.Option = new List<string>() { "Biển số", "Hiệu xe", "Chủ xe", "Tiền nợ" };
            SearchCommand = new RelayCommand<object>((p) => 
            { 
                if (Check() == false || String.IsNullOrEmpty(InputedItem) || String.IsNullOrEmpty(SelectedItem))
                    return false; 
                else return true; 
            }
            , (p) => 
            {
                XeList = new ObservableCollection<CarNumbericalOrder>();
                List<XE> list = null;
                switch (SelectedItem)
                {
                    case "Biển số":
                        list = SearchByLicensePlate();
                        break;
                    case "Hiệu xe":
                        list = SearchByCarBrand();
                        break;
                    case "Chủ xe":
                        list = SearchByOwner();
                        break;
                    case "Tiền nợ":
                        list = SearchByDebt();
                        break;
                }
                if (list.Count == 0)
                {
                    NotificationWindow.Notify("Không có đối tượng bạn đang tìm!");
                    return;
                }
                
                for (int i = 0; i < list.Count; i++) 
                {
                    CarNumbericalOrder carNumbericalOrder = new CarNumbericalOrder();
                    carNumbericalOrder.Number = i+1;
                    carNumbericalOrder.Car = list[i];
                    XeList.Add(carNumbericalOrder);
                }    
            }
            );
        }
        
        private bool Check()
        {
            if (SelectedItem == "Tiền nợ")
                return Decimal.TryParse(InputedItem, out _);
            
            return true;
        }
        private List<XE> SearchByLicensePlate()
        {
            List<XE> result = DataProvider.Instance.DB.XEs.Where(x=>x.BienSo == InputedItem ).ToList();
            return result;  
            
        }
        private List<XE> SearchByCarBrand()
        {
            List<XE> result = DataProvider.Instance.DB.XEs.Where(x => x.HIEUXE.TenHieuXe == InputedItem).ToList();
            return result;
        }
        private List<XE> SearchByOwner()
        {
            List<XE> result = DataProvider.Instance.DB.XEs.Where(x => x.TenChuXe == InputedItem).ToList();
            return result;

        }
        private List<XE> SearchByDebt()
        {
            decimal d = Convert.ToDecimal(InputedItem);
            List<XE> result = DataProvider.Instance.DB.XEs.Where(x => x.TienNo == d).ToList();
            return result;

        }
    }
}
