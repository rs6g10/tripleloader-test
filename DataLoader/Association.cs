using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataLoader
{
    public class Association
    {
        public long Id { get; set; }
        public string AssociationName { get; set; }

        public void InsertRowsToTable(string connectionString)
        {
            var associations = GetAssociations();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int tally = connection.Execute(@"insert Associations (Id, AssociationName) values(@Id, @AssociationName)",
                    associations);

                Console.WriteLine(tally);
            }
        }

        public List<Association> GetAssociations()
        {

            string[] associationnames = File.ReadAllLines(@"C:\rahul\projects\tripleloader-test-master\associations.txt");
            var associations = new List<Association>();
            List<int> randomIds = Utils.GenerateRandom(50);
            for (int i = 0; i < 50; i++)
            {
                associations.Add(new Association
                {
                    AssociationName = associationnames[i],
                    Id = randomIds[i]
                });
            }
            return associations;
        }


    }
}
