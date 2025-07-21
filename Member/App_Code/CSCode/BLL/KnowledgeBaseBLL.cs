using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KnowledgeBaseBLL
/// </summary>
public class KnowledgeBaseBLL
{
    public int kbId { get; set; }
    public int empId { get; set; }
    public int attchmentId { get; set; }
    public string kbDate { get; set; }
    public string kbDescrptn { get; set; }
    public string kbComments { get; set; }
    public string kbFile{ get; set; }
    public string empName { get; set; }
    public string kbTitle { get; set; }
    public int techId { get; set; }
    public string techName { get; set; }   
    public string subtechName { get; set; }
    public int projId { get; set; }
    public string projName { get; set; }
    public string Url { get; set; }
    public int custId { get; set; }
    public string projDesc { get; set; }
    public string hDate { get; set; }
    public string masterId { get; set; }
    public string comments { get; set; }

    public string commentHistory{get;set;}
    public KnowledgeBaseBLL(int kbId, int empId, string empName, string kbDate, string kbDescrptn, string kbComments, string kbFile, string kbTitle, int techId, string techName, string subtechName, int projId, string projName, string Url)
    {
        this.kbId = kbId;
        this.empId = empId;
        this.kbDate = kbDate;
        this.kbDescrptn = kbDescrptn;
        this.kbComments = kbComments;
        this.kbFile = kbFile;
        this.empName = empName;
        this.kbTitle = kbTitle;
        this.techId = techId;
        this.techName = techName;
        this.subtechName = subtechName;
        this.projId = projId;
        this.projName = projName;
        this.Url = Url;
    }
    
    public static List<KnowledgeBaseBLL> getall(string mode)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetAllKnowledgeBase(mode);
    }

    public static List<KnowledgeBaseBLL> getAttchment(int kbId)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetKBAttachments(kbId);
    }



    public static List<KnowledgeBaseBLL> GetCmmntHistory(int kbId)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetCommentHistory( kbId);
    }

    //public static DataTable GetCmmntHistory1(string mode, int kbId)
    //{
    //    KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
    //    return objtype.GetCommentHistory1(mode, kbId);
    //}

    public static List<KnowledgeBaseBLL> view(string mode, int kbId)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetKnowledgeBaseView(mode, kbId);
    }


    public static int Insertcmmnt( string commentHistory,int empId,int kbId)
    {
        KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
        obj.commentHistory = commentHistory;
        obj.empId = empId;
        obj.kbId = kbId;
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.InsertCmmntHistory( obj);
    }


   
    public static void Delete(string mode, int kbId)
    {
        KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
        obj.kbId = kbId;
        KnowledgeBaseDAL obj1 = new KnowledgeBaseDAL();
        obj1.deletekb(mode, kbId);
    }

    public static void UpdateKB(string mode, int kbId, string kbTitle, string kbComments, string kbDescrptn, int techId,string Url,string subtechname,string kbFile)
    {
        //KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        //return objtype.UpdateKB(mode, kbId);

        KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
        obj.kbId = kbId;
        obj.kbTitle = kbTitle;
        obj.kbComments = kbComments;
        obj.kbDescrptn = kbDescrptn;
        obj.techId = techId;       
        obj.Url = Url;
        obj.subtechName = subtechname;
        obj.kbFile = kbFile;
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
         objtype.updateKB(mode, obj);
        

    }

    public static void insertcommenthistory(string commentHistory, int kbId,int empId)
    {
        KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
        obj.commentHistory = commentHistory;
        obj.kbId = kbId;
        obj.empId = empId;
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        objtype.InsertCmmntHistory(obj);
    }


    public static int InsertKb(string mode, int empId, string kbDescrptn, string kbComments, string kbFile, string kbTitle, int techid, int projId, string Url,string subtechname)
    {
        KnowledgeBaseBLL obj = new KnowledgeBaseBLL();
        obj.empId = empId;   
        obj.kbDescrptn = kbDescrptn;
        obj.kbComments = kbComments;
        obj.kbFile = kbFile;
        obj.kbTitle = kbTitle;
        obj.projId = projId;
        obj.Url = Url;
        obj.techId = techid;
        obj.subtechName = subtechname;
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
         return objtype.InsertKnowB(mode,obj);
    }

    public KnowledgeBaseBLL(int techId, string techName, string subtechName)
    {
        this.techId = techId;
        this.techName = techName;
        this.subtechName = subtechName;
    }

    public KnowledgeBaseBLL( int empId, string empName)
    {
       this.empId = empId;       
       this.empName = empName;        
    }

    public KnowledgeBaseBLL(string masterId, string empName, string commentHistory, string hDate,int kbId)
    {
        this.masterId = masterId;
        this.empName = empName;
        this.commentHistory = commentHistory;
        this.hDate = hDate;
        this.kbId = kbId;
    }


    public KnowledgeBaseBLL(int attchmentId, int kbId, string kbFile)
    {
        this.attchmentId = attchmentId;
        this.kbId = kbId;
        this.kbFile = kbFile;
    }




    public KnowledgeBaseBLL(int projId, string projName,int custId)
    {
        this.projId = projId;
        this.projName = projName;
        this.custId = custId;
        
    }

    public static List<KnowledgeBaseBLL> getallProj(string mode)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetAllProj(mode);
    }


    public static List<KnowledgeBaseBLL> getallTech(string mode)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetAllTech(mode);
    }


    public static List<KnowledgeBaseBLL> getallsubTech(string mode)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetAllsubTech(mode);
    }



    public static List<KnowledgeBaseBLL> getallEmployee(string mode)
    {
        KnowledgeBaseDAL objtype = new KnowledgeBaseDAL();
        return objtype.GetAllKB(mode);
    }
	public KnowledgeBaseBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}



}