using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TP4_chat_interface
{
    public interface RemoteMethods
    {
        bool login(int portNumber, string login);
        bool disconnect(string login);
        bool sendMessage(Message m);

        Message recvMsg(string login);
    }
    //[SerializableAttribute]
    //[ComVisibleAttribute(true)]
    [Serializable]
    public class Message
    {
        private string sender;
        public Message(string s) { sender = s; }

        public string getSender() { return sender; }
    }

    [Serializable]
    public class ServerMessage : Message {
        private List<string> members;
        public ServerMessage(List<string> list) : base("server") {
            members = list;
        }

        public List<string> getMembers() { return members; }
    }

    [Serializable]
    public class ClientMessage : Message {
        private string msg;
        public ClientMessage(string login, string message):base(login)
        {
            msg = message;
        }
        public string getMsg() { return msg; }
    }

    public class CloseMessage : Message
    {
        public CloseMessage(string login) : base(login)
        {
        }
    }
    /*
    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    public class UserNodeFoundException : ServerException, ISerializable
    {
        public UserNodeFoundException() : base("user doesn't exist")
        {
        }

        UserNodeFoundException(string error) : base(error) { }
    }

    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    public class UserAlreadyExistedException : ServerException, ISerializable
    {
        public UserAlreadyExistedException() : base("user exists already")
        {
        }

        UserAlreadyExistedException(string error) : base(error) { }
    }*/
}
