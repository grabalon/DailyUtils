using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    public class SavingsVM : INotifyPropertyChanged
    {
        private FinancialData _financialData;
        private bool _includeGazelle;

        private const string AccountValue = "AccountValue";
        private Dictionary<string, LineSeries> _seriesPlots = new Dictionary<string, LineSeries>();


        public PlotModel Model { get; private set; }

        public SavingsVM(FinancialData financialData)
        {
            _financialData = financialData;

            Model = new PlotModel();
            Model.Title = "Savings Over Time";

            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var dateAxis = new DateTimeAxis();
            dateAxis.Position = AxisPosition.Bottom;
            dateAxis.Minimum = DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(0));
            dateAxis.Maximum = DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(MortgageConstants.OriginalDurationInMonths));
            Model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom });

            InitSeries();

            _financialData.DataChanged += OnFinancialDataChanged;
            RefreshSavingsSeries();

        }

        private void InitSeries()
        {
            var accountValueSeries = new LineSeries();
            accountValueSeries.Title = "Account Value";
            _seriesPlots[AccountValue] = accountValueSeries;
            Model.Series.Add(accountValueSeries);
        }

        private void OnFinancialDataChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshSavingsSeries();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public bool IncludeGazelle
        {
            get
            {
                return _includeGazelle;
            }
            set
            {
                if (_includeGazelle != value)
                {
                    _includeGazelle = value;
                    RefreshSavingsSeries();
                    RaisePropertyChanged();
                }
            }
        }

        private void RefreshSavingsSeries()
        {
            foreach (var pair in _seriesPlots)
            {
                pair.Value.Points.Clear();
            }

            decimal accountValue = 0m;
            foreach (var account in _financialData.Accounts)
            {
                accountValue += account.Value;
            }

            // Look out ~10 years of days
            for (int i = 0; i < 3650; i++)
            {
                var day = DateTime.Today.AddDays(i);
                
                foreach (var transaction in _financialData.Transactions)
                {
                    if (transaction.Date == day)
                    {
                        accountValue += transaction.Amount;
                    }
                }

                _seriesPlots[AccountValue].Points.Add(new DataPoint(DateTimeAxis.ToDouble(day), (double)accountValue));
            }
        }
    }
}
