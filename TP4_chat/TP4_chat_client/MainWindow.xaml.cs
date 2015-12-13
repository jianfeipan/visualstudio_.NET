using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TP4_chat_interface;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace TP4_chat_client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string login="username";
        int portDefault = 12344;
        static private RemoteMethods remoteMethods;
        private bool isLogin;

        private RecvWorker worker;
        private Thread recvMsgThread;
        public MainWindow()
        {
            InitializeComponent();
            isLogin = false;
            remoteMethods = (RemoteMethods)Activator.GetObject(typeof(RemoteMethods), "tcp://localhost:12345/Server");           
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (isLogin) {
                MessageBox.Show("You have already login");
                return;
            }
            else {
                login = login_textBox.Text.ToString();
                try
                {
                    portDefault = Int32.Parse(portNumber_textBox.Text.ToString());
                }
                catch (Exception) {
                    MessageBox.Show("the port number not valid");
                    return;
                }
                if (login == "")
                {
                    MessageBox.Show("please input login");
                } else if (portDefault<1024 || portDefault> 49151) {
                    MessageBox.Show("the port number not valid (1024 - 49151)");
                }
                else
                {
                    try {
                        if (remoteMethods.login(portDefault, login))
                        {
                            isLogin = true;
                            worker = new RecvWorker(this.Dispatcher, members_textBlock, readMsg_textBlock, remoteMethods, login, readMsg_scroller);
                            recvMsgThread = new Thread(new ThreadStart(worker.work));
                            recvMsgThread.Start();
                        }
                        else
                        {
                            MessageBox.Show("login exist already");
                        }
                    }
                    catch (Exception) {
                        MessageBox.Show("server is not running");
                    }                   
                }
                
            }
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            if (isLogin)
            {
                login = login_textBox.Text.ToString();
                if (login == "")
                {
                    MessageBox.Show("please input login");
                }
                else
                {
                    /*
                    try
                    {
                        remoteMethods.disconnect(login);
                    }
                    catch (TP4_chat_interface.UserNodeFoundException exception) {
                        MessageBox.Show(exception.Message.ToString());
                        return;
                    }*/
                    if (remoteMethods.disconnect(login))
                    {
                        isLogin = false;
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            members_textBlock.Text = " disconnected ";
                        }));
                        worker.stop();
                        recvMsgThread.Abort();                       
                   }
                    else
                    {
                        MessageBox.Show("Cannot find " + login + "in the server");
                    }
                }
            }
            else {
                MessageBox.Show("Already disconnected");
            }
        }

        private void send_button_Click(object sender, RoutedEventArgs e)
        {
            sendClientMessage();
        }

        private void sendClientMessage()
        {
            if (isLogin)
            {
                string inputmsg = inputMsg_textBox.Text.ToString();
                if (inputmsg != "")
                {
                    remoteMethods.sendMessage(new TP4_chat_interface.ClientMessage(login, inputmsg));
                }
                inputMsg_textBox.Text = "";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private class RecvWorker
        {
            private Dispatcher dispatcher;
            private TextBlock members_textBlock;
            private TextBlock readMsg_textBlock;
            private ScrollViewer readMsg_scroller;
            private RemoteMethods remote;
            bool loop = true;
            private string login;

            public RecvWorker(Dispatcher dispatcher, TextBlock members_textBlock, TextBlock readMsg_textBlock, RemoteMethods remote, string login, ScrollViewer readMsg_scroller) 
            {
                this.dispatcher = dispatcher;
                this.members_textBlock = members_textBlock;
                this.readMsg_textBlock = readMsg_textBlock;
                this.remote = remote;
                this.login = login;
                this.readMsg_scroller = readMsg_scroller;
            }

            public void stop() {
                loop = false;
            }

            public void work() {
                while (loop) {
                    try
                    {
                        Message msg = remote.recvMsg(login);
                        if (msg.GetType().IsAssignableFrom(typeof(ClientMessage)))
                        {
                            addNewMessageIntheWindow(msg.getSender(), ((ClientMessage)msg).getMsg());
                        }
                        else if (msg.GetType().IsAssignableFrom(typeof(ServerMessage)))
                        {
                            updateMemberList(((ServerMessage)msg).getMembers());
                        }
                        else if (msg.GetType().IsAssignableFrom(typeof(CloseMessage)))
                        {
                            MessageBox.Show("server is not ready");
                        }
                        else
                        {
                            throw new Exception("Unknow message type");                  
                        }

                    } catch (Exception e) {
                        MessageBox.Show(e.Message.ToString());
                    }

                }
            }

            private void updateMemberList(List<string> ms)
            {
                this.dispatcher.Invoke((Action)(() =>
                {
                    members_textBlock.Text = "";
                    for (int i = 1; i <= ms.Count; i++) 
                    {
                        members_textBlock.Text += "  "+i +" : "+ ms.ElementAt(i-1) + "\n";
                    }
                }));
            }

            private void addNewMessageIntheWindow(string s, string v)
            {
                this.dispatcher.Invoke((Action)(() =>
                {
                    readMsg_textBlock.Text += "  " + s + "   :   " + v + "\n";
                    readMsg_scroller.ScrollToBottom(); ;
                }));
            }
        }

        private void inputMsg_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                sendClientMessage();
            }
        }
    }

    
}
