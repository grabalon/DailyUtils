using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace QuarterlyFunding
{
    [DataContract]
    internal class FinancialDataContract : FinancialDataContractBase
    {
        public void Initialize()
        {
            InitializeAccounts();
        }
        [DataMember(Name = "Accounts", IsRequired = false, EmitDefaultValue = false)]
        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get
            {
                if (_accounts == null)
                {
                    _accounts = new ObservableCollection<Account>();
                    InitializeAccounts();
                }
                return _accounts;
            }
        }

        private void InitializeAccounts()
        {
            _accounts.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        var oldAccount = item as Account;
                        oldAccount.DataChanged -= RaiseDataChanged;
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        var newAccount = item as Account;
                        newAccount.DataChanged += RaiseDataChanged;
                    }
                }

                RaiseDataChanged(s, e);
            };
        }

        [DataMember(Name = "ProjectedTransactions", IsRequired = false, EmitDefaultValue = false)]
        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                {
                    _transactions = new ObservableCollection<Transaction>();
                    _transactions.CollectionChanged += (o, s) => RaiseDataChanged();
                }
                return _transactions;
            }
        }

        [DataMember(Name = "Goals", IsRequired = false, EmitDefaultValue = false)]
        private ObservableCollection<Goal> _goals;
        public ObservableCollection<Goal> Goals
        {
            get
            {
                if (_goals == null)
                {
                    _goals = new ObservableCollection<Goal>();
                    _goals.CollectionChanged += (o, s) => RaiseDataChanged();
                }
                return _goals;
            }
        }

        [DataMember(Name = "Allotments", IsRequired = false, EmitDefaultValue = false)]
        private ObservableCollection<Allotment> _allotments;
        public ObservableCollection<Allotment> Allotments
        {
            get
            {
                if (_allotments == null)
                {
                    _allotments = new ObservableCollection<Allotment>();
                    _allotments.CollectionChanged += (o, s) => RaiseDataChanged();
                }
                return _allotments;
            }
        }
    }
}