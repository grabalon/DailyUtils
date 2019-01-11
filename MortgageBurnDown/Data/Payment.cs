using System;
using System.Runtime.Serialization;

namespace MortgageBurnDown
{
    [DataContract]
    public class Payment : FinancialDataContractBase
    {
        public Payment() : this(DateTime.Now, 0m)
        {
        }

        public Payment(DateTime date, decimal amount)
        {
            _date = date;
            _amount = amount;
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
    }
}