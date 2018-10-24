using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuarterlyFunding
{
    public class FinancialDataContractBase : INotifyPropertyChanged
    {
        internal void RaiseDataChanged([CallerMemberName] string caller = null)
        {
            RaiseDataChanged(this, new PropertyChangedEventArgs(caller));
        }

        internal void RaiseDataChanged(object sender, PropertyChangedEventArgs e)
        {
            DataChanged?.Invoke(sender, e);
            PropertyChanged?.Invoke(sender, e);
        }

        public event PropertyChangedEventHandler DataChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}