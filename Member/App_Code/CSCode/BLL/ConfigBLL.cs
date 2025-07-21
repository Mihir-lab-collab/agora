using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;

public class ConfigBLL
{
    public int configID { get; set; }
    public string category { get; set; }
    public string name { get; set; }
    public string value { get; set; }
    public string value1 { get; set; }
    public string comment { get; set; }
    public DateTime modifiedOn { get; set; }
    public int modifiedBy { get; set; }
    public string mode { get; set; }
    public int insertedBy { get; set; }

    public ConfigBLL()
    {
    }

    public ConfigBLL(int ConfigurationID)
    {
        this.configID = ConfigurationID;
    }

    public ConfigBLL(int configID, string category, string name, string value, string value1, string comment, DateTime modifiedOn, int modifiedBy)
    {
        this.configID = configID;
        this.category = category;
        this.name = name;
        this.value = value;
        this.value1 = value1;
        this.comment = comment;
        this.modifiedOn = modifiedOn;
        this.modifiedBy = modifiedBy;
    }

    public static List<ConfigBLL> GetDefaultID(string mode)
    {
        ConfigDAL objConfig = new ConfigDAL();
        return objConfig.GetDefaultID (mode);
    }

    public static List<ConfigBLL> GetConfigDetails(string mode)
    {
        ConfigDAL objConfig = new ConfigDAL();
        return GetConfigDetails(mode, 0);
    }

    public static List<ConfigBLL> GetConfigDetails(string mode, int configID)
    {
        ConfigDAL objConfig = new ConfigDAL();
        return objConfig.GetConfigDetails(mode, configID);
    }

    public static int InsertConfig(string mode, int configID, string category, string name, string value, string value1, string comment, int insertedBy)
    {
        ConfigBLL objInsert = new ConfigBLL();
        objInsert.mode = mode;
        objInsert.configID = configID;
        objInsert.category = category;
        objInsert.name = name;
        objInsert.value = value;
        objInsert.value1 = value1;
        objInsert.comment = comment;
        objInsert.insertedBy = insertedBy;
        ConfigDAL objInsertInto = new ConfigDAL();
        return objInsertInto.InsertConfig(objInsert);
    }

    public static bool UpdateConfig(string mode, int configID, string category, string name, string value, string value1, string comment, int modifiedBy)
    {
        ConfigBLL objUpdate = new ConfigBLL();
        objUpdate.mode = mode;
        objUpdate.configID = configID;
        objUpdate.category = category;
        objUpdate.name = name;
        objUpdate.value = value;
        objUpdate.value1 = value1;
        objUpdate.comment = comment;
        objUpdate.modifiedBy = modifiedBy;
        ConfigDAL objUpdateInto = new ConfigDAL();
        return objUpdateInto.UpdateConfig(objUpdate);
    }
}