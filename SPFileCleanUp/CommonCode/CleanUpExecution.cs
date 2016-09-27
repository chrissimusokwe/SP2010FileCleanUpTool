// <copyright file="CleanUpExecution.cs" company="GTConsult">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Christopher Simusokwe</author>
// <date>2012-12-01</date>
// <summary>This contains the SPFileCleanUp.CommonCode.</summary>

namespace SPFileCleanUp.CommonCode
{
    using System;
    using Microsoft.SharePoint;
    using System.Data;

    /// <summary>
    /// This class executes the clean up.
    /// </summary>
    public class CleanUpExecution
    {
        /// <summary>
        /// This method deletes the previous versions from a file version history.
        /// </summary>
        /// <param name="web">The web instance.</param>
        public static void CleanUp(SPWeb web)
        {
            SPListCollection ListCollection = web.Lists;
            int CleanUpCheck = 0;

            try
            {
                CleanUpCheck = int.Parse(PropertyBag.GetProperty(web, "spfilecleanup"));

                if (CleanUpCheck == 1)
                {

                    #region Enumerate Document Libraries

                    foreach (SPList list in ListCollection)
                    {
                        if (list.BaseType == SPBaseType.DocumentLibrary && list.Title != "Master Page Gallery"
                            && list.Title != "Form Templates" && list.Title != "Site Assets"
                            && list.Title != "Site Collection Documents" && list.Title != "Style Library"
                            && list.Title != "Site Pages" && list.Title != "Customized Reports"
                            && list.Title != "Site Collection Images" && list.Hidden != true)
                        {

                            //Create table for all web files of document content type.
                            var q = new SPSiteDataQuery
                            {
                                Webs = "<Webs Scope='SiteCollection' />",
                                Lists = "<Lists><List ID='" + list.ID + "' /></Lists>",
                                ViewFields = "<FieldRef Name=\"UniqueId\" /><FieldRef Name='Title'/><FieldRef Name='ID'/><FieldRef Name='ContentType'/><FieldRef Name='LinkFilename'/>",
                                Query = "<Where><Eq><FieldRef Name='ContentType'/><Value Type='Text'>Document</Value></Eq></Where>"
                            };

                            DataTable dt = web.Site.RootWeb.GetSiteData(q);

                            foreach (DataRow row in dt.Rows)
                            {

                                string Temp = row["UniqueId"].ToString();
                                char[] delimeter = new char[] { '#' };
                                String[] parts = Temp.Split(delimeter);
                                string id = parts[1].ToString();
                                var file = web.GetFile(new Guid(id));
                                string filename = file.Name;

                                SPFileVersionCollection fileVersions = file.Versions;

                                int versionCount = fileVersions.Count;

                                if (versionCount > 1)
                                {
                                    int KeepVersion = int.Parse(PropertyBag.GetProperty(web, "keepversions"));

                                    if (file.Versions.Count > KeepVersion && KeepVersion != 0)
                                    {
                                        int counter = file.Versions.Count - KeepVersion;
                                        int versionsPurged = 0;

                                        //loop from most recent specified version and go top-down.
                                        for (int j = counter; j >= 0; j--)
                                        {
                                            if (file.Versions[j] != null)
                                            {
                                                file.Versions[j].Delete();
                                                versionsPurged++;
                                            }
                                        }
                                        Log.WriteToEventLog(versionsPurged + " versions of " + filename.ToString() + " purged at " + DateTime.Now, "SPFileCleanUp");
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Log.WriteToEventLog(ex.Message + " at " + DateTime.Now, "SPFileCleanUp");
            }
        }
    }
}
