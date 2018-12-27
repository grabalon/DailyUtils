using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuarterlyFunding
{
    public class RootVM : INotifyPropertyChanged, ICommand
    {
        private readonly FinancialData _financialData = new FinancialData(@"FinancialData.json");

        public OverviewVM Overview
        {
            get
            {
                if (_overview == null)
                {
                    _overview = new OverviewVM(financialData: _financialData);
                }
                return _overview;
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public bool CanExecute(object parameter)
        {
            switch (parameter)
            {
                case "Add Account":
                    return true;
                case "Delete Account":
                    return SelectedAccount != null;
                default:
                    return true;
            }
        }

        public void Execute(object parameter)
        {
            switch (parameter)
            {
                case "Add Account":
                    AddAccount();
                    break;
                case "Delete Account":
                    DeleteAccount();
                    break;
            }
        }

        private void DeleteAccount()
        {
            if (SelectedAccount != null)
            {
                // Save the actual account
                var toRemove = SelectedAccount;

                // Clear the selected account to avoid WPF doing weird things with the selection
                SelectedAccount = null;

                // Actually remove the account from the list
                int selectedIndex = Accounts.IndexOf(toRemove);
                Accounts.RemoveAt(selectedIndex);

                // Reset the selected account if possible
                if (selectedIndex < Accounts.Count)
                {
                    SelectedAccount = Accounts[selectedIndex];
                }
                else if (selectedIndex > 0)
                {
                    SelectedAccount = Accounts[selectedIndex - 1];
                }
            }
        }

        private void AddAccount()
        {
            Account newAccount = new Account();
            newAccount.Name = "New Account";
            newAccount.Value = 0;
            Accounts.Add(newAccount);
            SelectedAccount = newAccount;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CanExecuteChanged;

        public ObservableCollection<Account> Accounts => _financialData.Accounts;
        public ObservableCollection<Goal> Goals => _financialData.Goals;
        public ObservableCollection<Transaction> Transactions => _financialData.Transactions;
        public ObservableCollection<Allotment> Allotments => _financialData.Allotments;

        private Account _selectedAccount;
        private OverviewVM _overview;

        public Account SelectedAccount
        {
            get
            {
                return _selectedAccount;
            }
            set
            {
                if (_selectedAccount != value)
                {
                    _selectedAccount = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    RaisePropertyChanged();
                }
            }
        }
    }
}
