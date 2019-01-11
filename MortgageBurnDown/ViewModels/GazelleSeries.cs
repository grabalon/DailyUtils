using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    public class GazelleSeries : IMortgagePaydownSeries
    {
        private static GazelleSeries _instance;

        [Export(typeof(IMortgagePaydownSeries))]
        [ExportMetadata("Name", "Gazelle Series")]
        public static GazelleSeries Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GazelleSeries();
                }
                return _instance;
            }
        }

        public DateTime StartDate => DateTime.Now;

        public decimal StartValue => MortgageConstants.CurrentBalance;

        public decimal GetPayment(DateTime date, decimal balance)
        {
            if (balance < 25000)
            {
                return balance;
            }

            var partialPaymentAmount = MortgageConstants.MinimumPayment;

            for (int i = 0; i < ExtraPayments.Count; i++)
            {
                var payment = ExtraPayments[i];
                if (payment.Date.Month == date.Month && payment.Date.Year == date.Year)
                {
                    partialPaymentAmount += payment.Amount;
                }
            }

            return partialPaymentAmount;
        }

        public readonly ObservableCollection<Payment> ExtraPayments = new ObservableCollection<Payment>();

        public event EventHandler PaymentsChanged;

        private GazelleSeries()
        {
            ExtraPayments.CollectionChanged += (o, s) =>
            {
                PaymentsChanged?.Invoke(o, s);
            };
        }
    }
}
