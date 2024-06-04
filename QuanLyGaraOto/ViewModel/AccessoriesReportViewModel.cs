using QuanLyGaraOto.Model;
using QuanLyGaraOto.AddingClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace QuanLyGaraOto.ViewModel
{
    public class AccessoriesReportViewModel : BaseViewModel
    {
        private MonthList months = new MonthList();
        public MonthList Months
        {
            get { return months; }
            set { months = value; }
        }

        private Month selectedItem;
        public Month SelectedItem
        {
            get { return selectedItem; }
            set 
            {
                selectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem)); 
                if (SelectedItem != null)
                    Number = SelectedItem.Number; 
            }
        }

        private int number;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        private string year;
        public string Year
        { 
            get { return year; }
            set { year = value; }
        }

        private ObservableCollection<InventoryNumbericalOrder> reportList;
        public ObservableCollection<InventoryNumbericalOrder> ReportList
        {
            get { return reportList; }
            set { reportList = value; OnPropertyChanged(); }
        }

        public ICommand MakeReportCommand { get; set; }

        public AccessoriesReportViewModel()
        {
            MakeReportCommand = new RelayCommand<object>((p) => 
            {
                if (SelectedItem == null || string.IsNullOrEmpty(Year))
                    return false;
                return true;
            }, (p) => 
            {
                ReportList = new ObservableCollection<InventoryNumbericalOrder>();
                int reportedYear = Convert.ToInt32(Year);
                var list = DataProvider.Instance.DB.BAOCAOTONs.Where(x => x.ThoiGian.Value.Year == reportedYear && x.ThoiGian.Value.Month == number).ToList();
                if (list.Count == 0)
                {
                    NotificationWindow.Notify("Không có báo cáo tồn của tháng!");
                    return;
                }    
                for (int i = 0; i < list.Count; i++)
                {
                    InventoryNumbericalOrder inventoryNumbericalOrder = new InventoryNumbericalOrder();
                    inventoryNumbericalOrder.Number = i + 1;
                    inventoryNumbericalOrder.BaoCaoTon = list[i];
                    ReportList.Add(inventoryNumbericalOrder);
                }    
            });
        }
    }
}
