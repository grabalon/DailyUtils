using System;
using System.Runtime.Serialization;

namespace QuarterlyFunding
{
    [DataContract]
    public class Goal : FinancialDataContractBase
    {
        [DataMember]
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaiseDataChanged();
                }
            }
        }

        [DataMember]
        private decimal _value;
        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    RaiseDataChanged();
                }
            }
        }

        [DataMember]
        private DateTime _deadline;
        public DateTime Deadline {
            get
            {
                return _deadline;
            }
            set
            {
                if (value != _deadline)
                {
                    _deadline = value;
                    RaiseDataChanged();
                }
            }
        }
    }
}