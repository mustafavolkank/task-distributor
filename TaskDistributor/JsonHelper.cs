using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using C5;

namespace TaskDistributor
{
    /*
     *  Helper class for serializing workers to Json and deserializing Json file to Tasks
     */
    public class JsonHelper
    {
        /*
         *  This method serializes the workers to json and writes to a output file
         */
        public static void Serialize(IntervalHeap<Worker> workers, int outputNumber) 
        {
            try
            {
                string JSONstring = JsonConvert.SerializeObject(workers.ToArray());
                File.WriteAllText(@"distributions\distribution" + outputNumber + ".json", JSONstring);
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Error when writing workers to file" + ",error: " + e.ToString());
            }
        }

        /*
         *  This method deserializes the json input file to a list of Task
         */
        public static List<Task> Deserialize(string fileName) 
        {
            try
            {
                string JSONstring = File.ReadAllText(fileName);
                List<Task> taskList = JsonConvert.DeserializeObject<List<Task>>(JSONstring);
                return taskList;
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Error when reading tasks from file: " + fileName + ", error: " + e.ToString());
                throw e;
            }
        }
    }
}
