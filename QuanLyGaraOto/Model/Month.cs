using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGaraOto.Model
{
    public class MonthList
    {
        private List<Month> months;
        public List<Month> Months
        { 
            get { return months; } 
            set { months = value; }
        }

        public MonthList()
        {
            string[] string_of_months = { "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười", "Mười một", "Mười hai" };
            Months = new List<Month>();
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(new Month() { Name = string_of_months[i - 1], Number = i });
            }    
        }
    }

    public class Month
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
