using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    [Export(typeof(IMortgagePaydownSeries))]
    [ExportMetadata("Name", "Triple Series")]
    class DelaySeries : IMortgagePaydownSeries
    {
        public DateTime StartDate => DateTime.Now;

        public decimal StartValue => MortgageConstants.CurrentBalance;

        public decimal GetPayment(DateTime date)
        {
            return MortgageConstants.MinimumPayment * 3;
        }

        public event EventHandler PaymentsChanged;
    }

    [Export(typeof(IMortgagePaydownSeries))]
    [ExportMetadata("Name", "Double Series")]
    class DelaySeries2 : IMortgagePaydownSeries
    {
        public DateTime StartDate => DateTime.Now;

        public decimal StartValue => MortgageConstants.CurrentBalance;

        public decimal GetPayment(DateTime date)
        {
            return MortgageConstants.MinimumPayment * 2;
        }

        public event EventHandler PaymentsChanged;
    }
}
