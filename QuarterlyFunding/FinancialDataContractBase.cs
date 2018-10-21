using System;

namespace QuarterlyFunding
{
    public class FinancialDataContractBase
    {
        internal void RaiseDataChanged()
        {
            RaiseDataChanged(this, EventArgs.Empty);
        }

        internal void RaiseDataChanged(object sender, EventArgs e)
        {
            DataChanged?.Invoke(sender, e);
        }

        public event EventHandler DataChanged;
    }
}