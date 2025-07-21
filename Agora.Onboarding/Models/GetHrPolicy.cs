using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agora.Onboarding.Models
{
    public class GetHrPolicy
    {
        public string FileName { get; set; }
        public int FileTypeId { get; set; }
        public string CustomFileName { get; set; }
        public string FileUploadPath { get; set; }
        public string DisplayFileURL { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}