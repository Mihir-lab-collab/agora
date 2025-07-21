using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Customer.Model;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

/// <summary>
/// Summary description for SkillMatrixBLL
/// </summary>
public class SkillMatrixBLL
{
    public int SkillID { get; set; }
    public int CategoryId { get; set; }
    public int EmployeeSkillID { get; set; }
    public int EmpID { get; set; }
    public int UserID { get; set; }
    public int Experience { get; set; }
    public int Years { get; set; }
    public int Months { get; set; }
    public int EmpCount { get; set; }
    public string Mode { get; set; }
    public string Category { get; set; }
    public string SkillName { get; set; }
    public string Level { get; set; }
    public bool ActiveSkill { get; set; }
    public string Status { get; set; }
    public string EmpName { get; set; }
    public string MaxExperience { get; set; }
    public string InsertedDate { get; set; }

    public List<SkillMatrixBLL> lstEmpSkill = new List<SkillMatrixBLL>();

    public static DataTable dt = null;
    public SkillMatrixBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<SkillMatrixBLL> GetSkills(string mode, string skillName)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        objBLL.Mode = mode;
        objBLL.SkillName = skillName;

        dt = new DataTable();
        dt = objDAL.GetSkills(objBLL);

        return objBLL.BindSkillList(dt);
    }

    private List<SkillMatrixBLL> BindSkillList(DataTable dt)
    {
        List<SkillMatrixBLL> lstBLL = new List<SkillMatrixBLL>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SkillMatrixBLL objBLL = new SkillMatrixBLL();

                objBLL.SkillID = Convert.ToInt32(dt.Rows[i]["SkillID"].ToString());
                objBLL.CategoryId = Convert.ToInt32(dt.Rows[i]["SkillCategoryID"].ToString());
                objBLL.Category = dt.Rows[i]["Category"].ToString();
                objBLL.SkillName = dt.Rows[i]["Skill"].ToString();
                objBLL.EmpCount = Convert.ToInt32(dt.Rows[i]["EmpCount"].ToString());
                objBLL.MaxExperience = dt.Rows[i]["MaxExp"].ToString();
                objBLL.Status = "Closed";

                lstBLL.Add(objBLL);
            }
        }
        return lstBLL;
    }


    public static List<KeyValueModel> GetSkill(int CategoryID)
    {
        SkillMatrixDAL objDAL = new SkillMatrixDAL();
        return objDAL.GetSkill(CategoryID);
    }

    public static List<KeyValueModel> GetCategory()
    {
        SkillMatrixDAL objDAL = new SkillMatrixDAL();
        return objDAL.GetCategory();
    }

    public static int SaveSkill(string mode, int userID, int SkillID, int CategoryID, string SkillName)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        objBLL.Mode = mode;
        objBLL.UserID = userID;
        objBLL.SkillID = SkillID;
        objBLL.CategoryId = CategoryID;
        objBLL.SkillName = SkillName;

        return objDAL.SaveSkill(objBLL);
    }

    public static int SaveCategory(string mode, int userID, int categoryID, string categoryName)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        objBLL.Mode = mode;
        objBLL.UserID = userID;
        objBLL.CategoryId = categoryID;
        objBLL.Category = categoryName;

        return objDAL.SaveCategory(objBLL);
    }


    public SkillMatrixBLL GetEmployeeSkill(string mode, int empID, string skillName = "", int toggleSkill = 0)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        objBLL.Mode = mode;
        objBLL.EmpID = empID;
        objBLL.SkillName = skillName;
        objBLL.EmpCount = toggleSkill;
        dt = new DataTable();
        dt = objDAL.GetEmployeeSkill(objBLL);

        return objBLL.BindEmployeSkill(dt);
    }

    private SkillMatrixBLL BindEmployeSkill(DataTable dt, string Type = "")
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        objBLL.lstEmpSkill = new List<SkillMatrixBLL>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SkillMatrixBLL obj = new SkillMatrixBLL();
            obj.CategoryId = Convert.ToInt32(dt.Rows[i]["SkillCategoryID"].ToString());
            obj.SkillID = Convert.ToInt32(dt.Rows[i]["SkillID"].ToString());
            obj.EmployeeSkillID = Convert.ToInt32(dt.Rows[i]["EmployeeSkillID"].ToString());
            obj.Category = dt.Rows[i]["Category"].ToString();
            obj.SkillName = dt.Rows[i]["SkillName"].ToString();
            obj.Experience = Convert.ToInt16(dt.Rows[i]["Experience"].ToString());
            obj.Level = dt.Rows[i]["Level"].ToString();
            obj.ActiveSkill = Convert.ToBoolean(Convert.ToInt16(dt.Rows[i]["ActiveSkill"].ToString()));

            if (Type == "EMP_DETAIL")
                obj.EmpName = dt.Rows[i]["empName"].ToString();

            objBLL.lstEmpSkill.Add(obj);
        }

        return objBLL;
    }
    //--------
    private SkillMatrixBLL BindEmployeSkillCount(DataTable dt)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        objBLL.lstEmpSkill = new List<SkillMatrixBLL>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SkillMatrixBLL obj = new SkillMatrixBLL();

            obj.EmpName = dt.Rows[i]["empname"].ToString();
            obj.EmpCount = Convert.ToInt32(dt.Rows[i]["empcount"].ToString());
            obj.InsertedDate = dt.Rows[i]["InsertedOn"].ToString();

            objBLL.lstEmpSkill.Add(obj);
        }

        return objBLL;
    }

    /// ---------- Employee Skill Section

    public static List<KeyValueModel> GetEmployee(string mode)
    {
        SkillMatrixDAL objDAL = new SkillMatrixDAL();
        return objDAL.GetEmployee(mode);
    }

    public string SaveEmployeeSkill(string mode, SkillMatrixBLL objEmpSkill)
    {
        string outputID = "0";

        //string strXML = GenerateXML(objEmpSkill.lstEmpSkill);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc = GenXML(objEmpSkill.lstEmpSkill);
        //savefile(strXML, "xmltext");

        SkillMatrixBLL obj = new SkillMatrixBLL();
        obj.Mode = mode;
        obj.EmpID = objEmpSkill.EmpID;
        obj.UserID = objEmpSkill.UserID;

        //return outputID =new SkillMatrixDAL().SaveEmployeeSkill(strXML,obj).ToString();
        return outputID = new SkillMatrixDAL().SaveEmployeeSkill1(xmlDoc, obj).ToString();
    }

    private XmlDocument GenXML(List<SkillMatrixBLL> lstEmpSkill)
    {
        XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
        // Initializes a new instance of the XmlDocument class.          
        XmlSerializer xmlSerializer = new XmlSerializer(lstEmpSkill.GetType());
        // Creates a stream whose backing store is memory. 
        using (MemoryStream xmlStream = new MemoryStream())
        {
            xmlSerializer.Serialize(xmlStream, lstEmpSkill);
            xmlStream.Position = 0;
            //Loads the XML document from the specified string.
            xmlDoc.Load(xmlStream);
            return xmlDoc;

        }
    }
    private void savefile(string strfile, string fname)
    {
        FileStream fs1 = new FileStream(@"D:/" + fname + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fs1);
        writer.Write(strfile);
        writer.Close();
    }
    private string GenerateXML(List<SkillMatrixBLL> lstEmpSkill)
    {

        XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
        // Initializes a new instance of the XmlDocument class.          
        XmlSerializer xmlSerializer = new XmlSerializer(lstEmpSkill.GetType());
        // Creates a stream whose backing store is memory. 
        using (MemoryStream xmlStream = new MemoryStream())
        {
            xmlSerializer.Serialize(xmlStream, lstEmpSkill);
            xmlStream.Position = 0;
            //Loads the XML document from the specified string.
            xmlDoc.Load(xmlStream);
            return xmlDoc.InnerXml;

        }
    }



    public SkillMatrixBLL GetEmployeeDetail(string mode, int SkillID)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        objBLL.Mode = mode;
        objBLL.SkillID = SkillID;
        dt = new DataTable();
        dt = objDAL.GetEmployeeDetail(objBLL);

        return objBLL.BindEmployeSkill(dt, "EMP_DETAIL");
    }

    public static int CheckEmployeeSkillExists(int EmpID)
    {
        int SkillID;
        SkillMatrixDAL objDAL = new SkillMatrixDAL();
        return SkillID = objDAL.CheckEmployeeSkillExists(EmpID);
    }

    public static string GetRemainderMsg()
    {
        SkillMatrixDAL objDAL = new SkillMatrixDAL();
        return objDAL.GetRemainderMsg();
    }

    public SkillMatrixBLL GetEmployeeSkillCount(string mode)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();
        dt = new DataTable();
        dt = objDAL.GetEmployeeSkillCount(mode);

        return objBLL.BindEmployeSkillCount(dt);
    }

    public static DataTable GetHighlightSkill(string mode, int empID)
    {
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        return objDAL.GetHighlightSkill(mode, empID);
    }

    public SkillMatrixBLL GetSkillByID(string mode, int skillID)
    {
        SkillMatrixBLL objBLL = new SkillMatrixBLL();
        SkillMatrixDAL objDAL = new SkillMatrixDAL();

        objBLL.Mode = mode;
        objBLL.SkillID = skillID;
        dt = new DataTable();
        dt = objDAL.GetSkillByID(objBLL);
        if (dt.Rows.Count > 0)
        {
            objBLL.SkillID = Convert.ToInt32(dt.Rows[0]["SkillID"].ToString());
            objBLL.CategoryId = Convert.ToInt32(dt.Rows[0]["SkillCategoryID"].ToString());
            objBLL.Category = dt.Rows[0]["category"].ToString();
            objBLL.SkillName = dt.Rows[0]["skill"].ToString();
        }

        return objBLL;
    }
}