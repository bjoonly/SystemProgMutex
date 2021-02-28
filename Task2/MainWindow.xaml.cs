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

namespace Task2
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mutex mutexObj = new Mutex(false);

                Thread[] threads = new Thread[2];


                threads[0] = new Thread(ShowAsc);
                threads[0].Start(mutexObj);
                
                threads[1] = new Thread(ShowDesc);
                threads[1].Start(mutexObj);
                threads[1].Join(1);
           
                        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void ShowAsc(Object obj)
        {

            Mutex m = obj as Mutex;
            m.WaitOne();
            for (int i = 0; i <= 20; i++)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {

                    ascListBox.Items.Add(i);
                }
                ));

                Thread.Sleep(200);


            }
            m.ReleaseMutex();
        }
        public void ShowDesc(Object obj)
        {

            Mutex m = obj as Mutex;
            m.WaitOne();
            for (int i = 10; i >= 0; i--)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {

                    descListBox.Items.Add(i);
                }
                 ));

                Thread.Sleep(200);

            }
            m.ReleaseMutex();

        }

    }
}
