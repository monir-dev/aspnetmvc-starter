using System;
using System.Web;

namespace aspnetmvc_starter.Web.Utility
{
    public class AppConstant
    {
        #region Fields
        //private static AIMSSecurityService.Menu currentMenu;
        #endregion

        #region Ctor
        public AppConstant()
        {
            //if ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"] != null)
            //    currentMenu = (AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"];
            //else
            //    currentMenu = new AIMSSecurityService.Menu();
            
        }
        #endregion

        public static string ProjectName
        {
            get
            {
                string projectName = string.Empty;
                if (System.Configuration.ConfigurationManager.AppSettings["ProjectName"] != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ProjectName"].ToString();
                }
                else
                {
                    projectName = "LILI_BMS";
                }
                return projectName;
            }
        }
        public static Int32 PageSize
        {

            get { return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString()); }
        }
        public static string FileUploadPath
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["FileUploadPath"]; }
        }

        public static string ApplicationName
        {
            get { return "AIMS"; }
        }
        public static string AIMSModuleName
        {
            get { return "AIMS"; }
        }
        public static string UMSModuleName
        {
            get { return "UMS"; }
        }

        public static string PublicUserName
        {
            get { return "guest"; }
        }
        public static string PublicUserPassword
        {
            get { return "guest123"; }
        }

        ////Current User Info.
        //public static User User
        //{
        //    get
        //    {
        //        if (System.Web.HttpContext.Current.Session["User"] == null)
        //            return new User();
        //        else
        //            return (User)System.Web.HttpContext.Current.Session["User"];
        //    }
        //}
        public static int NumLoginId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[0]); }
        }
        public static string StrLoginId
        {
            get { return (string)HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[1]; }
        }
        public static int intAppUserId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[2]); }
        }
        public static string strAppUserId
        {
            get { return (string)HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[3];}
        }
        public static string UserFullName
        {
            get { return (string)HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[4]; }
        }
        public static int OrganizationTypeId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[5]); }
        }
        public static string OrganizationTypeName
        {
            get { return (string)HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[6]; }
        }
        public static int OrgTypeFundSourceMinistryId
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[7]); }
        }
        public static string OrganizationName
        {
            get { return (string)HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[8]; }
        }
        public static string EMail
        {
            get { return (string)HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[9]; }
        }
        public static int ProjectPermissionType
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[14]); }
        }
        //Right Permission
        public static bool IsApproveAssigned
        {
            //get { return SecurityAgent.GetRightByLoginIdAndRightName(HttpContext.Current.User.Identity.Name, "Approve").IsAssignedRight; }
            get { return Convert.ToBoolean(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[10]); }
        }
        public static bool IsLockAssigned
        {
            //get { return SecurityAgent.GetRightByLoginIdAndRightName(HttpContext.Current.User.Identity.Name, "Lock").IsAssignedRight; }
            get { return Convert.ToBoolean(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[11]); }
        }
        public static bool IsADPInfoAssigned
        {
            //get { return SecurityAgent.GetRightByLoginIdAndRightName(HttpContext.Current.User.Identity.Name, "ADPInfo").IsAssignedRight; }
            get { return Convert.ToBoolean(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[12]); }
        }
        public static bool IsAttachmentsAssigned
        {
            //get { return SecurityAgent.GetRightByLoginIdAndRightName(HttpContext.Current.User.Identity.Name, "Attachments").IsAssignedRight; }
            get { return Convert.ToBoolean(HttpContext.Current.Session["User_Identity_Name"].ToString().Split('|')[13]); }
        }

        public static bool IsDonor
        {
            get { return AppConstant.OrganizationTypeId == 1 ? true : false; }
        }
        public static bool IsERD
        {
            get { return AppConstant.OrganizationTypeId == 4 ? true : false; }
        }
        public static bool IsFABA
        {
            get { return AppConstant.OrganizationTypeId == 5 ? true : false; }
        }

        ////Menu Permission
        //public bool IsViewAssigned
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["CurrentMenu"] == null ? false : ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"]).IsAssignedMenu;
        //    }
        //}
        //public bool IsAddAssigned
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["CurrentMenu"] == null ? false : ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"]).IsAddAssigned;
        //    }
        //}
        //public bool IsEditAssigned
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["CurrentMenu"] == null ? false : ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"]).IsEditAssigned;
        //    }
        //}
        //public bool IsDeleteAssigned
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["CurrentMenu"] == null ? false : ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"]).IsDeleteAssigned;
        //    }
        //}
        //public bool IsCancelAssigned
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["CurrentMenu"] == null ? false : ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"]).IsCancelAssigned;
        //    }
        //}
        //public bool IsPrintAssigned
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["CurrentMenu"] == null ? false : ((AIMSSecurityService.Menu)HttpContext.Current.Session["CurrentMenu"]).IsPrintAssigned;
        //    }
        //}

        //UserStatus
        public static string UserStatus_Accept
        {
            get { return "Accept"; }
        }
    }
}