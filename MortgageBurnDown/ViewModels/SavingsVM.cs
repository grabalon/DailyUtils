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
        private const string AccountValue = "Account Value";
        private const string AllottedMoney = "Allotted Money";
        private GazelleSeries _gazelleSeries;
        private FinancialData _financialData;
        private bool _includeGazelle;
        private Dictionary<string, LineSeries> _seriesPlots = new Dictionary<string, LineSeries>();

        public PlotModel Model { get; private set; }

        public SavingsVM(FinancialData financialData)
        {
            _gazelleSeries = GazelleSeries.Instance;
            _financialData = financialData;

            Model = new PlotModel();
            Model.Title = "Savings Over Time";

            var valueAxis = new LinearAxis();
            valueAxis.Position = AxisPosition.Left;
            valueAxis.Minimum = 0;
            Model.Axes.Add(valueAxis);

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
            var accountValueSeries = new AreaSeries();
            accountValueSeries.Title = AccountValue;
            _seriesPlots[AccountValue] = accountValueSeries;
            Model.Series.Add(accountValueSeries);

            var allottedSeries = new AreaSeries();
            allottedSeries.Title = AllottedMoney;
            _seriesPlots[AllottedMoney] = allottedSeries;
            Model.Series.Add(allottedSeries);
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

            decimal allottedValue = 0m;
            foreach (var allotment in _financialData.Allotments)
            {
                if (allotment.Date == null)
                {
                    allottedValue += allotment.Value;
                }
            }

            // Look out ~10 years of days
            for (int i = 0; i < 3650; i++)
            {
                var day = DateTime.Today.AddDays(i);

                foreach (var transaction in _financialData.Transactions)
                {
                    if (transaction.Payment.Date == day)
                    {
                        accountValue += transaction.Payment.Amount;

                        if (!string.IsNullOrEmpty(transaction.AllotmentName))
                        {
                            allottedValue += transaction.Payment.Amount;
                        }
                    }
                }

                foreach (var allotment in _financialData.Allotments)
                {
                    if (allotment.Date == day)
                    {
                        allottedValue += allotment.Value;
                    }
                }


                if (!_includeGazelle)
                {
                    foreach (var extraPayment in _gazelleSeries.ExtraPayments)
                    {
                        if (extraPayment.Date == day)
                        {
                            accountValue -= extraPayment.Amount;
                        }
                    }
                }

                _seriesPlots[AccountValue].Points.Add(new DataPoint(DateTimeAxis.ToDouble(day), (double)accountValue));
                _seriesPlots[AllottedMoney].Points.Add(new DataPoint(DateTimeAxis.ToDouble(day), (double)allottedValue));
            }

            Model.InvalidatePlot(updateData: true);
        }
    }
}
