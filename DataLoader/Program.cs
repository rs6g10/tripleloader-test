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

            var watch = System.Diagnostics.Stopwatch.StartNew();

            //association.InsertRowsToTable(connectionString);
            InsertTriples(connectionString);

            //IList<Triple> triples = new Query(connectionString).GetTripleFromAssociation(1603724309);
            //var triples = new Query(connectionString).GetTripleFromSubject(12125);
            //var triples = new Query(connectionString).GetTripleFromObject(15265);
            watch.Stop();
            // Console.WriteLine("Total rows returned: {0}. Total execution time: {1}ms", triples.Count, watch.ElapsedMilliseconds);

            Console.Read();
        }

        private static void InsertTriples(string connectionString)
        {
            SqlLoader sqlLoader = new SqlLoader();
            int totalSize = 50000;
            int batchSize = 5000;

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

                sqlLoader.Load(connectionString, triples);
                Console.WriteLine("Done inserting batch {0} of {1} with size: {2}", i, counter, batchSize);
            }
            Console.WriteLine("Done inserting");
        }
    }
}
