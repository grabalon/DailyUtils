using System;
using System.Runtime.Serialization;

namespace QuarterlyFunding
{
    [DataContract]
    internal class Transaction
    {
        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember(IsRequired = false)]
        public string GoalName { get; set; }
    }
}