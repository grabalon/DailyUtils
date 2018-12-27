using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    [Export]
    public class RootVM : INotifyPropertyChanged
    {
        private static readonly Dictionary<DateTime, decimal> _gazelleExtras = new Dictionary<DateTime, decimal>();

        [ImportMany]
        private List<Lazy<IMortgagePaydownSeries, IMortgagePaydownSeriesMetadata>> _seriesPaymentData;

        private Dictionary<string, LineSeries> _mefSeriesPlots = new Dictionary<string, LineSeries>();

        public RootVM()
        {
            InitMef();
            InitGazelle();
            CreateSeries();

            _model = new PlotModel();
            _model.Title = "Mortgage";

            _model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var dateAxis = new DateTimeAxis();
            dateAxis.Position = AxisPosition.Bottom;
            dateAxis.Minimum = DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(0));
            dateAxis.Maximum = DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(MortgageConstants.OriginalDurationInMonths));
            _model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom });
            _model.Series.Add(_originalSeries);
            _model.Series.Add(_currentSeries);
            _model.Series.Add(_gazelleSeries);

            foreach (var seriesPair in _mefSeriesPlots)
            {
                _model.Series.Add(seriesPair.Value);
            }
        }

        private void InitMef()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(RootVM).Assembly));
            var container = new CompositionContainer(catalog);
            {
                container.ComposeParts(this);
            }
        }

        private void InitGazelle()
        {
            _gazelleExtras[DateTime.Parse("April 2019")] = 20000;
            _gazelleExtras[DateTime.Parse("April 2020")] = 20000;
            _gazelleExtras[DateTime.Parse("April 2021")] = 20000;
            _gazelleExtras[DateTime.Parse("April 2022")] = 20000;
            _gazelleExtras[DateTime.Parse("April 2023")] = 20000;
        }



        private void CreateSeries()
        {
            foreach (var seriesLazy in _seriesPaymentData)
            {
                var lineSeries = new LineSeries();
                lineSeries.Title = seriesLazy.Metadata.Name;

                var seriesValue = seriesLazy.Value;
                var balance = seriesValue.StartValue;
                var startDate = seriesValue.StartDate;

                int m;
                for (m = 0; balance > 0; m++)
                {
                    var date = MortgageConstants.GetDateFromMonthOfMortgage(m);

                    if (date >= startDate)
                    {
                        lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), (double)balance));
                        balance = HandleMonth(balance, seriesValue.GetPayment(date));
                    }
                }
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(m)), 0));
                _mefSeriesPlots[lineSeries.Title] = lineSeries;
            }

            _originalSeries = new LineSeries();
            _originalSeries.Title = "Original";

            _currentSeries = new LineSeries();
            _currentSeries.Title = "Current";

            _gazelleSeries = new LineSeries();
            _gazelleSeries.Title = "Gazelle";

            var balanceO = MortgageConstants.OriginalBalance;
            var balanceC = MortgageConstants.CurrentBalance;
            var balanceG = balanceC;

            bool currentZero = false;
            bool gazelleZero = false;

            int month;
            for (month = 0; balanceO > 0; month++)
            {
                var date = MortgageConstants.GetDateFromMonthOfMortgage(month);
                var now = DateTime.Now;
                var thisMonth = new DateTime(now.Year, now.Month, 1);

                _originalSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), (double)balanceO));
                balanceO = HandleMonth(balanceO, MortgageConstants.MinimumPayment);

                if (date >= thisMonth && !currentZero)
                {
                    if (balanceC <= 0)
                    {
                        currentZero = true;
                    }

                    _currentSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), (double)balanceC));
                    balanceC = HandleMonth(balanceC, MortgageConstants.MinimumPayment);

                    if (!gazelleZero)
                    {
                        if (balanceG <= 0)
                        {
                            gazelleZero = true;
                        }

                        _gazelleSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), (double)balanceG));

                        var gazellePayment = MortgageConstants.MinimumPayment;
                        if (_gazelleExtras.ContainsKey(date))
                        {
                            gazellePayment += _gazelleExtras[date];
                        }

                        balanceG = HandleMonth(balanceG, gazellePayment);

                        if (balanceG < 20000)
                        {
                            balanceG = 0;
                        }
                    }
                }
            }

            _originalSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(month)), 0));
        }

        private PlotModel _model;
        private LineSeries _originalSeries;
        private LineSeries _currentSeries;
        private LineSeries _gazelleSeries;

        public PlotModel Model
        {
            get
            {
                return _model;
            }
        }

        private static decimal HandleMonth(decimal balance, decimal payment)
        {
            decimal interest = Math.Round(MortgageConstants.Interest / 12 * balance, 2);
            return Math.Max(balance + interest - payment, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
