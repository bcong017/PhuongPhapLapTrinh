using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.ViewModel
{
    public class ShowAccountListViewModel : BaseViewModel
    {
        private ObservableCollection<AccountNumbericalOrder> accountList;
        public ObservableCollection<AccountNumbericalOrder> AccountList
        {
            get { return accountList; }
            set { accountList = value; }
        }
        public ShowAccountListViewModel()
        {
            AccountList = new ObservableCollection<AccountNumbericalOrder>();
            List<TAIKHOAN> TaiKhoan = DataProvider.Instance.DB.TAIKHOANs.Where(x => x.QuyenHan == 0).ToList();
            for (int i = 0; i < TaiKhoan.Count; i++)
            {
                AccountNumbericalOrder accountNumbericalOrder = new AccountNumbericalOrder();
                accountNumbericalOrder.Number = i + 1;
                accountNumbericalOrder.TaiKhoan = TaiKhoan[i];
                AccountList.Add(accountNumbericalOrder);
            }    
        }
    }
}
