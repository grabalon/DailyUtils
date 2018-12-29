using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    public interface IMortgagePaydownSeries
    {
        DateTime StartDate { get; }
        decimal StartValue { get; }
        decimal GetPayment(DateTime date, decimal balance);
        event EventHandler PaymentsChanged;
    }

    public interface IMortgagePaydownSeriesMetadata
    {
        string Name { get; }
    }
}
