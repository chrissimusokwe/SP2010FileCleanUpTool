

namespace SPFileCleanUp.Features.SPFileCleanUp.TimerJob
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("cb2ec0f5-d3ce-430e-ac0f-f216455ee57e")]
    public class SPFileCleanUpEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// This method assists in the deletion of an exisiting Job.
        /// </summary>
        /// <param name="jobs"></param>
        private void DeleteJob(SPJobDefinitionCollection jobs)
        {
            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name.Equals(CommonCode.CleanUpScheduler.JobName, StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }
            }
        }

        /// <summary>
        /// The method below handles the event raised after a feature has been activated.
        /// </summary>
        /// <param name="properties"></param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;

                    DeleteJob(webApp.JobDefinitions);

                    var remoteAdministratorAccessDenied = SPWebService.ContentService.RemoteAdministratorAccessDenied;

                    SPWebService.ContentService.RemoteAdministratorAccessDenied = false;

                    CommonCode.CleanUpScheduler Job = new CommonCode.CleanUpScheduler(webApp);

                    SPMinuteSchedule schedule = new SPMinuteSchedule();
                    schedule.BeginSecond = 0;
                    schedule.EndSecond = 59;
                    schedule.Interval = 10;

                    Job.Schedule = schedule;
                    Job.Update();

                    SPWebService.ContentService.RemoteAdministratorAccessDenied = remoteAdministratorAccessDenied;

                });

            }
            catch (Exception ex)
            {
                throw new SPException(ex.Message);
            }
        }

        /// <summary>
        /// The method below handles the event raised before a feature is deactivated.
        /// </summary>
        /// <param name="properties"></param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            DeleteJob(webApp.JobDefinitions);
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
