using MySqlConnector;
using Radzen.Blazor.Rendering;
using System.Security.Cryptography;
using Xyrenth.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Timer = System.Timers.Timer;

namespace Xyrenth.Models
{
    public class Timers
    {
		internal static List<SSHServer> SSHList { get; set; }

		public int DatabaseInterval { get; }
        public int ServerDataInterval { get; }
        public int HashInterval { get; }
        public Timers(
        int hashInterval = 300/*SECONDS*/,
        int databaseInterval = 60/*SECONDS*/,
        int serverDataInterval = 5/*SECONDS*/)
        {
            DatabaseInterval = databaseInterval;
            ServerDataInterval = serverDataInterval;
            HashInterval = hashInterval;

            ClientController.BHashes = new List<Hash>();
            ClientController.LHashes = new List<Hash>();

            SyncBans();
            SyncSSH();

			DoLibrariesHashUpdate();
            //DoBundlesHashUpdate();

            var timerDatabase = new Timer();
            timerDatabase.Elapsed += (_, _) => { SyncBans(); SyncSSH(); };
            timerDatabase.Interval = 1000 * HashInterval;
            timerDatabase.Start();

            var timerHash = new Timer();
            timerHash.Elapsed += (_, _) => { DoLibrariesHashUpdate(); };
            timerHash.Interval = 1000 * HashInterval;
            timerHash.Start();
        }
        private static async void SyncBans()
        {
			const string connStr = "Server=127.0.0.1;Port=3306;Database=Xyrenth;Uid=root;Pwd=TA45dsp22;";
			MySqlConnection conn = new(connStr);
			conn.Open();

			string sql = $"SELECT * FROM Bans;";
			MySqlCommand command = new(sql, conn);

			ServerController.BanList.Clear();

			var reader = command.ExecuteReader();
			while (reader.Read())
			{
				var ban = new Ban(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[7].ToString(), reader[8].ToString(), reader[5].ToString(), reader[4].ToString(), reader[6].ToString());
				ServerController.BanList.Add(ban);
			}

			reader.Close();
			conn.Close();
		}
		private static async void SyncSSH()
		{
			const string connStr = "Server=127.0.0.1;Port=3306;Database=Xyrenth;Uid=root;Pwd=TA45dsp22;";
			MySqlConnection conn = new(connStr);
			conn.Open();

			string sql = $"SELECT * FROM Servers;";
			MySqlCommand command = new(sql, conn);

            SSHList.Clear();

			var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var ssh = new SSHServer(reader[0].ToString(), reader[2].ToString(), reader[1].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
                SSHList.Add(ssh);
            }

            reader.Close();
			conn.Close();
		}

        private static async void DoLibrariesHashUpdate()
        {
            ClientController.LHashes.Clear();

            const string directoryPath = "/rustlegacy";

            using var sha256 = SHA256.Create();
            var fileNames = Directory.EnumerateFiles(directoryPath, "*.dll", SearchOption.AllDirectories);

            foreach (var file in fileNames)
            {
                await using var fileStreamF = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                var hashF = await sha256.ComputeHashAsync(fileStreamF);
                var hashStringF = BitConverter.ToString(hashF).Replace("-", "");

                ClientController.LHashes.Add(new Hash
                {
                    Name = file.Replace("/rustlegacy", ""),
                    Value = hashStringF
                });
            }
        }
    }
}
