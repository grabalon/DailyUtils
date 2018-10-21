using System;
using System.Runtime.Serialization;

namespace QuarterlyFunding
{
    [DataContract]
    public class Allotment : FinancialDataContractBase
    {
        [DataMember]
        public string GoalName { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime ProjectedDate;
    }
}