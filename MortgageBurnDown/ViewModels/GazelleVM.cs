using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MortgageBurnDown
{
    public class GazelleVM : INotifyPropertyChanged, ICommand
    {
        private KeyValuePair<DateTime, decimal>? _selectedItem;
        private FinancialData _financialData;

        public GazelleVM(FinancialData financialData)
        {
            _financialData = financialData;
            _financialData.DataChanged += OnFinancialDataChanged;
            OnFinancialDataChanged(null, EventArgs.Empty);
        }

        private void OnFinancialDataChanged(object sender, EventArgs e)
        {
            ExtraPayments.Clear();

            var unallocatedBalances = new Dictionary<Account, Dictionary<DateTime, decimal>>();

            var now = DateTime.Now;
            var currentMonth = new DateTime(now.Year, now.Month, 1);
            int currentMonthIndex = 0;
            var endMonth = MortgageConstants.GetDateFromMonthOfMortgage(MortgageConstants.OriginalDurationInMonths);

            foreach (var account in _financialData.Accounts)
            {
                var accountBalances = new Dictionary<DateTime, decimal>();

                int month = 0;
                // Get to this month
                for (; MortgageConstants.GetDateFromMonthOfMortgage(month) < currentMonth; month++) ;

                currentMonthIndex = month;

                // For the rest of the term of the mortgage, get the current balance of the account
                for (; month < MortgageConstants.OriginalDurationInMonths; month++)
                {
                    var date = MortgageConstants.GetDateFromMonthOfMortgage(month);
                    accountBalances[date] = account.Value;

                    // Account for each transaction we know about
                    foreach (var transaction in _financialData.Transactions)
                    {
                        if (transaction.AccountName == account.Name && transaction.Date <= date)
                        {
                            accountBalances[date] += transaction.Amount;
                        }
                    }
                }
                unallocatedBalances[account] = accountBalances;
            }

            // Merge all accounts together, then deal with allotments for account-agnostic stuff
            var mergedBalances = new Dictionary<DateTime, decimal>();

            foreach (var account2 in unallocatedBalances.Keys)
            {
                for (int month = currentMonthIndex; month < MortgageConstants.OriginalDurationInMonths; month++)
                {
                    var date = MortgageConstants.GetDateFromMonthOfMortgage(month);

                    if (mergedBalances.ContainsKey(date))
                    {
                        mergedBalances[date] += unallocatedBalances[account2][date];
                    }
                    else
                    {
                        mergedBalances[date] = unallocatedBalances[account2][date];
                    }
                }
            }

            foreach (var allotment in _financialData.Allotments)
            {
                for (int month = currentMonthIndex; month < MortgageConstants.OriginalDurationInMonths; month++)
                {
                    var date = MortgageConstants.GetDateFromMonthOfMortgage(month);

                    if (allotment.Date <= date)
                    {
                        mergedBalances[date] += allotment.Value;
                    }
                }
            }

            var min = mergedBalances.Values.LastOrDefault();
            // Walk backwards through the dates and keep the minimum balances
            foreach (var date in mergedBalances.Keys.Reverse())
            {
                if (min > mergedBalances[date])
                {
                    min = mergedBalances[date];
                }
                else
                {
                    mergedBalances[date] = min;
                }
            }

            // Now walk forwards through the dates and make payments for every time the balance goes up
            var max = mergedBalances.Values.FirstOrDefault();
            ExtraPayments.Add(new KeyValuePair<DateTime, decimal>(currentMonth, max));
            foreach (var date in mergedBalances.Keys)
            {
                if (mergedBalances[date] > max)
                {
                    ExtraPayments.Add(new KeyValuePair<DateTime, decimal>(date, mergedBalances[date] - max));
                    max = mergedBalances[date];
                }
            }
        }

        public ObservableCollection<KeyValuePair<DateTime, decimal>> ExtraPayments
        {
            get
            {
                return GazelleSeries.Instance.ExtraPayments;
            }
        }

        public KeyValuePair<DateTime, decimal> SelectedItem
        {
            get
            {
                return _selectedItem.HasValue ? _selectedItem.Value : default(KeyValuePair<DateTime, decimal>);
            }
            set
            {
                _selectedItem = value;
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var tuple = parameter as Tuple<string, DateTime, decimal>;

            switch (tuple.Item1)
            {
                case "Add":
                    ExtraPayments.Add(new KeyValuePair<DateTime, decimal>(tuple.Item2, tuple.Item3));
                    break;
                case "Remove":
                    if (_selectedItem.HasValue)
                    {
                        ExtraPayments.Remove(_selectedItem.Value);
                    }
                    break;
            }
        }
    }
}