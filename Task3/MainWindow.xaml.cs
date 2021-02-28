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

namespace Task3
{

    public partial class MainWindow : Window
    {
            bool semaphoreWasCreated;
        Semaphore sem;

        public MainWindow()
        {
            InitializeComponent();
            sem = new Semaphore(2, 3, "CountCopySem", out semaphoreWasCreated);
            if (semaphoreWasCreated)
            {
                MessageBox.Show("First window!");

            }
            else if (sem.WaitOne(0))
            {

                MessageBox.Show("New Window" );

            }
            else
            {
                MessageBox.Show("Can't open more than 3 windows");               
                App.Current.Shutdown();
                }
           
            }


        
    }
    }

