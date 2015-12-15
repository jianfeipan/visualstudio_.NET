using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// Author : Jianfei PAN
    /// </summary>
    class Server : MarshalByRefObject, RemoteMethods
    {
        Dictionary<string,User> members = new Dictionary<string, User>();    
        //static public event CancelEventHandler Closing;
        static void Main(string[] args)
        {
            //Closing += new CancelEventHandler(closeChannels);

            TcpChannel canal = new TcpChannel(12345);

            ChannelServices.RegisterChannel(canal,false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Server), "Server", WellKnownObjectMode.Singleton);

            Console.WriteLine("Server is ready :");

            if (Console.ReadLine()=="exit") {
                Environment.Exit(0);
            }
        }
        public override object InitializeLifetimeService()
        {
            return null ;
        }

        public bool disconnect(string login)
        {
            if (members.ContainsKey(login))
            {
                Console.WriteLine("Disconnect : " + login);
                members.Remove(login);
                Message m = new ServerMessage(members.Keys.ToList<string>());
                foreach (User  u in members.Values.ToList()) {
                    u.getMq().Enqueue(m);
                }
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
                Console.WriteLine("login : " + login);
                Message m = new ServerMessage(members.Keys.ToList<string>());
                foreach (User u in members.Values.ToList())
                {
                    u.getMq().Enqueue(m);
                }
                return true;
            }
        }

        public bool sendMessage(Message m)
        {
            foreach (User u in members.Values.ToList())
            {
                u.getMq().Enqueue(m);
            }
            return true;
        }

        public Message recvMsg(string client)
        {
            Queue<Message> mq = members[client].getMq();
            Message m;
            while (true) {
                if (mq.Count() !=0 ){
                    return mq.Dequeue();
                }
                else {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
    }

    public class User {
        private string login;
        private int port;
        private Queue<Message> msgQ = new Queue<Message>();

        public User(string login, int port) 
        {
            this.login = login;
            this.port = port;          
        }
        public string getLogin() { return login; }
        public Queue<Message> getMq() { return msgQ; }

        public int getPort() { return port; }

        public bool equals(User u) {
            return u.getLogin() == login;
        }
    }
}
