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

            var payment = MortgageConstants.MinimumPayment;

            for (int i = 0; i < ExtraPayments.Count; i++)
            {
                var datePaymentPair = ExtraPayments[i];
                if (datePaymentPair.Key.Month == date.Month && datePaymentPair.Key.Year == date.Year)
                {
                    payment += datePaymentPair.Value;
                }
            }

            return payment;
        }

        public readonly ObservableCollection<KeyValuePair<DateTime, Decimal>> ExtraPayments = new ObservableCollection<KeyValuePair<DateTime, decimal>>();

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
