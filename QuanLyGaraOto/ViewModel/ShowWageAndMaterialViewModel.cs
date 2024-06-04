using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyGaraOto.Model;
using QuanLyGaraOto.ViewUserControl.SwitchViewClass;

namespace QuanLyGaraOto.ViewModel
{
    public class ShowWageAndMaterialViewModel : BaseViewModel
    {
        private BaseViewModel showListViewModel;
        public BaseViewModel ShowListViewModel
        {
            get { return showListViewModel; }
            set { showListViewModel = value; OnPropertyChanged(); }
        }
        public ICommand ShowListCommand { get; set; }
        public ShowWageAndMaterialViewModel()
        {
            ShowListViewModel = new SwitchViewShowAccessoriesList();
            ShowListCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                switch (p.Name)
                {
                    case "showAccessoriesBtn":
                        {
                            ShowListViewModel = new SwitchViewShowAccessoriesList();
                            break;
                        }
                    case "showWagesBtn":
                        {
                            ShowListViewModel = new SwitchViewShowWagesList();
                            break;
                        }
                    case "showCarBrandBtn":
                        {
                            ShowListViewModel = new SwitchViewShowCarBrandsList();
                            break;
                        }
                }
            });
        }
    }
}
