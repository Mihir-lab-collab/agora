using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Customer.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Customer.DAL
{
    /// <summary>
    /// Summary description for CustModulesDAL
    /// </summary>
    /// 
    public class ModulesDAL
    {
        private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public static string IPAddress
        {
            get { return Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]); }
        }
        public ModulesDAL()
        {

        }
        public List<Modules> GetModulesForProjects()
        {
            List<Modules> curmodules = new List<Modules>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_GetModulesForProjects", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = null;
            using (con)
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curmodules.Add(new Modules(
                        Convert.ToInt32(reader["ModuleID"]),
                        reader["ModuleID_Parent"].ToString() == "" ? 0 : Convert.ToInt32(reader["ModuleID_Parent"]),
                        reader["Type"].ToString(),
                        reader["Name"].ToString(),
                        reader["Menu"].ToString(),
                        reader["EntryPage"].ToString(),
                        Convert.ToBoolean(reader["IsMenuVisible"].ToString()),
                         Convert.ToBoolean(reader["IsGenric"].ToString()),
                        Convert.ToInt32(reader["SecurityLevelView"].ToString()),
                        Convert.ToInt32(reader["SecurityLevelAdd"].ToString()),
                        Convert.ToInt32(reader["SecurityLevelUpdate"].ToString()),
                        Convert.ToInt32(reader["SortOrder"].ToString()),
                        reader["InsertedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["InsertedOn"].ToString()),
                        Convert.ToInt32(reader["InsertedBy"].ToString()),
                        reader["InsertedIP"].ToString(),
                        reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                        reader["ModifiedBy"].ToString() == "" ? 0 : Convert.ToInt32(reader["ModifiedBy"]),
                        reader["ModifiedIP"].ToString()
                        ));
                }
            }
            return curmodules;
        }

        //new
        public List<Modules> BindModuleData(string mode, string Type, int? ModuleID)
        {
            List<Modules> curmodules = new List<Modules>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Module", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@ModuleID", ModuleID);
            SqlDataReader Dr = null;
            using (con)
            {
                con.Open();
                Dr = cmd.ExecuteReader();
                while (Dr.Read())
                {
                    Modules obj = new Modules();
                    obj.ModuleID = Convert.ToInt32(Dr["ModuleID"]);
                    obj.ModuleID_ParentName = Convert.ToString(Dr["Module_ParentName"]);
                    obj.ModuleID_Parent = Convert.ToInt32(Dr["ParentId"]);
                    obj.Name = Convert.ToString(Dr["Name"]);
                    obj.Menu = Convert.ToString(Dr["Menu"]);
                    obj.EntryPage = Convert.ToString(Dr["EntryPage"]);
                    obj.Parameter = Convert.ToString(Dr["Param"]);
                    obj.IsGenric = Convert.ToBoolean(Dr["IsGenric"]);
                    obj.IsMenuVisible = Convert.ToBoolean(Dr["IsMenuVisible"]);
                    obj.SortOrder = Convert.ToInt32(Dr["SortOrder"]);
                    curmodules.Add(obj);
                }
            }
            return curmodules;

        }
        public List<Modules> GetParentModules(string mode)
        {
            List<Modules> curmodules = new List<Modules>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Module", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            SqlDataReader Dr = null;
            using (con)
            {
                con.Open();
                Dr = cmd.ExecuteReader();
                while (Dr.Read())
                {
                    Modules obj = new Modules();
                    obj.ModuleID_ParentName = Convert.ToString(Dr["Module_ParentName"]);
                    obj.ModuleID_Parent = Convert.ToInt32(Dr["ParentId"]);
                    curmodules.Add(obj);
                }
            }
            return curmodules;
        }
        public int InsertModule(Modules objInsert)
        {
            int outputid = 0;
            SqlConnection con = new SqlConnection(_strConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_Module", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", objInsert.mode);
            cmd.Parameters.AddWithValue("@ModuleID", objInsert.ModuleID);
            cmd.Parameters.AddWithValue("@ModuleParentID", objInsert.ModuleID_Parent);
            cmd.Parameters.AddWithValue("@Type", objInsert.Type);
            cmd.Parameters.AddWithValue("@Name", objInsert.Name);
            cmd.Parameters.AddWithValue("@Menu", objInsert.Menu);
            cmd.Parameters.AddWithValue("@PageURL", objInsert.EntryPage);
            cmd.Parameters.AddWithValue("@Param", objInsert.Parameter);
            cmd.Parameters.AddWithValue("@IsMenuVisible", objInsert.IsMenuVisible);
            cmd.Parameters.AddWithValue("@IsGenric", objInsert.IsGenric);
            cmd.Parameters.AddWithValue("@SortOrder", objInsert.SortOrder);
            cmd.Parameters.AddWithValue("@IpAddress", IPAddress);
            cmd.Parameters.AddWithValue("@UserID", objInsert.InsertedBy);

            try
            {
                using (con)
                {
                    outputid = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }

            }
            catch (Exception ex)
            { }
            return outputid;
        }
        public bool UpdateModule(Modules objUpdate)
        {
            bool updated = false;
            SqlConnection con = new SqlConnection(_strConnection);
            SqlCommand cmd = new SqlCommand("SP_Module", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", objUpdate.mode);
            cmd.Parameters.AddWithValue("@ModuleID", objUpdate.ModuleID);
            cmd.Parameters.AddWithValue("@ModuleParentID", objUpdate.ModuleID_Parent);
            cmd.Parameters.AddWithValue("@Type", objUpdate.Type);
            cmd.Parameters.AddWithValue("@Name", objUpdate.Name);
            cmd.Parameters.AddWithValue("@Menu", objUpdate.Menu);
            cmd.Parameters.AddWithValue("@PageURL", objUpdate.EntryPage);
            cmd.Parameters.AddWithValue("@Param", objUpdate.Parameter);
            cmd.Parameters.AddWithValue("@IsMenuVisible", objUpdate.IsMenuVisible);
            cmd.Parameters.AddWithValue("@IsGenric", objUpdate.IsGenric);
            cmd.Parameters.AddWithValue("@SortOrder", objUpdate.SortOrder);
            cmd.Parameters.AddWithValue("@IpAddress", IPAddress);
            cmd.Parameters.AddWithValue("@UserID", objUpdate.ModifiedBy);

            try
            {
                using (con)
                {
                    con.Open();
                    int output = Convert.ToInt32(cmd.ExecuteScalar());
                    if (output == 1)
                        updated = true;
                }
            }
            catch (Exception ex)
            { }
            return updated;
        }





        public ModuleParam GetModuleData(string mode, string Menuname, string pagename)
        {
            ModuleParam ObjModuleParam = new ModuleParam();
            try
            {
                SqlConnection con = new SqlConnection(_strConnection);
                SqlCommand cmd = new SqlCommand("SP_Module", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GetModuleDataByPage");
                cmd.Parameters.AddWithValue("@PageURL", pagename);
                cmd.Parameters.AddWithValue("@Menu", Menuname);
                SqlDataReader reader = null;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ObjModuleParam.ModuleID = Convert.ToInt32(reader["ModuleID"].ToString());
                        ObjModuleParam.Parameter = reader["Param"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ObjModuleParam;
        }

    }
}
