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
using System.Threading;
using ClassLibrary1;
using System.Diagnostics;
using System.Windows.Forms;

namespace TP3_Threads
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        bool pause = false;
        List<MyThread> threads;
        int nbballThread = 0;
        int nbPremierThread = 0;

        public MainWindow()
        {
            InitializeComponent();
            threads = new List<MyThread>();
            updateTextblockDispatcher();
        }

        private void ballStart_click(object sender, RoutedEventArgs e)
        {
            if (threads.Count() < 5)
            {
                ThreadWorker ballonworker = new BallonWorker();
                ballonworker.workerFinishedEvent += ballFinishedHandler;
                Thread ballThread = new Thread(new ThreadStart(ballonworker.work));
                MyThread mythread = new MyThread(ballThread, ballonworker);
                threads.Add(mythread);
                ballThread.Start();
                nbballThread++;
                updateTextblockDispatcher();
            }
            else {
                System.Windows.MessageBox.Show("Already 5 threads have been created");
            }

        }

        private void ballFinishedHandler(object sender, EventArgs e)
        {

            MyThread m = threads.Find(delegate (MyThread mythread)
            {
                return mythread.getWorkerInstance().Equals((ThreadWorker)sender);
            });
            if (removeMyThread(m))
            {
                nbballThread--;
                updateTextblockDispatcher();
            }
        }

        private void premierStart_click(object sender, RoutedEventArgs e)
        {

            if (threads.Count() < 5)
            {
                ThreadWorker premierworker = new PremierWorker(nbPremierThread);
                Thread premierThread = new Thread(new ThreadStart(premierworker.work));
                threads.Add(new MyThread(premierThread, premierworker));
                premierThread.Start();
                nbPremierThread++;
                updateTextblockDispatcher();
            }
            else {
                System.Windows.MessageBox.Show("Already 5 threads have been created");
            }
        }

        private void pause_restart_click(object sender, RoutedEventArgs e)
        {
            if (pause)
            {
                foreach (MyThread t in threads)
                {
#pragma warning disable CS0618 // Le type ou le membre est obsolète
                    t.getThread().Resume();//déconsilé
#pragma warning restore CS0618 // Le type ou le membre est obsolète
                }
                pause = false;
            }
            else {
                foreach (MyThread t in threads)
                {
#pragma warning disable CS0618 // Le type ou le membre est obsolète
                    t.getThread().Suspend();//déconsilé
#pragma warning restore CS0618 // Le type ou le membre est obsolète
                }
                pause = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeAllThreads();
        }

        private void exit_click(object sender, RoutedEventArgs e)
        {
            closeAllThreads();
            this.Close();
        }

        private void closeAllThreads() {
            int nb = threads.Count();
            for (int i = nb - 1; i >= 0; i--)
            {
                MyThread t = threads.ElementAt(i);
                t.getThread().Abort();
                threads.Remove(t);
                Debug.WriteLine("closing : " + " i :" + i + t.GetType());
            }
            nbPremierThread = 0;
            nbballThread = 0;

        }

        private void closeLastBall_click(object sender, RoutedEventArgs e)
        {
            MyThread m = threads.FindLast(delegate (MyThread mythread)
             {
                 return mythread.getWorkerInstanceType().IsAssignableFrom(typeof(BallonWorker));
             });
            if (removeMyThread(m))
            {
                nbballThread--;
                updateTextblockDispatcher();
            }
        }

        private bool removeMyThread(MyThread m)
        {
            if (m != null)
            {
                if (pause)
                {
#pragma warning disable CS0618 // Le type ou le membre est obsolète
                    m.getThread().Resume();
#pragma warning restore CS0618 // Le type ou le membre est obsolète
                }
                threads.Remove(m);

                if (threads.Count() == 0) pause = false;
                return true;        
            }
            return false;
        }

        private void closeLastPremier_click(object sender, RoutedEventArgs e)
        {
            MyThread m = threads.FindLast(delegate (MyThread mythread)
           {
               return mythread.getWorkerInstanceType().IsAssignableFrom(typeof(PremierWorker));
           });

            if (removeMyThread(m))
            {
                nbPremierThread--;
                updateTextblockDispatcher();
            }
        }
        private void closeLast_click(object sender, RoutedEventArgs e)
        {
            MyThread m = threads.Last();
            if (m != null) {
                if (pause)
                {
#pragma warning disable CS0618 // Le type ou le membre est obsolète
                    m.getThread().Resume();
#pragma warning restore CS0618 // Le type ou le membre est obsolète
                }
                threads.Remove(m);
                m.getThread().Abort();
                if (m.getWorkerInstanceType().IsAssignableFrom(typeof(BallonWorker)))
                    nbballThread--;
                else
                    nbPremierThread--;
                if (threads.Count() == 0) pause = false;
                updateTextblockDispatcher();
            }
        }
        private void closeAll_click(object sender, RoutedEventArgs e)
        {
            if (pause) {
                foreach (MyThread t in threads)
                {
#pragma warning disable CS0618 // Le type ou le membre est obsolète
                    t.getThread().Resume();//déconsilé
#pragma warning restore CS0618 // Le type ou le membre est obsolète
                }
                pause = false;
            }
            closeAllThreads();
            updateTextblockDispatcher();
        }

        private void updateTextblockDispatcher()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                updateTextblock();
            }));         

        }
        private void updateTextblock() {
            int count = threads.Count();
            textBlock.Text = count + " threads are running : \n";
            textBlock.Text += "number of ballon threads : " + nbballThread + "\n";
            textBlock.Text += "number of premier threads : " + nbPremierThread + "\n";
            textBlock.Text += "********************************************************** \n";
            if (threads.Count != 0)
            {
                for (int i = 0; i < (threads.Count()); i++)
                {
                    MyThread m = threads.ElementAt(i);
                    textBlock.Text += "Thread number : " + i + " is a thread of " + (m.getWorkerInstanceType().IsAssignableFrom(typeof(PremierWorker)) ? "Premier" : "ballon") + " its id is: " + m.getThread().ManagedThreadId + "\n";
                }
            }
        }
    }

    public abstract class ThreadWorker {
        public delegate void workerFinishedEventHandler(object sender, EventArgs e);
        public event EventHandler workerFinishedEvent;
        abstract public void work();
        protected virtual void OnworkerFinishedEvent(EventArgs e)
        {
            EventHandler handler = workerFinishedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class BallonWorker : ThreadWorker
    {
        Ballon ballonInstance;
        public BallonWorker() {
            this.ballonInstance = new Ballon();
        }

        public override void work()
        {
            ballonInstance.Go();
            //if (OnUpdateStatus == null) return;

            EventArgs args = new EventArgs();
            OnworkerFinishedEvent(args);
        }
    }

    public class PremierWorker : ThreadWorker
    {
        Premier premierInstance;
        public PremierWorker(int nbThread)
        {
            this.premierInstance = new Premier(nbThread);
        }
        public override void  work()
        {
            premierInstance.NombrePremiers();
        }
    }

 /*   public class WorkerFinishedEvent : EventArgs
    {
        public bool finished;

        public WorkerFinishedEvent(bool isFinished)
        {
            finished = isFinished;
        }
    }*/

    public class MyThread
    {
        Thread t;
        ThreadWorker worker;

        public MyThread(Thread t, ThreadWorker worker)
        {
            this.t = t;
            this.worker = worker;
        }

        public Thread getThread()
        {
            return t;
        }

        public Type getWorkerInstanceType() {
            return worker.GetType();
        }

        public ThreadWorker getWorkerInstance() {
            return worker;
        }
    }
}
