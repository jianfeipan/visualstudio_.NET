using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace WpfApplication1_tp2
{
    /// <summary>
    /// author : Jianfei PAN
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        List<Process> processes;
        public MainWindow()
        {
            InitializeComponent();
            processes = new List<Process>();
        }

        public void mainClosing(object sender, CancelEventArgs e) {
            closAllProcesses();
        }

        private void closAllProcesses()
        {
            foreach (Process p in processes)
            {
                    p.Kill();
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
                this.Dispatcher.Invoke((Action)(() =>// multi thread to same UI element
                {
                    updateTextblock();
                }));

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
                this.Dispatcher.Invoke((Action)(() =>
                {
                    updateTextblock();
                }));

            }
            else
            {
                noMoreProcesses();
            }
        }

        private void processExisted(object sender, EventArgs e)
        {
            if (processes.Remove((Process)sender))
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    updateTextblock();
                }));
               
            }         
        }

        private void updateTextblock() {
            textBlock.Text = processes.Count() + " processes are running";
            textBlock.Inlines.Add(new LineBreak());
            if (processes.Count != 0)
            {
                foreach (Process p in processes)
                {
                    textBlock.Text += " " + p.StartInfo.FileName.Split('\\').Last() +" pid: "+ p.Id.ToString();
                    textBlock.Inlines.Add(new LineBreak());
                }
            }

        }

        private void CloseLast_click(object sender, RoutedEventArgs e){
            processes.Last().Kill();
        }

        private void CloseLastBall_click(object sender, RoutedEventArgs e)
        {
            processes.FindLast(delegate (Process proc)
            {
                return proc.StartInfo.FileName.Split('\\').Last().Contains("Ball");
            }).Kill();
        }

        private void CloseLastPremier_click(object sender, RoutedEventArgs e)
        {
            processes.FindLast(delegate (Process proc)
            {
                return proc.StartInfo.FileName.Split('\\').Last().Contains("premier");
            }).Kill();
        }

        private void Closeall_click(object sender, RoutedEventArgs e)
        {
            closAllProcesses();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minimizeAll_click(object sender, RoutedEventArgs e)
        {
            if (processes.Count() != 0)
            {
                foreach (Process p in processes)
                {
                    ShowWindow(p.MainWindowHandle, 2); 
                }
            }
        }
    }
}
