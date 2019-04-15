using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using C5;

namespace TaskDistributor
{
    /*
     *  This class distributes the tasks to the workers
     */
    public class Distributor
    {
        private int numberOfWorkers;

        private Dictionary<String, Task> tasksDictionary;

        /* This is an Min-Heap, stores workers */
        private IntervalHeap<Worker> workersHeap;

        private int numberOfLogs;

        private int numberOfOutputs;

        public const int DefaultNumberOfWorkers = 3;
        public const int DefaultNumberOfLogs = 1;
        public const int DefaultNumberOfOutputs = 1;

        public Distributor()
        {
            tasksDictionary = new Dictionary<string, Task>();
            numberOfLogs = DefaultNumberOfLogs;
            numberOfOutputs = DefaultNumberOfOutputs;
            numberOfWorkers = DefaultNumberOfWorkers;
            workersHeap = new IntervalHeap<Worker>(numberOfWorkers, new WorkerComparer());
            CreateWorkers();
        }

        public Distributor(int numberOfWorkers, int numberOfLogs, int numberOfOutputs)
        {
            tasksDictionary = new Dictionary<string, Task>();
            this.numberOfLogs = numberOfLogs;
            this.numberOfOutputs = numberOfOutputs;
            this.numberOfWorkers = numberOfWorkers;
            workersHeap = new IntervalHeap<Worker>(numberOfWorkers, new WorkerComparer());
            CreateWorkers();
        }

        /*
         *  This method creates the workers
         */
        public void CreateWorkers() // This method creates workers 
        {
            for (int i = 0; i < numberOfWorkers; i++)
            {
                Worker worker = new Worker("Worker" + (i + 1));
                workersHeap.Add(worker);
            }
        }

        /*
         *  This method reads the logs and adds tasks to the TaskDictionary
         */
        private void ReadLogs() 
        {
            for (int i = 0; i < numberOfLogs; i++)
            {
                AddTasksFromALog(i + 1); 
            }
        }

        /*
         *  This method reads a single log and adds its tasks to the TaskDictionary,
         *  if a task is already exist in the TaskDictionary then adds its duration to the existing task
         */
        private void AddTasksFromALog(int logCount) 
        {
            List<Task> taskListFromSingleLog = JsonHelper.Deserialize(@"logs\log" + logCount + ".json");
            foreach (Task task in taskListFromSingleLog)
            {
                Task existingTask;
                bool isTaskAlreadyExist = tasksDictionary.TryGetValue(task.Name, out existingTask);
                if (isTaskAlreadyExist)
                {
                    existingTask.AddDuration(task.TotalDuration);
                }
                else
                {
                    tasksDictionary.Add(task.Name, task);
                }
            }
        }

        /*
         *  This method is the entry point of the algorithm, calls ReadLogs and the internal DistributeTasks methods
         */
        public void Distribute() 
        {
            ReadLogs(); 
            DistributeTasks();
        }

        /*
         *  This mehtod sorts the tasks in the descending order, 
         *  then loops through the sortedList and each time assigns the task to the worker who has the minimum total duration
         */
        private void DistributeTasks() 
        {
            for (int outputNumber = 0; outputNumber < numberOfOutputs; outputNumber++) 
            {
                List<Task> sortedList;
                if (outputNumber == 0)
                {
                    sortedList = tasksDictionary.Values.OrderByDescending(x => x.CalculateAverageDuration()).ToList();
                }
                else
                {
                    sortedList = tasksDictionary.Values.OrderBy(x => Guid.NewGuid()).ToList(); // this shuffles tasks, used for the alternative distribution
                }

                foreach (Task task in sortedList)
                {
                    Worker minWorker = workersHeap.First();
                    minWorker.AddTask(task);
                    UpdateWorkersHeap(minWorker);
                }

                WriteWorkers(outputNumber + 1);
                ClearWorkersTasks();
            }
        }

        /*
         *  This method clears worker's task list for all workers in the WorkersHeap
         */
        private void ClearWorkersTasks() 
        {
            foreach (Worker worker in workersHeap)
            {
                worker.ClearTasks();
            }
        }

        /*
         *  This method updates the position of a worker in the WorkersHeap
         */
        private void UpdateWorkersHeap(Worker minWorker) 
        {
            workersHeap.DeleteMin();
            workersHeap.Add(minWorker);
        }

        /*
         *  This method writes workers to the file
         */
        private void WriteWorkers(int outputNumber) 
        {
            JsonHelper.Serialize(workersHeap, outputNumber);
        }
    }
}
