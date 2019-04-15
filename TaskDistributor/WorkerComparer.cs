using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDistributor
{
    /*
     *  Implementation of the IComparer for the Worker, compares workers with their TotalTaskDurations
     */
    public class WorkerComparer: IComparer<Worker> 
    {
        int IComparer<Worker>.Compare(Worker worker1, Worker worker2) 
        {
            if (worker1.TotalTaskDuration == worker2.TotalTaskDuration)
            {
                return 0;
            }
            else if (worker1.TotalTaskDuration < worker2.TotalTaskDuration)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
