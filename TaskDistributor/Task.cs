using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskDistributor
{
    /*
     *  Represents a Task, if multiple log files are used represents the combination of a task from the multiple log files
     */
    public class Task
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public double AverageDuration { get; set; }

        /*
         *  stores the duration of a task, if multiple log files are used then stores the total duration of a Task from the multiple log files
         */
        [JsonIgnore]
        public double TotalDuration { get; set; }

        /*
         *  stores that how many durations are added to the Duration property (this is equal to number of log files that contains this task)
         */
        [JsonIgnore]
        public int NumberOfDurations { get; set; }
        
        [JsonConstructor]
        public Task(string name, string type, double duration)
        {
            this.Name = name;
            this.Type = type;
            this.TotalDuration = duration;
            this.NumberOfDurations = 1;
        }

        /*
         *  This method adds the duration of task to existing same task's duration 
         */
        public void AddDuration(double duration) 
        {
            this.TotalDuration += duration;
            NumberOfDurations++;
        }

        /*
         *  This method takes the average of total duration
         */
        public double CalculateAverageDuration() 
        {
            AverageDuration = TotalDuration / NumberOfDurations;
            return AverageDuration;
        }
    }
}
