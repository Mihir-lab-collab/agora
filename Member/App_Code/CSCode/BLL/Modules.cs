using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Customer.BLL
{
    /// <summary>
    /// Summary description for CustUsers
    /// </summary>
    public class Modules
    {
        public int ModuleID { get; set; }
        public int ModuleID_Parent { get; set; }
      //  public int ParentID { get; set; }
        public int TypeID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Menu { get; set; }
        public string EntryPage { get; set; }
        public string Parameter { get; set; }
        public bool IsMenuVisible { get; set; }
        public bool IsGenric { get; set; }
        public int SecurityLevelView { get; set; }
        public int SecurityLevelAdd { get; set; }
        public int SecurityLevelUpdate { get; set; }
        public int SortOrder { get; set; }
        public DateTime InsertedOn { get; set; }
        public int InsertedBy { get; set; }
        public string InsertedIP { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedIP { get; set; }
        public bool isModule { get; set; }
        public string ModuleID_ParentName { get; set; }
        public string mode { get; set; }
        public string IpAddress { get; set; }
     

        public Modules()
        {

        }

        public Modules(int _ModuleID, int _ModuleID_Parent, string _Type, string _Name, string _Menu, string _EntryPage, bool _IsMenuVisible, bool _IsGenric,
            int _SecurityLevelView, int _SecurityLevelAdd, int _SecurityLevelUpdate, int _SortOrder, DateTime _InsertedOn, int _InsertedBy, string _InsertedIP,
            DateTime _ModifiedOn, int _ModifiedBy, string _ModifiedIP)
        {
            this.ModuleID = _ModuleID;
            this.ModuleID_Parent = _ModuleID_Parent;
            this.Type = _Type;
            this.Name = _Name;
            this.Menu = _Menu;
            this.EntryPage = _EntryPage;          
            this.IsMenuVisible = _IsMenuVisible;
            this.IsGenric = _IsGenric;
            this.SecurityLevelView = _SecurityLevelView;
            this.SecurityLevelAdd = _SecurityLevelAdd;
            this.SecurityLevelUpdate = _SecurityLevelUpdate;
            this.SortOrder = _SortOrder;
            this.InsertedOn = _InsertedOn;
            this.InsertedBy = _InsertedBy;
            this.InsertedIP = _InsertedIP;
            this.ModifiedOn = _ModifiedOn;
            this.ModifiedBy = _ModifiedBy;
            this.ModifiedIP = _ModifiedIP;

        }
        public Modules(int TypeID)
        {
            this.TypeID = TypeID;
        }

        //public static List<Modules> GetModulesCurUser(int UserId, int ProjectId)
        //{
        //    ModulesDAL objCustModules = new ModulesDAL();
        //    return objCustModules.GetModulesCurUser(UserId, ProjectId);
        //}

        //public static List<Modules> GetCustomerModules()
        //{
        //    ModulesDAL objCustModules = new ModulesDAL();
        //    return objCustModules.GetCustomerModules();

        //}

        //public static Modules GetModuleByModuleId(int ModuleId)
        //{
        //    ModulesDAL objCustModules = new ModulesDAL();
        //    return objCustModules.GetModuleByModuleId(ModuleId);
        //}
        public static List<Modules> GetModulesForProjects()
        {
            ModulesDAL objCustModules = new ModulesDAL();
            return objCustModules.GetModulesForProjects();
        }
        //new
        public static List<Modules> GetParentModules(string mode)
        {
            ModulesDAL objType = new ModulesDAL();
            return objType.GetParentModules(mode);
        }
        public static List<Modules> BindModules(string mode, string Type )
        {
            ModulesDAL objType = new ModulesDAL();
            return objType.BindModuleData(mode, Type,0);
        }
        public static List<Modules> BindModules(string mode,string Type, int? ModuleId)
        {
            ModulesDAL objType = new ModulesDAL();
            return objType.BindModuleData(mode, Type, ModuleId);
        }
        public static int InsertModule(Modules objModuledata)
        {
            ModulesDAL objIns = new ModulesDAL();
            return objIns.InsertModule(objModuledata);
        }
        public static bool UpdateModule(Modules objModuledata)
        {
            ModulesDAL objIns = new ModulesDAL();
            return objIns.UpdateModule(objModuledata);
        }     
    }
}