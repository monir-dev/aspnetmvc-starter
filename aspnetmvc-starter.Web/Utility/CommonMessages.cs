namespace aspnetmvc_starter.Web.Utility
{
    public enum CommonMessageEnum
    {
        DeleteFailed, DeleteSuccessful, UpdateFailed, UpdateSuccessful, InsertFailed, InsertSuccessful, DataNotFound,MandatoryInputFailed
    }
    public enum CommonAction
    {
        Save, Update, Delete
    }
    public enum ApplicationModule
    {         
        AFP=1
    }

    public static class CommonMessage
    {
        public static string ProjectIdNotFound
        {
            get { return "Please save Project Basic Info. first!"; }
        }

        public static string AccessDenied
        {
            get { return "Access Denied, You are not authorized to access this Information!"; }
        }
        
        public static string ErrorOccurred
        {
            get { return "Sorry, an error occurred while processing your request."; }
        }
        
    }
}