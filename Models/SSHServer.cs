namespace Xyrenth.Models
{
	public class SSHServer
	{
		public SSHServer(string name, string password, string iP, string authKey, string rconPort, string rconPassword)
		{
			Name = name;
			Password = password;
			IP = iP;
			AuthKey = authKey;
			RconPort = rconPort;
			RconPassword = rconPassword;
		}

		public string Name { get; set; }
		public string Password { get; set; }
		public string IP { get; set; }
		public string AuthKey { get; set; }
		public string RconPort { get; set; }
		public string RconPassword { get; set; }

		public static SSHServer Get(string name, string password = "")
		{
			var ssh = Timers.SSHList.Find(f => f.Name == name);
			if (ssh == null) return null;
			if (ssh.AuthKey != password) return null;
			return ssh;
		}
	}
}
