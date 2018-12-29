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
        private Dictionary<string, LineSeries> _mefSeriesPlots = new Dictionary<string, LineSeries>();

        [ImportMany]
        private List<Lazy<IMortgagePaydownSeries, IMortgagePaydownSeriesMetadata>> _seriesPaymentData;

        public RootVM()
        {
            InitMef();
            CreateSeries();

            GazelleVM = new GazelleVM();

            Model = new PlotModel();
            Model.Title = "Mortgage";

            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var dateAxis = new DateTimeAxis();
            dateAxis.Position = AxisPosition.Bottom;
            dateAxis.Minimum = DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(0));
            dateAxis.Maximum = DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(MortgageConstants.OriginalDurationInMonths));
            Model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom });

            foreach (var seriesPair in _mefSeriesPlots)
            {
                Model.Series.Add(seriesPair.Value);
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

        private void CreateSeries()
        {
            foreach (var seriesLazy in _seriesPaymentData)
            {
                var lineSeries = new LineSeries();
                lineSeries.Title = seriesLazy.Metadata.Name;
                var series = seriesLazy.Value;

                PopulateSeriesData(lineSeries, series);

                series.PaymentsChanged += (o, e) =>
                {
                    lineSeries.Points.Clear();
                    PopulateSeriesData(lineSeries, series);
                    Model.InvalidatePlot(updateData: true);
                };

                _mefSeriesPlots[lineSeries.Title] = lineSeries;
            }
        }

        private static void PopulateSeriesData(LineSeries lineSeries, IMortgagePaydownSeries series)
        {
            var balance = series.StartValue;
            var startDate = series.StartDate;

            int m;
            for (m = 0; balance > 0; m++)
            {
                var date = MortgageConstants.GetDateFromMonthOfMortgage(m);

                if (date >= startDate)
                {
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), (double)balance));
                    balance = HandleMonth(balance, series.GetPayment(date, balance));
                }
            }
            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(MortgageConstants.GetDateFromMonthOfMortgage(m)), 0));
        }

        public PlotModel Model { get; }

        public GazelleVM GazelleVM { get; }

        private static decimal HandleMonth(decimal balance, decimal payment)
        {
            if (balance == payment)
            {
                return 0;
            }

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
