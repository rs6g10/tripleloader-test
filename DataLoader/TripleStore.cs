using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    public class TripleStore
    {
        public DataTable GetTriples(int min, int max)
        {
            Random rnd = new Random();
            DataTable dataTable = new DataTable("Triples");

            dataTable.Columns.Add("OrderId");
            dataTable.Columns.Add("Association");
            dataTable.Columns.Add("ObjectId");

            List<string> associationIds = File.ReadAllLines(@"C:\rahul\projects\tripleloader-test-master\association_ids.txt").ToList();
            List<Triple> triples = new List<Triple>();
            for (int i = min; i <= max; i++)
            {
                var values = new object[3];
                foreach (var associationId in associationIds)
                {
                    Console.Write("Making Triples: {0}\r", dataTable.Rows.Count);
                    values[0] = i;
                    values[1] = Convert.ToInt64(associationId);
                    values[2] = i;

                    dataTable.Rows.Add(values);
                }
            }
            Console.WriteLine("Done getting triples object!");
            return dataTable;
        }

    }

    public class Triple
    {
        public long OrderId { get; set; }
        public long Association { get; set; }
        public long ObjectId { get; set; }
    }
}
