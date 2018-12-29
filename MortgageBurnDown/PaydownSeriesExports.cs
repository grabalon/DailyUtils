using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    [Export(typeof(IMortgagePaydownSeries))]
    [ExportMetadata("Name", "Original Series")]
    public class OriginalSeries : IMortgagePaydownSeries
    {
        public DateTime StartDate => MortgageConstants.OriginalDate;

        public decimal StartValue => MortgageConstants.OriginalBalance;

        public event EventHandler PaymentsChanged;

        public decimal GetPayment(DateTime date, decimal balance)
        {
            return MortgageConstants.MinimumPayment;
        }
    }

    [Export(typeof(IMortgagePaydownSeries))]
    [ExportMetadata("Name", "Current Series")]
    public class DelaySeries : IMortgagePaydownSeries
    {
        public DateTime StartDate => DateTime.Now;

        public decimal StartValue => MortgageConstants.CurrentBalance;

        public decimal GetPayment(DateTime date, decimal balance)
        {
            return MortgageConstants.MinimumPayment;
        }

        public event EventHandler PaymentsChanged;
    }
}
