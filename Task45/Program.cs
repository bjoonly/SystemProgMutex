using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task45
{
    class Numbers
    {

        string randomNumbersPath = "randomNumbers.txt";
        string primaryNumbersPath = "primaryNumbers.txt";
        string primaryEnd7NumbersPath = "primaryEnd7Numbers.txt";
        string statisticPath = "statistic.txt";

        public void WriteRandomNumbers(object obj)
        {
            Mutex mutexObj = obj as Mutex;
            mutexObj.WaitOne();


            Random rand = new Random();
            using (StreamWriter sw = new StreamWriter(File.Create(randomNumbersPath)))
            {
                for (int i = 0; i < 20; i++)
                {
                    sw.WriteLine(rand.Next(0, 1001));
                }
                sw.Close();
            }
            Console.WriteLine("Random numbers write");
            Thread.Sleep(500);

            mutexObj.ReleaseMutex();

        }
        public void WritePrimaryNumbers(Object obj)
        {
            Mutex mutexObj = obj as Mutex;
            mutexObj.WaitOne();


            using (StreamReader sr = new StreamReader(File.OpenRead(randomNumbersPath)))
            {
                using (StreamWriter sw = new StreamWriter(File.Create(primaryNumbersPath)))
                {

                    while (!sr.EndOfStream)
                    {
                        int number;
                        if (int.TryParse(sr.ReadLine(), out number))
                        {
                            int count = 0;
                            for (int i = 2; i <= number / 2; i++)
                            {
                                if (number % 2 == 0)
                                {
                                    count++;
                                    break;
                                }
                            }
                            if (count == 0)
                                sw.WriteLine(number);
                        }
                    }
                    sw.Close();
                }
                sr.Close();

            }
            Console.WriteLine("Primary numbers write");
            Thread.Sleep(500);
            mutexObj.ReleaseMutex();
        }
        public void WritePrimaryEnd7Numbers(Object obj)
        {
            Mutex mutexObj = obj as Mutex;
            mutexObj.WaitOne();


            using (StreamReader sr = new StreamReader(File.OpenRead(primaryNumbersPath)))
            {
                using (StreamWriter sw = new StreamWriter(File.Create(primaryEnd7NumbersPath)))
                {

                    while (!sr.EndOfStream)
                    {
                        int number;
                        if (int.TryParse(sr.ReadLine(), out number))
                        {


                            if (number % 10 == 7)
                            {
                                sw.WriteLine(number);
                            }
                        }
                    }
                    sw.Close();
                }
                sr.Close();
            }
            Console.WriteLine("Primary numbers end 7 write");
            Thread.Sleep(500);

            mutexObj.ReleaseMutex();

        }
        public void Statistics(Object obj)
        {
            Mutex mutexObj = obj as Mutex;
            mutexObj.WaitOne();

            using (StreamWriter sw = new StreamWriter(File.Create(statisticPath)))
            {
                List<string> paths = new List<string>();
                paths.Add(randomNumbersPath);
                paths.Add(primaryNumbersPath);
                paths.Add(primaryEnd7NumbersPath);
                for (int i = 0; i < paths.Count; i++)
                {
                    using (StreamReader sr = new StreamReader(File.OpenRead(paths[i])))
                    {
                        int count = 0;
                        List<int> numbers = new List<int>();
                        while (!sr.EndOfStream)
                        {
                            int number;
                            if (int.TryParse(sr.ReadLine(), out number))
                            {
                                numbers.Add(number);
                                count++;

                            }
                        }
                        sw.WriteLine("File: "+paths[i]);
                        sw.WriteLine("Count numbers: " + count);
                        sw.WriteLine("Size of file: " + sr.BaseStream.Length);
                        foreach (var item in numbers)
                        {
                            sw.Write(item + " ");
                        }
                        sw.WriteLine();
                    }
                }
                sw.Close();
            }

            Console.WriteLine("Statistics write");
            Thread.Sleep(500);

            mutexObj.ReleaseMutex();
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Mutex mutexObj = new Mutex(false);

                Thread[] threads = new Thread[4];
                Numbers n = new Numbers();

                threads[0] = new Thread(n.WriteRandomNumbers);
                threads[0].Start(mutexObj);
                threads[0].Join();

                threads[1] = new Thread(n.WritePrimaryNumbers);
                threads[1].Start(mutexObj);
                threads[1].Join();

                threads[2] = new Thread(n.WritePrimaryEnd7Numbers);
                threads[2].Start(mutexObj);
                threads[2].Join();

                threads[3] = new Thread(n.Statistics);
                threads[3].Start(mutexObj);
                threads[3].Join();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}


