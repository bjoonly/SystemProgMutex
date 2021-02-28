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

namespace Task1
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Semaphore s = new Semaphore(3, 3);
 
            for (int i = 0; i < 10; ++i) {
           
                
               ThreadPool.QueueUserWorkItem(Show, s);
            }

            }

        public void Show(object obj)
        {

            Semaphore s = obj as Semaphore;


            bool stop = false;
            while (!stop)
            {
                if (s.WaitOne(500))
                {
                    try
                    {

                            int currentId = Thread.CurrentThread.ManagedThreadId;
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            listBox.Items.Add(("Thread "+ currentId + " got a lock"));
                        }
                        ));
                       
                        Random random = new Random();
                        for (int i = 0; i < 10; i++)
                        {
                            int number = random.Next(-20, 21);
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                               
                                listBox.Items.Add(number);
                            }
                            ));
                              
                            Thread.Sleep(200);
                        }
                        Thread.Sleep(1000);

                    }
                    finally
                    {
                        stop = true;
                        int currentId = Thread.CurrentThread.ManagedThreadId;
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            listBox.Items.Add(("Thread " + currentId + " remove a lock"));
                        }
                            ));
                        s.Release();
                    }
                }                
            }

        }
    }
}


