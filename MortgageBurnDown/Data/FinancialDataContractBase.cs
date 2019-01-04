using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MortgageBurnDown
{
    public class FinancialDataContractBase : INotifyPropertyChanged
    {
        internal void RaiseDataChanged([CallerMemberName] string caller = null)
        {
            RaiseDataChanged(this, new PropertyChangedEventArgs(caller));
        }

        internal void RaiseDataChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        public event PropertyChangedEventHandler DataChanged
        {
            add
            {
                PropertyChanged += value;
            }
            remove
            {
                PropertyChanged -= value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}