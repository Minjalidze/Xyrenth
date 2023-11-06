using Xyrenth.Controllers;

namespace Xyrenth.Models
{
    public class Server
    {
        public string Name { get;set; }
        public int Online { get;set; }
        public int MaxPlayers { get;set; }
        public string Address { get;set; }

        public string Players { get;set; }

        public DateTime LatestAnswer { get; set; }

        public Server(string name, int online, int maxPlayers, string address)
        {
            Name = name;
            Online = online;
            MaxPlayers = maxPlayers;
            Address = address;

            Players = "";

            LatestAnswer = DateTime.Now;
        }

        public static Server Get(string name, string address = "") => ServerController.ServerList.Find(f => address != "" ? f.Address == address : f.Name == name);
    }
}
