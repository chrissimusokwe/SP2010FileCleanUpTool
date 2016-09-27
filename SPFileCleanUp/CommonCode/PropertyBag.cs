// <copyright file="PropertyBag.cs" company="GTConsult">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Christopher Simusokwe</author>
// <date>2012-12-01</date>
// <summary>This contains the SPFileCleanUp.CommonCode.</summary>

namespace SPFileCleanUp.CommonCode
{
    using Microsoft.SharePoint;

    /// <summary>
    /// This class accesses the SharePoint web properties
    /// </summary>
    public class PropertyBag
    {
        /// <summary>
        /// This method gets the property bag value.
        /// </summary>
        /// <param name="web"></param>
        /// <param name="strPropertyKey">The property bag key.</param>
        /// <returns>Returns the property value.</returns>
        public static string GetProperty(SPWeb web, string strPropertyKey)
        {
            string strValue = "0";
            try
            {
                if (web.Site.RootWeb.Properties[strPropertyKey] != null)
                {
                    strValue = web.Site.RootWeb.Properties[strPropertyKey].ToString();
                }
            }
            catch
            {
            }
            return strValue;
        }
    }
}
