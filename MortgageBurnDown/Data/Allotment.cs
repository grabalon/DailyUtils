using System;
using System.Runtime.Serialization;

namespace MortgageBurnDown
{
    [DataContract]
    public class Allotment : FinancialDataContractBase
    {
        [DataMember]
        private string _goalName;
        public string GoalName
        {
            get
            {
                return _goalName;
            }
            set
            {
                if (value != _goalName)
                {
                    _goalName = value;
                    RaiseDataChanged();
                }
            }
        }

        [DataMember]
        private decimal _value;
        public decimal Value {
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

        [DataMember(IsRequired = false)]
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