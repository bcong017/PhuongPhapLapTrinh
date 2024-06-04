using QuanLyGaraOto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyGaraOto.ViewUserControl.ViewUC
{
    /// <summary>
    /// Interaction logic for ShowAccessoriesListUC.xaml
    /// </summary>
    public partial class ShowAccessoriesListUC : UserControl
    {
        public ShowAccessoriesListUC()
        {
            InitializeComponent();
            this.DataContext = new ShowAccessoriesViewModel();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
