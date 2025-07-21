<%@ WebHandler Language="C#" Class="ViewImage" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

public class ViewImage : IHttpHandler
{
    byte[] imageBytes = null;
    public void ProcessRequest(HttpContext context)
    {
        clsCommon objCommon = new clsCommon();
        if (context.Request.QueryString["id"] != null)
        {
            int UserId = int.Parse(context.Request.QueryString["id"]);
           
            imageBytes = objCommon.GetEmployeePhoto(UserId);
        }
        else if (!string.IsNullOrEmpty(context.Request.QueryString["FileName"]))
        {
            string filename = context.Request.QueryString["FileName"];
            string TargetDir = System.Configuration.ConfigurationManager.AppSettings["DataBank"].ToString() + @"\Location\";
            if (File.Exists(TargetDir + filename))
            {
                FileStream fs = new FileStream(TargetDir + filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageBytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();
            }
            //context.Response.Write(TargetDir + filename);
        }
        if (imageBytes != null && imageBytes.Length > 0)
        {
                 int MaxWidthSize=120;   
                 context.Response.ContentType = "application/octet-stream";
            
                 if (!string.IsNullOrEmpty(context.Request.QueryString["FileName"]))
                 MaxWidthSize = 200;
            
                int MaxHeightSize = 100;
                if (context.Request.QueryString["from"] != null)
                    MaxHeightSize = 55;

                MemoryStream ms = new MemoryStream(imageBytes);
                System.Drawing.Image oImg = System.Drawing.Image.FromStream(ms);
                int intOldWidth = oImg.Width;
                int intOldHeight = oImg.Height;

                int intNewWidth = intOldWidth;
                int intNewHeight = intOldHeight;

                if (intOldHeight > MaxHeightSize || intOldWidth > MaxWidthSize)
                {
                    Double xRatio = (double)intOldWidth / MaxWidthSize;
                    Double yRatio = (double)intOldHeight / MaxHeightSize;
                    Double ratio = Math.Max(xRatio, yRatio);
                    intNewHeight = (int)Math.Floor(intOldHeight / ratio);
                    intNewWidth = (int)Math.Floor(intOldWidth / ratio);

                    System.Drawing.Image oThumbNail = new Bitmap(intNewWidth, intNewHeight);

                    Graphics oGraphic = Graphics.FromImage(oThumbNail);
                    oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                    oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Rectangle oRectangle = new Rectangle(0, 0, intNewWidth, intNewHeight);

                    oGraphic.DrawImage(oImg, oRectangle);
                    byte[] byteArray = null;
                    MemoryStream msNew = new MemoryStream();
                    oThumbNail.Save(msNew, ImageFormat.Jpeg);
                    byteArray = new byte[ms.Length];
                    msNew.Position = 0;
                    msNew.Read(byteArray, 0, Convert.ToInt32(msNew.Length));
                    context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                    oGraphic.Dispose();
                    oImg.Dispose();
                    ms.Close();
                    ms.Dispose();
                    msNew.Close();
                    msNew.Dispose();
                    oThumbNail.Dispose();
                    oGraphic.Dispose();
                }
                else
                    context.Response.OutputStream.Write(imageBytes, 0, imageBytes.Length);            
        }
        else
        {
            context.Response.ContentType = "application/octet-stream";
            if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
                context.Response.WriteFile("../../Images/NoImage.jpg");
        }


        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}