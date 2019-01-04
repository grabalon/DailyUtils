using System.Runtime.Serialization;

namespace MortgageBurnDown
{
    [DataContract]
    public class Account : FinancialDataContractBase
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
                if (_name != value)
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
                if (_value != value)
                {
                    _value = value;
                    RaiseDataChanged();
                }
            }
        }
    }
}