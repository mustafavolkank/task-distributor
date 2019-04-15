using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDistributor
{
    /*
     *  Represents a worker
     */
    public class Worker
    {
        public string Name { get; set; }
        public List<Task> TaskList { get; set; }
        public double TotalTaskDuration { get; set; }

        public Worker(string Name)
        {
            this.Name = Name;
            this.TaskList = new List<Task>();
            this.TotalTaskDuration = 0;
        }

        /*
         *  This method adds the given task to the worker's task list
         */
        public void AddTask(Task task) 
        {
            TaskList.Add(task);
            TotalTaskDuration += task.AverageDuration; // used average because task object could contain multiple durations from multiple log files
        }

        /*
         *  This method clears all the tasks in the worker's task list and resets the TotalTaskDuration
         */
        public void ClearTasks() 
        {
            TaskList.Clear();
            TotalTaskDuration = 0;
        }
    }
}
