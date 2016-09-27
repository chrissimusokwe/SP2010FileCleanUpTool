// <copyright file="CleanUpScheduler.cs" company="GTConsult">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author>Christopher Simusokwe</author>
// <date>2012-12-01</date>
// <summary>This contains the SPFileCleanUp.CommonCode.</summary>

namespace SPFileCleanUp.CommonCode
{
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// This class defines the Timer Job Definition.
    /// </summary>
    public class CleanUpScheduler : SPJobDefinition
    {

        public const string JobName = "SP File Clean Up Scheduler Timer Job Tool";

        public CleanUpScheduler() : base() { }

        public CleanUpScheduler(SPWebApplication webApp)
            : base(JobName, webApp, null, SPJobLockType.Job)
        {
            Title = "SP File Clean Up Scheduler Timer Job Tool";
        }

        /// <summary>
        /// This method overrides the Execute method, to create steps that would be performed during the Jobs execution.
        /// </summary>
        /// <param name="contentDbId"></param>
        public override void Execute(Guid contentDbId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                SPSiteCollection siteCollections = webApp.Sites;

                foreach (SPSite siteCollection in siteCollections)
                {
                    SPWebCollection collWebsite = siteCollection.AllWebs;

                    foreach (SPWeb _subWeb in collWebsite)
                    {
                        CleanUpExecution.CleanUp(_subWeb);                     
                    }
                }
            });
        }
    }
}
