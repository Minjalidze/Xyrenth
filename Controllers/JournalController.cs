using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using Xyrenth.Models;

namespace Xyrenth.Controllers
{
    public class JournalController : Controller
    {
        const string connStr = "Server=127.0.0.1;Port=3306;Database=Journal;Uid=root;Pwd=TA45dsp22;";

        [HttpGet("/journaladdskip")]
        public async Task<IActionResult> JournalAddSkip(string studentID, string lessonID, string date, string reason, string isValid)
        {
            MySqlConnection conn = new(connStr);
            conn.Open();

            string sql = $"INSERT INTO Skips VALUES (NULL, \"{studentID}\", \"{lessonID}\", \"{date}\", \"{reason}\", {isValid})";
            MySqlCommand command = new(sql, conn);

            command.ExecuteNonQuery();
            conn.Close();

            return Content("Пропуск успешно добавлен.");
        }


        [HttpGet("/journallessons")]
        public async Task<IActionResult> JournalLessons()
        {
            MySqlConnection conn = new(connStr);
            conn.Open();

            var list = new List<Lesson>();

            string sql = "SELECT * FROM Lessons;";
            MySqlCommand command = new(sql, conn);

            var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(new Lesson(reader.GetInt32(0), reader.GetString(1)));
            reader.Close();
            conn.Close();

            return Content(JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        [HttpGet("/journalskips")]
        public async Task<IActionResult> JournalSkips()
        {
            MySqlConnection conn = new(connStr);
            conn.Open();

            string sql = "SELECT studentID, lessonID, day, reason, valid FROM Skips;";
            MySqlCommand command = new (sql, conn);

            var list = new List<Skip>();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var date = reader.GetDateTime(2);
                list.Add(new Skip(reader.GetInt32(0), reader.GetInt32(1), $"{date.Day}.{date.Month}.{date.Year}", reader.GetString(3), reader.GetBoolean(4)));
            }
            reader.Close();
            conn.Close();

            return Content(JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        [HttpGet("/journalstudents")]
        public async Task<IActionResult> JournalStudents()
        {
            MySqlConnection conn = new(connStr);
            conn.Open();

            var sql = "SELECT id, secondName, firstName, surname FROM Students;";
            MySqlCommand command = new(sql, conn);

            var list = new List<Student>(); 
            
            var reader = command.ExecuteReader();
            while (reader.Read()) list.Add(new Student(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
            reader.Close();
            conn.Close();

            return Content(JsonConvert.SerializeObject(list, Formatting.Indented));
        }
    }
}
