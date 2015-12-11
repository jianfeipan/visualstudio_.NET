using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using TP4_chat_interface;

namespace TP4_chat_server
{
    class Server : MarshalByRefObject, TP4_chat_interface.RemoteMethods
    {
        Dictionary<string,User> members = new Dictionary<string, User>();
        static void Main(string[] args)
        {
            // Création d'un nouveau canal pour le transfert des données via un port 
            TcpChannel canal = new TcpChannel(12345);

            // Le canal ainsi défini doit être Enregistré dans l'annuaire
            ChannelServices.RegisterChannel(canal,false);

            // Démarrage du serveur en écoute sur objet en mode Singleton
            // Publication du type avec l'URI et son mode 
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Server), "Server", WellKnownObjectMode.Singleton);

            Console.WriteLine("Server is ready :");
            // pour garder la main sur la console
            Console.ReadLine();
        }

        public bool disconnect(string login)
        {
            if (members.ContainsKey(login))
            {
                Console.WriteLine("Disconnect : " + login);
                members.Remove(login);
                return true;
            }
            else
            {
                //throw new TP4_chat_interface.UserNodeFoundException();
                return false;
            }
        }

        public bool login(int portNumber, string login)
        {
            if (members.ContainsKey(login)) {
                //throw new TP4_chat_interface.UserAlreadyExistedException();
                return false;
            } else {
                members.Add(login, new User(login, portNumber));
                Console.WriteLine("login : " + login + " : "+portNumber);
                return true;
            }
        }

        public Message recvMessage()
        {
            throw new NotImplementedException();
        }

        public bool sendMessage(Message m)
        {
            throw new NotImplementedException();
        }
    }

    public class User {
        private string login;
        private int port;
        public User(string login,int port) {
            this.login = login;
            this.port = port;
        }

        public string getLogin() { return login; }

        public int getPort() { return port; }

        public bool equals(User u) {
            return u.getLogin() == login;
        }
    }
}
