using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace DataLoader
{
    public class Query
    {
        private readonly string _connectionString;
        public Query(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public IList<Triple> GetTripleFromSubject(long orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = string.Format(@"SELECT * FROM Triples_v2 Where OrderId={0}", orderId);
                return connection.Query<Triple>(query).ToList<Triple>();
            }
        }

        public IList<Triple> GetTripleFromAssociation(long association)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = string.Format(@"SELECT * FROM Triples_v2 Where Association={0}", association);
                return connection.Query<Triple>(query).ToList<Triple>();
            }
        }

        public IList<Triple> GetTripleFromObject(long objectId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = string.Format(@"SELECT * FROM Triples_v2 Where ObjectId={0}", objectId);
                return connection.Query<Triple>(query).ToList<Triple>();
            }
        }
    }
}
