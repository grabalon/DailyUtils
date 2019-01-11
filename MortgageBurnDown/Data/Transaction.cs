using System;
using System.Runtime.Serialization;

namespace MortgageBurnDown
{
    [DataContract]
    public class Transaction : FinancialDataContractBase
    {
        public override void InitializeEvents()
        {
            base.InitializeEvents();
            
            if (_payment != null)
            {
                _payment.DataChanged += RaiseDataChanged;
            }
        }

        [DataMember]
        private Payment _payment;
        public Payment Payment
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new Payment();
                    _payment.DataChanged += RaiseDataChanged;
                }
                return _payment;
            }
            set
            {
                if (value != _payment)
                {
                    if (_payment != null)
                    {
                        _payment.DataChanged -= RaiseDataChanged;
                    }

                    _payment = value;

                    if (_payment != null)
                    {
                        _payment.DataChanged += RaiseDataChanged;
                    }

                    RaiseDataChanged();
                }
            }
        }

        [DataMember]
        private string _accountName;
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                if (value != _accountName)
                {
                    _accountName = value;
                    RaiseDataChanged();
                }
            }
        }

        [DataMember(IsRequired = false)]
        private string _allotmentName;
        public string AllotmentName
        {
            get
            {
                return _allotmentName;
            }
            set
            {
                if (value != _allotmentName)
                {
                    _allotmentName = value;
                    RaiseDataChanged();
                }
            }
        }
    }
}