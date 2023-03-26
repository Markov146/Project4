using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    internal class budgetMoney
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Ammount { get; set; }
        public bool IsIncome { get; set; }
        public DateTime CurrentDate;

        public budgetMoney(DateTime entryDate, string entryName, string entryType, int entryMoney, bool isIncome)
        {
            CurrentDate = entryDate;
            Name = entryName;
            Type = entryType;
            Ammount = entryMoney;
            IsIncome = isIncome;
        }
    }
}
