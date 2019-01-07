using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MortgageBurnDown
{
    [DataContract]
    internal class FinancialDataContract : FinancialDataContractBase
    {
        public void Initialize()
        {
            if (_accounts != null)
                ConnectCollectionEvents(_accounts, nameof(Accounts));
            if (_transactions != null)
                ConnectCollectionEvents(_transactions, nameof(Transactions));
            if (_goals != null)
                ConnectCollectionEvents(_goals, nameof(Goals));
            if (_allotments != null)
                ConnectCollectionEvents(_allotments, nameof(Allotments));
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
                    ConnectCollectionEvents(_accounts, nameof(Accounts));
                }
                return _accounts;
            }
        }

        private void ConnectCollectionEvents<T>(ObservableCollection<T> collection, string propertyName) where T : FinancialDataContractBase
        {
            foreach (FinancialDataContractBase item in collection)
            {
                item.DataChanged += RaiseDataChanged;
            }

            collection.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        (item as FinancialDataContractBase).DataChanged -= RaiseDataChanged;
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        (item as FinancialDataContractBase).DataChanged += RaiseDataChanged;
                    }
                }

                RaiseDataChanged(s, new PropertyChangedEventArgs(propertyName));
            };
        }

        [DataMember(Name = "Transactions", IsRequired = false, EmitDefaultValue = false)]
        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                {
                    _transactions = new ObservableCollection<Transaction>();
                    ConnectCollectionEvents(_transactions, nameof(Transactions));
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
                    ConnectCollectionEvents(_goals, nameof(Goals));
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
                    ConnectCollectionEvents(_allotments, nameof(Allotments));
                }
                return _allotments;
            }
        }
    }
}