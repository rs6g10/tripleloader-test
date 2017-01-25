using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Association association = new Association();
            string connectionString = KnownSettings.ConnectionString;
            //InsertTriples(connectionString);
            while (true)
            {
                Console.WriteLine("Enter ObjectId (number only):");
                int objectId = int.Parse(Console.ReadLine());
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var triples = new Query(connectionString).GetTripleFromObject(objectId);
                watch.Stop();
                Console.WriteLine("Total rows returned: {0}. Total execution time: {1}ms", triples.Count, watch.ElapsedMilliseconds);
                Console.WriteLine("Do you want to query more? Y/N");
                bool continueLoop = Console.ReadLine().Trim().ToLowerInvariant() == "y";
                if (!continueLoop)
                {
                    break;
                }
            }
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        private static void InsertTriples(string connectionString)
        {
            SqlLoader sqlLoader = new SqlLoader();
            int totalSize = 10000000;
            int batchSize = 50000;

            int counter = (int)Math.Round((Double)(totalSize / batchSize));

            for (int i = 1; i <= counter; i++)
            {
                int min = i;
                int max = i * batchSize;
                if (i > 1)
                {
                    min = ((i - 1) * batchSize) + (i - 1);
                }
                var triples = new TripleStore().GetTriples(min, max);
                int triplesSize = triples.Rows.Count;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                sqlLoader.Load(connectionString, triples);
                watch.Stop();
                Console.WriteLine("Done inserting batch {0} of {1} with size: {2} with time {3}ms", i, counter, triplesSize, watch.ElapsedMilliseconds);
            }
            Console.WriteLine("Done inserting");
        }
    }
}
