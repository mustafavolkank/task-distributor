using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace TaskDistributor
{    
    class Application
    {
        static void Main(string[] args)
        {
            try
            {
                Distributor distributor = new Distributor(3, 2, 2);
                distributor.Distribute(); 
            }
            catch (IOException e) // IOException is already logged
            {
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unknown error" + ", error: " + e.ToString());
                Environment.Exit(0);
            }
        }
    }
}
