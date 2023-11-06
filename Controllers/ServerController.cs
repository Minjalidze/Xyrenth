using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using Xyrenth.Models;

namespace Xyrenth.Controllers
{
    public class ServerController : Controller
    {
        internal static List<Server> ServerList { get; set; }
        internal static List<Ban> BanList { get; set; }

		[HttpGet("/createban")]
        public async Task<IActionResult> CreateBan(string nick, string steamID, string hwid, string reason, string duration, string date, string price, string ip)
		{
			const string connStr = "Server=127.0.0.1;Port=3306;Database=Xyrenth;Uid=root;Pwd=TA45dsp22;";
			MySqlConnection conn = new(connStr);
            conn.Open();

            if (reason.Contains(':')) { reason = reason.Split(':')[0]; }

            string sql = $"INSERT INTO Bans VALUES (NULL, '{nick}', '{steamID}', '{reason}', '{date}', '{duration}', {price}, '{hwid}', '{ip}')";
            MySqlCommand command = new(sql, conn);

            var ban = new Ban(nick, steamID, reason, hwid, ip, duration, date, price);
            BanList.Add(ban);

            command.ExecuteNonQuery();
            conn.Close();

            return Content("Успешно заблокирован.");
		}

		[HttpGet("/getservers")]
		public async Task<IActionResult> GetServers()
		{
			return Content(JsonConvert.SerializeObject(ServerList, Formatting.Indented));
		}
		[HttpGet("/getbans")]
        public async Task<IActionResult> GetBans()
		{
			return Content(JsonConvert.SerializeObject(BanList, Formatting.Indented));
		}
        [HttpGet("/getplayers")]
        public async Task<IActionResult> GetPlayers(string address)
        {
            try
            {
                var server = Server.Get("", address);
                return Content(server.Players);
            } catch { return Content(""); }
        }

        [HttpGet("/serverupdate")]
        public async Task ServerUpdate(string name, string address, int maxslots, int online)
        {
            var server = Server.Get(name) ?? Server.Get("", address);
            if (server is null)
            {
                ServerList.Add(new Server(name, online, maxslots, address));

                var timer = new System.Timers.Timer();
                timer.Elapsed += (_, _) =>
                {
                    var nowTime = DateTime.Now;
                    var server = Server.Get("", address);
                    if ((nowTime - server.LatestAnswer).Seconds > 5)
                    {
                        ServerList.Remove(server);
                        timer.Close();
                    }
                };
                timer.Interval = 5000;
                timer.Start();
            }
            else
            {
                server.Online = online;
                server.LatestAnswer = DateTime.Now;
            }
        }
        [HttpGet("/serverplayers")]
        public async Task ServerPlayers(string address, string players)
        {
            try
            {
                var server = Server.Get("", address);
                if (string.IsNullOrEmpty(players)) players = ".";
                server.Players = players;
                server.LatestAnswer = DateTime.Now;
            }
            catch { }
        }
    }
}
