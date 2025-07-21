using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using nsMobileAPI;



/// <summary>
/// Summary description for MobileUserMaster
/// </summary>
public class MobileUserMaster : UserMaster
{
    public string Token { get; set; }
    public MobileUserMaster()
    {
        
        //
        // TODO: Add constructor logic here
        //     
        
    }
    public string UploadedImage { get; set; }

    //public ResponseData ObjresponseData { get; set; }

    //public IDictionary<string, string> MobileAPIProps;
}
