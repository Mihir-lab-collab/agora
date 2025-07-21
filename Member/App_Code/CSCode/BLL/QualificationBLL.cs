using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for QualificationBLL
/// </summary>
public class QualificationBLL
{
    public int QID { get; set; }
    public string QualDesc { get; set; }
    public string QualType { get; set; }
    public int insertedBy { get; set; }
    public int modifiedBy { get; set; }
    public string mode { get; set; }
    public string IpAddress { get; set; }
	public QualificationBLL()
	{
		
	}
    public QualificationBLL(int QID, string qualificationDesc, string qualificationType,  int modifiedBy)
    {
        this.QID = QID;
        this.QualDesc = qualificationDesc;
        this.QualType = qualificationType;
        this.modifiedBy = modifiedBy;
    }

    public QualificationBLL(int QID, string qualificationDesc)
    {
        this.QID = QID;
        this.QualDesc = qualificationDesc;
       
    }
    public static List<QualificationBLL> GetQualificationDetails(string mode)
    {
        QualificationDAL objConfig = new QualificationDAL();
        return objConfig.GetQualificationDetails(mode, 0);
    }

    public static List<QualificationBLL>GetQualification(string mode)
    {
        QualificationDAL objConfig = new QualificationDAL();
        return objConfig.GetQualification(mode);
    }


    public static int InsertQualification(string mode, int QID, string QualDesc, string QualType, int insertedBy)
    {
        QualificationBLL objInsert= new QualificationBLL();
        objInsert.mode = mode;
        objInsert.QID = QID;
        objInsert.QualDesc = QualDesc;
        objInsert.QualType = QualType;

        objInsert.insertedBy = insertedBy;
        QualificationDAL objUpdateInto = new QualificationDAL();
        return objUpdateInto.InsertQualification(objInsert);
    }
    public static bool UpdateQualification(string mode, int QID, string QualDesc, string QualType, int modifiedBy)
    {
        QualificationBLL objUpdate = new QualificationBLL();
        objUpdate.mode = mode;
        objUpdate.QID = QID;
        objUpdate.QualDesc = QualDesc;
        objUpdate.QualType = QualType;

        objUpdate.modifiedBy = modifiedBy;
        QualificationDAL objUpdateInto = new QualificationDAL();
        return objUpdateInto.UpdateQualification(objUpdate);
    }
}