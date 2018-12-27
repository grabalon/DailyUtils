using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace QuarterlyFunding
{
    public class FinancialData
    {
        private string _backingFile;
        private FinancialDataContract _contract;

        public FinancialData(string filename)
        {
            _backingFile = filename;
            if (File.Exists(_backingFile))
            {
                try
                {
                    // deserialize JSON directly from a file
                    using (StreamReader file = File.OpenText(_backingFile))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        _contract = (FinancialDataContract)serializer.Deserialize(file, typeof(FinancialDataContract));
                        _contract.Initialize();
                    }
                }
                catch
                {
                    Debug.Fail("Error deserializing financial data");
                }
            }

            if (_contract == null)
            {
                _contract = new FinancialDataContract();
            }

            _contract.DataChanged += OnContractDataChanged;
            OnContractDataChanged(this, EventArgs.Empty);
        }

        private void OnContractDataChanged(object sender, System.EventArgs e)
        {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(_backingFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, _contract);
            }
        }

        public ObservableCollection<Account> Accounts => _contract.Accounts;
        public ObservableCollection<Goal> Goals => _contract.Goals;
        public ObservableCollection<Transaction> Transactions => _contract.Transactions;
        public ObservableCollection<Allotment> Allotments => _contract.Allotments;
    }
}