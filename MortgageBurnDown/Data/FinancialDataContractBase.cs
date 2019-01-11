using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MortgageBurnDown
{
    public class FinancialDataContractBase : INotifyPropertyChanged
    {
        public virtual void InitializeEvents()
        {
        }

        internal void RaiseDataChanged([CallerMemberName] string caller = null)
        {
            RaiseDataChanged(this, new PropertyChangedEventArgs(caller));
        }

        internal void RaiseDataChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(sender, e);
            }

            var dataChanged = DataChanged;
            if (dataChanged != null)
            {
                dataChanged(sender, e);
            }
        }

        public event PropertyChangedEventHandler DataChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}