using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.Models
{
    public class ResponseData
    {
        public string Status = string.Empty;
        public string Message = string.Empty;
        public object Result { get; set; }
    }
}
