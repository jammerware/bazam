using System;
using System.ComponentModel;

namespace Bazam.Modules
{
    public static class BackgroundBuddy
    {
        public static void RunAsync(Action doThisNow)
        {
            RunAsync(doThisNow, null);
        }

        public static void RunAsync(Action doThisNow, Action doThisAfter)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += (run, allTheThings) => { doThisNow(); };

            if (doThisAfter != null) {
                worker.RunWorkerCompleted += (call, allTheCallbacks) => { doThisAfter(); };
            }

            worker.RunWorkerAsync();
        }
    }
}
