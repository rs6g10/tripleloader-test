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
            SqlLoader sqlLoader = new SqlLoader();
            var triples = new TripleStore().GetTriples();
            string connectionString = @"Data Source =RAHULPC\SQLEXPRESS;Initial Catalog=TripleStore;Integrated Security=True;MultipleActiveResultSets=true;";
            sqlLoader.Load(connectionString, triples);
            Console.WriteLine("Done inserting");
            Console.Read();
        }
    }

    public class TripleStore
    {


        public DataTable GetTriples()
        {
            Random rnd = new Random();
            DataTable dataTable = new DataTable("Triples");

            dataTable.Columns.Add("OrderId");
            dataTable.Columns.Add("Association");
            dataTable.Columns.Add("ObjectId");

            List<string> associationIds = File.ReadAllLines(@"C:\Rahul\associations.txt").ToList();
            List<Triple> triples = new List<Triple>();
            for (int i = 300000; i <= 400000; i++)
            {
                var values = new object[3];
                foreach (var associationId in associationIds)
                {
                    Console.Write("Making Triples: {0}\r", dataTable.Rows.Count);
                    values[0] = i + 23400;
                    values[1] = Convert.ToInt64(associationId);
                    values[2] = 1000 + i;

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
