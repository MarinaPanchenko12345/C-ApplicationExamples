using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAppSql.Models;
using MySql.Data.MySqlClient;

namespace TodoAppSql.Services
{
    internal class DatabaseService
    {
        private readonly string connectionString = "server=localhost;user=root;password=;database=todoapp;";

        public BindingList<TodoModel> LoadData()
        {
            BindingList<TodoModel> list = new BindingList<TodoModel>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM todo_items", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TodoModel
                        {
                            CreationDate = Convert.ToDateTime(reader["creation_date"]),
                            Text = reader["text"].ToString(),
                            IsDone = Convert.ToBoolean(reader["is_done"])
                        });
                    }
                }
            }

            return list;
        }

        public void SaveData(IEnumerable<TodoModel> todoList)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach (var todo in todoList)
                {
                    if (string.IsNullOrWhiteSpace(todo.Text))
                        continue; // ❗ пропускаем пустые строки

                    var cmd = new MySqlCommand(
                        @"INSERT INTO todo_items (creation_date, text, is_done) 
                  VALUES (@date, @text, @done)", conn);

                    cmd.Parameters.AddWithValue("@date", todo.CreationDate);
                    cmd.Parameters.AddWithValue("@text", todo.Text);
                    cmd.Parameters.AddWithValue("@done", todo.IsDone);
                    cmd.ExecuteNonQuery();
                }
            }
        }




        public void DeleteById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM todo_items WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }


    }
}
