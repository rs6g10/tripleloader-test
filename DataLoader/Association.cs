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

        private Random random = new Random();

        public void InsertRowsToTable()
        {
            var associations = GetAssociations();
            using (
                SqlConnection connection =
                    new SqlConnection(
                        @"Data Source =RAHULPC\SQLEXPRESS;Initial Catalog=TripleStore;Integrated Security=True;MultipleActiveResultSets=true;")
            )
            {
                int tally = connection.Execute(@"insert Associations (Id, AssociationName) values(@Id, @AssociationName)",
                    associations);

                Console.WriteLine(tally);
            }
        }

        public List<Association> GetAssociations()
        {
            var ids = File.ReadAllLines(@"C:\Rahul\associations.txt");
            var associations = new List<Association>();
            foreach (var id in ids)
            {
                var associationName = RandomWordGenerator(new Random().Next(4, 8));
                while (associations.Any(p => p.AssociationName == associationName))
                {
                    associationName = RandomWordGenerator(new Random().Next(4, 8));
                }
                var association = new Association
                {
                    AssociationName = associationName,
                    Id = Convert.ToInt64(id)
                };
                associations.Add(association);
            }
            return associations;
        }

        private string RandomWordGenerator(int requestedLength)
        {
            Random rnd = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            string[] vowels = { "a", "e", "i", "o", "u" };

            string word = "";

            if (requestedLength == 1)
            {
                word = GetRandomLetter(rnd, vowels);
            }
            else
            {
                for (int i = 0; i < requestedLength; i += 2)
                {
                    word += GetRandomLetter(rnd, consonants) + GetRandomLetter(rnd, vowels);
                }

                word = word.Replace("q", "qu").Substring(0, requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
            }

            return word.ToUpperInvariant();
        }

        private string GetRandomLetter(Random rnd, string[] letters)
        {
            return letters[rnd.Next(0, letters.Length - 1)];
        }

        private List<int> GenerateRandom(int count)
        {
            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();
            while (candidates.Count < count)
            {
                // May strike a duplicate.
                candidates.Add(random.Next());
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(candidates);

            // shuffle the results:
            int i = result.Count;
            while (i > 1)
            {
                i--;
                int k = random.Next(i + 1);
                int value = result[k];
                result[k] = result[i];
                result[i] = value;
            }
            return result;
        }
    }
}
