using System;

namespace CC_Ingenio_Resources
{
    public static class ResourceUtils
    {                
        #region GetResourceString
        public static string GetResourceString(string AResourceString)
        {
            return Resources.ResourceManager.GetString(AResourceString);
        }
        #endregion
    }
}
