using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;


namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
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

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Breed, Notes, OwnerId, ImageURL
                        FROM Dog";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Dog> dogs = new List<Dog>();
                        while (reader.Read())
                        {
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = !reader.IsDBNull(reader.GetOrdinal("Notes")) ? reader.GetString(reader.GetOrdinal("Notes")) : " ",
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                ImageURL = !reader.IsDBNull(reader.GetOrdinal("ImageURL")) ? reader.GetString(reader.GetOrdinal("ImageUrl")) : " "
                            };

                            dogs.Add(dog);
                        }
                        return dogs;
                    }
                }
            }
        }
        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Id, [Name], Breed, Notes, OwnerId, ImageURL
                        FROM Dog
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Dog dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = !reader.IsDBNull(reader.GetOrdinal("Notes")) ? reader.GetString(reader.GetOrdinal("Notes")) : " ",
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                ImageURL = !reader.IsDBNull(reader.GetOrdinal("ImageURL")) ? reader.GetString(reader.GetOrdinal("ImageUrl")) : " "
                            };

                            return dog;
                        }

                        return null;
                    }
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Dog ([Name], Breed, Notes, OwnerId, ImageURL)
                        OUTPUT INSERTED.ID
                        VALUES (@name, @breed, @notes, @ownerId, @imageURL)";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes == null ? DBNull.Value : dog.Notes);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@imageURL", dog.ImageURL == null ? DBNull.Value : dog.Notes);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }

            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Dog
                        SET
                           [Name] = @name,
                            Breed = @breed,
                            Notes = @notes,
                            OwnerId = @ownerId,
                            ImageURL = @imageUrl
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes == null ? DBNull.Value : dog.Notes);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@imageURL", dog.ImageURL == null ? DBNull.Value : dog.Notes);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
