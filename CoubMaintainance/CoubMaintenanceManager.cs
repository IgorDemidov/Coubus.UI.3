using Adam.Core;
using Adam.Core.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoubMaintenance
{
    public static class CoubMaintenanceManager
    {
        public static Guid GetNewJob(Application app, CoubTarget target)
        {
            CoubJob job = new CoubJob(app);
            job.AddNew();
            job.Targets.Add(target);
            job.Save();
            return job.Id;
        }

        public static void Execute(Application app, Guid jobId)
        {
            var manager = new MaintenanceManager(app);
            manager.JobIds.Add(jobId);
            manager.Execute();
        }
    }
}
