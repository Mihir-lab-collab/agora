using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectsDLL
/// </summary>
/// 
namespace Customer.BLL
{
    public class ProposalsProjectsBLL
    {
        private string p;


        public int projectID { get; set; }
        public string projectTitle { get; set; }
        public string projDesc { get; set; }
        public string accessCode { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public string clientMail { get; set; }
        public int insertedBy { get; set; }
        public string mode { get; set; }
        public int proposalID { get; set; }
        public string status { get; set; }
        public int ProposalCSDefaultID { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectUrl { get; set; }
        public string ProjectDesc { get; set; }
        public byte[] image { get; set; }

        public string ImageName { get; set; }
        public string contentType { get; set; }
        public byte [] bytes { get; set; }

        public ProposalsProjectsBLL()
        {
        }
        public ProposalsProjectsBLL(int projectCSID, string projectCSTitle, string projectUrl, string projectCSDesc, string image)
        {
            this.ProposalCSDefaultID = projectCSID;
            this.ProjectTitle = projectCSTitle;
            this.ProjectUrl = projectUrl;
            this.ProjectDesc = projectCSDesc;
            this.ImageName = image;
        }

        public ProposalsProjectsBLL(int projectID, string projectTitle, string projectDesc, string status, string createdBy, DateTime createdOn, string modifiedBy, DateTime modifiedOn, string clientMail)
        {
            this.projectID = projectID;
            this.projectTitle = projectTitle;
            this.projDesc = projectDesc;
            this.status = status;
            //this.accessCode = accessCode;
            this.createdBy = createdBy;
            this.createdOn = createdOn;
            this.modifiedBy = modifiedBy;
            this.modifiedOn = modifiedOn;
            this.clientMail = clientMail;
        }
        public static List<ProposalsProjectsBLL> GetProjectDetails(int EmpID, Boolean IsAdmin)
        {
            ProposalsProjectsDAL objProposalsProjects = new ProposalsProjectsDAL();
            return objProposalsProjects.GetProjectDetails(EmpID, IsAdmin);
        }
        public static int InsertProposalsProjects(string mode, string projectTitle, string projectDesc, string clientMail, int insertedBy)
        {
            ProposalsProjectsBLL objInsert = new ProposalsProjectsBLL();
            objInsert.mode = mode;
            objInsert.projectTitle = projectTitle;
            objInsert.projDesc = projectDesc;
            objInsert.clientMail = clientMail;
            objInsert.insertedBy = insertedBy;
            ProposalsProjectsDAL objInsertInto = new ProposalsProjectsDAL();
            return objInsertInto.InsertProposalsProjects(objInsert);
        }

        public static bool UpdateProposalProjects(string mode, int proposalID, string projectEditTitle, string projectEditDesc, string clientEditMail, string status, string modifiedBy)
        {
            ProposalsProjectsBLL objInsert = new ProposalsProjectsBLL();
            objInsert.mode = mode;
            objInsert.proposalID = proposalID;
            objInsert.projectTitle = projectEditTitle;
            objInsert.projDesc = projectEditDesc;
            objInsert.clientMail = clientEditMail;
            objInsert.status = status;
            objInsert.modifiedBy = modifiedBy;
            ProposalsProjectsDAL objInsertInto = new ProposalsProjectsDAL();
            return objInsertInto.UpdateProposalProjects(objInsert);
        }

        public static List<ProposalsProjectsBLL> GetProjectCSDetails(string mode)
        {
            ProposalsProjectsDAL objProposalsCSProjects = new ProposalsProjectsDAL();
            return objProposalsCSProjects.GetProjectCSDetails(mode);
        }

        public static int InsertProposalsCSProjects(string mode, string projectCSTitle, string projectUrl, string projectCSDesc, string image, string contentType, byte [] bytes)
        {
            ProposalsProjectsBLL objInsertCS = new ProposalsProjectsBLL();
            objInsertCS.mode = mode;
            objInsertCS.projectTitle = projectCSTitle;
            objInsertCS.ProjectUrl = projectUrl;
            objInsertCS.projDesc = projectCSDesc;
            objInsertCS.ImageName = image;
            objInsertCS.contentType = contentType;
            objInsertCS.bytes = bytes;
            ProposalsProjectsDAL objInsertInto = new ProposalsProjectsDAL();
            return objInsertInto.InsertProposalsCSProjects(objInsertCS);
        }
        public static bool UpdateProposalCSProjects(string mode, int proposalID, string projectEditTitle, string projectUrl, string projectEditDesc, string image, string contentType)
        {
            ProposalsProjectsBLL objUpdateCS = new ProposalsProjectsBLL();
            objUpdateCS.mode = mode;
            objUpdateCS.proposalID = proposalID;
            objUpdateCS.projectTitle = projectEditTitle;
            objUpdateCS.ProjectUrl = projectUrl;
            objUpdateCS.ImageName = image;
            objUpdateCS.contentType = contentType;
            objUpdateCS.projDesc = projectEditDesc;
            ProposalsProjectsDAL objUpdateInto = new ProposalsProjectsDAL();
            return objUpdateInto.UpdateProposalCSProjects(objUpdateCS);
        }

    }
}