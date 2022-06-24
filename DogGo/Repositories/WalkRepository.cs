using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.Date, w.Duration, w.DogId, w.WalkerId, d.Name AS DogName, o.Name AS OwnerName, o.Id AS OwnerId
                        FROM Walks as w
                        LEFT JOIN Dog as d ON d.Id = w.DogId
                        LEFT JOIN Owner as o ON o.Id = d.OwnerId
                        WHERE w.WalkerId = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),

                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                }
                            };

                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
    }
}
