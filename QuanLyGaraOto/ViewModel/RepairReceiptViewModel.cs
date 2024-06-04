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
using QuanLyGaraOto.Facade;

namespace QuanLyGaraOto.ViewModel
{
    public class RepairReceiptViewModel : BaseViewModel
    {
        private int itemId = 1;
        private List<XE> carList;
        public List<XE> CarList
        {
            get { return carList; }
            set { carList = value; OnPropertyChanged(); }
        }

        private List<TIENCONG> wagesList;
        public List<TIENCONG> WagesList
        {
            get { return wagesList; }
            set { wagesList = value; OnPropertyChanged(); }
        }

        private List<VATTU> itemList;
        public List<VATTU> ItemList
        {
            get { return itemList; }
            set { itemList = value; OnPropertyChanged(); }
        }

        private XE selectedCar;
        public XE SelectedCar
        {
            get { return selectedCar; }
            set { selectedCar = value; OnPropertyChanged(); }
        }

        private Decimal totalMoney;
        public Decimal TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; OnPropertyChanged(); }
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }

        private string times;
        public string Times
        {
            get { return times; }
            set { times = value; OnPropertyChanged(); }
        }

        private TIENCONG selectedWage;
        public TIENCONG SelectedWage
        {
            get { return selectedWage; }
            set { selectedWage = value; OnPropertyChanged(); }
        }

        private VATTU selectedItem;
        public VATTU SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }
        private string amount;
        public string Amount
        {
            get { return amount; }
            set { amount = value; OnPropertyChanged(); }
        }

        private ContentNumbericalOrder selectedContent;
        public ContentNumbericalOrder SelectedContent
        {
            get { return selectedContent; }
            set
            {
                selectedContent = value;
                OnPropertyChanged();
                if (SelectedContent != null)
                {
                    Content = SelectedContent.NoiDung;
                    Times = SelectedContent.SoLan.ToString();
                    SelectedWage = DataProvider.Instance.DB.TIENCONGs.Single(x => x.MaTienCong == SelectedContent.MaTienCong);
                    AccessoriesList = new ObservableCollection<ItemNumbericalOrder>(SelectedContent.ItemList);
                }

            }
        }

        private ItemNumbericalOrder selectedAccessories;
        public ItemNumbericalOrder SelectedAccessories
        {
            get { return selectedAccessories; }
            set
            {
                selectedAccessories = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ContentNumbericalOrder> contentList;
        public ObservableCollection<ContentNumbericalOrder> ContentList
        {
            get { return contentList; }
            set { contentList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ItemNumbericalOrder> accessoriesList;
        public ObservableCollection<ItemNumbericalOrder> AccessoriesList
        {
            get { return accessoriesList; }
            set { accessoriesList = value; OnPropertyChanged(); }
        }

        private List<List<ItemNumbericalOrder>> list = new List<List<ItemNumbericalOrder>>();

        public ICommand AddContentCommand { get; set; }
        public ICommand EditContentCommand { get; set; }
        public ICommand DeleteContentCommand { get; set; }
        public ICommand DeSelectedContentCommand { get; set; }
        public ICommand AddAccessoriesCommand { get; set; }
        public ICommand DeleteAccessoriesCommand { get; set; }
        public ICommand MakeReceiptCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ShowReceiptRecordCommand { get; set; }

        public RepairReceiptViewModel()
        {
            Load();

            AddContentCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Content) || string.IsNullOrEmpty(Times) || SelectedWage == null || SelectedContent != null)
                    return false;
                if (AccessoriesList.Count == 0)
                    return false;
                if (CheckContentExist() == false)
                    return false;
                return true;
            }, (p) =>
            {
                int repeatTimes = Convert.ToInt32(Times);
                ContentNumbericalOrder content = new ContentNumbericalOrder()
                {
                    Number = ContentNumbericalOrder.orderNumber,
                    NoiDung = Content.Trim(),
                    SoLan = repeatTimes,
                    TenTienCong = SelectedWage.TenTienCong,
                    MaTienCong = SelectedWage.MaTienCong,
                    ItemList = new List<ItemNumbericalOrder>(AccessoriesList)
                };
                content.ThanhTien = CalculateContentMoney(content, SelectedWage);
                ContentList.Add(content);
                Content = Times = ""; SelectedWage = null;
                AccessoriesList.Clear();
                ContentNumbericalOrder.orderNumber++;
                itemId = 1;

                UpdateTotalMoney();
            });

            EditContentCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedContent == null)
                    return false;
                if (string.IsNullOrEmpty(Content) || SelectedWage == null)
                    return false;
                if (CheckContentExist() == false)
                    return false;
                return true;
            }, (p) =>
            {
                foreach (var content in ContentList)
                {
                    if (content == SelectedContent)
                    {
                        content.NoiDung = Content.Trim();
                        content.SoLan = Convert.ToInt32(Times);
                        content.TenTienCong = SelectedWage.TenTienCong;
                        content.MaTienCong = SelectedWage.MaTienCong;
                        content.ThanhTien = CalculateContentMoney(content, SelectedWage);
                        SelectedContent = content;
                        break;
                    }
                }
                Content = Times = ""; SelectedWage = null; SelectedContent = null;

                UpdateTotalMoney();
            });

            DeleteContentCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedContent == null)
                    return false;
                return true;
            }, (p) =>
            {
                int i = 1;
                foreach (var content in ContentList)
                {
                    if (content == SelectedContent)
                    {
                        ContentList.Remove(content);
                        break;
                    }
                }
                ContentNumbericalOrder.orderNumber--;

                foreach (var content in ContentList)
                {
                    content.Number = i;
                    i++;
                }
                Content = Times = ""; SelectedWage = null; SelectedContent = null; AccessoriesList.Clear();

                UpdateTotalMoney();
            });

            DeSelectedContentCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedContent == null)
                    return false;
                return true;
            }, (p) =>
            {
                Content = Times = ""; SelectedWage = null; SelectedContent = null; AccessoriesList.Clear();
            });

            AddAccessoriesCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null || string.IsNullOrEmpty(Amount))
                    return false;
                if (CheckItemExist() == false)
                    return false;
                if (SelectedContent != null)
                    return false;

                return true;
            }, (p) =>
            {
                int quantity = Convert.ToInt32(Amount);
                if (quantity > SelectedItem.SoLuongTon)
                {
                    NotificationWindow.Notify(String.Format("Số vật tư bạn đã chọn không còn đủ (Hiện có: {0})!", SelectedItem.SoLuongTon));
                    return;
                }
                ItemNumbericalOrder item = new ItemNumbericalOrder()
                {
                    Number = itemId,
                    TenVatTu = SelectedItem.TenVatTu,
                    MaVatTu = SelectedItem.MaVatTu,
                    SoLuong = quantity,
                    ThanhTien = (SelectedItem.DonGiaHienTai * quantity) ?? 0
                };
                itemId++;
                AccessoriesList.Add(item);
                SelectedItem = null; Amount = "";

            });
            DeleteAccessoriesCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedAccessories == null)
                    return false;
                if (SelectedContent != null)
                    return false;
                return true;
            }, (p) =>
            {
                AccessoriesList.Remove(SelectedAccessories);
                itemId--;
                int i = 1;
                foreach (var item in AccessoriesList)
                {
                    item.Number = i;
                    i++;
                }
                SelectedAccessories = null; Amount = "";
                UpdateTotalMoney();
            });

            MakeReceiptCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedCar == null || SelectedDate == null || TotalMoney == 0)
                    return false;
                return true;
            }, (p) =>
            {
                MakeRepairReceiptFacade facade = new MakeRepairReceiptFacade();
                facade.Make(SelectedCar, SelectedDate, TotalMoney, ContentList, list);               

            });

            RefreshCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SelectedDate = DateTime.Now;
                AccessoriesList = new ObservableCollection<ItemNumbericalOrder>();
                ContentList = new ObservableCollection<ContentNumbericalOrder>();
                TotalMoney = 0;
                SelectedCar = null;
                SelectedContent = null;
                SelectedItem = null;
                Amount = Times = "";
            });

            ShowReceiptRecordCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ShowRepairReceptListWindow showInputRecordWindow = new ShowRepairReceptListWindow(); showInputRecordWindow.ShowDialog(); });
        }

        private void Load()
        {
            SelectedDate = DateTime.Now;
            CarList = DataProvider.Instance.DB.XEs.ToList();
            WagesList = DataProvider.Instance.DB.TIENCONGs.ToList();
            ItemList = DataProvider.Instance.DB.VATTUs.ToList();
            ContentList = new ObservableCollection<ContentNumbericalOrder>();
            AccessoriesList = new ObservableCollection<ItemNumbericalOrder>();
        }

        private bool CheckContentExist()
        {
            foreach (var content in ContentList)
            {
                if (content != SelectedContent)
                {
                    if (string.Compare(content.NoiDung, Content) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool CheckItemExist()
        {
            foreach (var item in AccessoriesList)
            {
                if (string.Compare(item.TenVatTu, SelectedItem.TenVatTu) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private void UpdateTotalMoney()
        {
            TotalMoney = 0;

            foreach (var content in ContentList)
            {
                TotalMoney += content.ThanhTien;
            }
        }

        private Decimal CalculateContentMoney(ContentNumbericalOrder content, TIENCONG SelectedWage)
        {
            Decimal itemMoney = 0;
            foreach (var item in content.ItemList)
            {
                itemMoney += (item.ThanhTien * item.SoLuong);
            }


            return ((itemMoney + SelectedWage.GiaTienCong) * content.SoLan) ?? 0;
        }

        

    }
}
