using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using TP4_chat_interface;

namespace TP4_chat_client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string login="username";
        int portDefault = 44444;
        private RemoteMethods remoteMethods;
        private bool isLogin;
        public MainWindow()
        {
            InitializeComponent();
            isLogin = false;
            remoteMethods = (RemoteMethods)Activator.GetObject(
                typeof(RemoteMethods), "tcp://localhost:12345/Server");
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (isLogin) {
                MessageBox.Show("You have already login");
                return;
            }
            else {
                login = login_textBox.Text.ToString();
                portDefault = Int32.Parse(portNumber_textBox.Text.ToString());
                if (login == "")
                {
                    MessageBox.Show("please input login");
                } else if (portDefault<1024 || portDefault> 49151) {
                    MessageBox.Show("the port number not valid (1024 - 49151)");
                }
                else
                {
                    /*
                    try {
                        remoteMethods.login(44444, "Jianfei");
                    }catch (TP4_chat_interface.UserAlreadyExistedException exception){
                        MessageBox.Show(exception.Message.ToString());
                        return;
                    }*/
                    if (remoteMethods.login(portDefault, login))
                    {
                        isLogin = true;
                    }
                    else {
                        MessageBox.Show("login exist already");
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
            if (isLogin) { }
        }
    }
}
