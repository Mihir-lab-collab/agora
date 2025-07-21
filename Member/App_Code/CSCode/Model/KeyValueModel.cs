using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Customer.Model
{
    /// <summary>
    /// Summary description for KeyValueModel
    /// </summary>
    [DataContract]
    public class KeyValueModel
    {
        [DataMember]
        public string Key { get; set; }
        
        [DataMember]
        public int Value { get; set; }
    }
}