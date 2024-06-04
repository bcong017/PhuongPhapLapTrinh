using QuanLyGaraOto.Model;
using QuanLyGaraOto.AddingClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class SalesReportViewModel : BaseViewModel
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

        private Decimal? totalMoney;
        public Decimal? TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SalesNumbericalOrder> reportList;
        public ObservableCollection<SalesNumbericalOrder> ReportList
        {
            get { return reportList; }
            set { reportList = value; OnPropertyChanged(); }
        }

        public ICommand MakeReportCommand { get; set; }

        public SalesReportViewModel()
        {
            TotalMoney = 0;
            MakeReportCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null || string.IsNullOrEmpty(Year) || Int32.TryParse(Year, out _) == false)
                    return false;
                return true;
            }, (p) =>
            {
                ReportList = new ObservableCollection<SalesNumbericalOrder>();
                int reportedYear = Convert.ToInt32(Year);
                
                var saleReport = DataProvider.Instance.DB.BAOCAODOANHSOes.Where(x => x.ThoiGian.Value.Year == reportedYear && x.ThoiGian.Value.Month == number).FirstOrDefault();
                if (saleReport == null)
                {
                    NotificationWindow.Notify("Không có báo cáo doanh số của tháng tương ứng!");
                    return;
                }

                var list = DataProvider.Instance.DB.CT_BCDS.Where(x => x.MaBCDS == saleReport.MaBCDS).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    SalesNumbericalOrder salesNumbericalOrder = new SalesNumbericalOrder();
                    salesNumbericalOrder.Number = i + 1;
                    salesNumbericalOrder.SalesDetail = list[i];
                    salesNumbericalOrder.SalesDetail.TiLe = (saleReport.TongDoanhThu != 0 ? (Math.Round((double)salesNumbericalOrder.SalesDetail.ThanhTien / (double)saleReport.TongDoanhThu * 100)) : 0).ToString() + "%";
                    ReportList.Add(salesNumbericalOrder);
                    if (saleReport.ThoiGian < new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1) )
                    {
                        list[i].TiLe = (saleReport.TongDoanhThu != 0 ? (Math.Round((double)salesNumbericalOrder.SalesDetail.ThanhTien / (double)saleReport.TongDoanhThu * 100)) : 0).ToString() + "%";
                    }
                }
                TotalMoney = saleReport.TongDoanhThu ?? 0;
                DataProvider.Instance.DB.SaveChanges();
            });
        }
    }

}
