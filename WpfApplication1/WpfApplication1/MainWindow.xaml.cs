using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Process> processes;
        public MainWindow()
        {
            InitializeComponent();
            processes = new List<Process>();
        }

        private void mainClosing(object sender, CancelEventArgs e) {
            foreach (Process p in processes) {
                if (!p.HasExited){
                    p.Kill();
                }
            }
        }

        private void ballButton_Click(object sender, RoutedEventArgs e)
        {
            if (processes.Count < 5)
            {
                Process ballProc = Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + @"\Ballon.exe");
                ballProc.EnableRaisingEvents = true;// important !!!
                ballProc.Exited += new EventHandler(processExisted);
                processes.Add(ballProc);
            }
            else {
                noMoreProcesses();
            }
            
        }

        private void noMoreProcesses()
        {
            MessageBox.Show("Already 5 processes have been created");
        }

        private void permierButton_Click(object sender, RoutedEventArgs e)
        {
            if (processes.Count < 5)
            {

                Process premierProc = Process.Start(System.AppDomain.CurrentDomain.BaseDirectory+@"\premier.exe");
                premierProc.EnableRaisingEvents = true;// important !!!
                premierProc.Exited += new EventHandler(processExisted);
                processes.Add(premierProc);

            }
            else
            {
                noMoreProcesses();
            }
        }

        private void processExisted(object sender, EventArgs e)
        {
            processes.Remove((Process)sender);
        }
    }
}
