﻿using System;
using System.Runtime.Serialization;

namespace MortgageBurnDown
{
    [DataContract]
    public class Transaction : FinancialDataContractBase
    {
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

        [DataMember]
        private decimal _amount;
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    RaiseDataChanged();
                }
            }
        }

        [DataMember]
        private DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
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