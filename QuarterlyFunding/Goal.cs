using System;
using System.Runtime.Serialization;

namespace QuarterlyFunding
{
    [DataContract]
    internal class Goal
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public DateTime Deadline { get; set; }
    }
}