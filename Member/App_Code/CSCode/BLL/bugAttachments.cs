using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.BLL
{

    public class bugAttachments
    {
        public int bugFileId { get; set; }
        public int bug_Id { get; set; }
        public int bugsResolutionId { get; set; }
        public string bugFilePath { get; set; }
        public DateTime bugFileDate { get; set; }

        public bugAttachments()
        {
        }
        public bugAttachments(int _bugFileId, int _bug_Id,int _bugsResolutionId, string _bugFilePath, DateTime _bugFileDate)
        {
            this.bugFileId = _bugFileId;
            this.bug_Id = _bug_Id;
            this.bugsResolutionId = _bugsResolutionId;
            this.bugFilePath = _bugFilePath;
            this.bugFileDate = _bugFileDate;
        }

        public static List<bugAttachments> GetAllbugAttachmentsByBugId(int BugId)
        {
            bugAttachmentsDAL objbugAttachmentsByBugId = new bugAttachmentsDAL();
            return objbugAttachmentsByBugId.GetAllbugAttachmentsByBugId(BugId);
        }

        public static bool DeletebugAttachmentsByFileId(int bugFileId)
        {
            bugAttachmentsDAL objbugAttachmentsByBugId = new bugAttachmentsDAL();
            return objbugAttachmentsByBugId.DeletebugAttachmentsByFileId(bugFileId);
        }

        public static bool InsertbugAttachments(int bug_Id, string bugFilePath)
        {
            bugAttachments curbugAttachments = new bugAttachments();         
            curbugAttachments.bug_Id = bug_Id;
            curbugAttachments.bugsResolutionId = 0;
            curbugAttachments.bugFilePath = bugFilePath;
            curbugAttachments.bugFileDate = DateTime.Now;
            bugAttachmentsDAL objbugAttachmentsByBugId = new bugAttachmentsDAL();
            return objbugAttachmentsByBugId.InsertbugAttachments(curbugAttachments);
        }

        public static bool InsertResolutionbugAttachments(int bug_Id, int bugsResolutionId,string bugFilePath)
        {
            bugAttachments curbugAttachments = new bugAttachments();
            curbugAttachments.bug_Id = bug_Id;
            curbugAttachments.bugsResolutionId = bugsResolutionId;
            curbugAttachments.bugFilePath = bugFilePath;
            curbugAttachments.bugFileDate = DateTime.Now;
            bugAttachmentsDAL objbugAttachmentsByBugId = new bugAttachmentsDAL();
            return objbugAttachmentsByBugId.InsertbugAttachments(curbugAttachments);
        }

    }
}