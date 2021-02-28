using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task6
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Semaphore sem = new Semaphore(5, 6);              
                Casino c = new Casino();
               
                Player[] players = new Player[c.maxCountPlayers];
                Console.WriteLine("Max players: "+c.maxCountPlayers);
                Thread[] threads = new Thread[c.maxCountPlayers];
                for (int i =0; i < c.maxCountPlayers; i++)
                {
                    
                    threads[i] = new Thread(c.Play);
                    players[i]= new Player();
                     threads[i].Start(new Params() { player = players[i], semaphore = sem });
                    Thread.Sleep(1000);
                }

                for (int i = 0; i < threads.Length; i++)
                {
                    threads[i].Join();
                }
                
                Console.WriteLine("Result write");
                using (StreamWriter sw=new StreamWriter(File.Create("ResultDay.txt")))
                {
                    foreach (var item in players)
                    {
                        sw.WriteLine("Player: "+item.NumberPlayer +" Initial amount: "+item.InitialAmount+"\tFinal amount: "+item.FinalAmount);
                    }
                }
                Console.WriteLine("Day end.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    class Params
    {
        public Player player;
        public Semaphore semaphore;
    
    }
    class Player
    {
        private int numberPlayer;
        public int NumberPlayer { get { return numberPlayer; } set { numberPlayer = value; } }
        static int countPlayers;
        private int initialAmount;
        private int finalAmount;
        public int FinalAmount { get { return finalAmount; } set { finalAmount = value; } }
        public int InitialAmount { get { return initialAmount; } set { initialAmount = value; } }
        public Player()
        {
            lock (this)
            {
                countPlayers++;

                NumberPlayer = countPlayers;
                Random random = new Random();
                InitialAmount = random.Next(10000, 100000);
                FinalAmount = initialAmount;
            }
        }
    }

    class Casino
    {
        public int maxCountPlayers { get; set; } = 0;
        int countDayPlayers = 0;
        int currentPlayers = 0;
        int numberOfBets = 0;
     
        public int NumberOfBets { get { return numberOfBets; } set { numberOfBets = value; if (numberOfBets < 0) numberOfBets = 0; } }
       
        public Casino()
        {
  
            maxCountPlayers = new Random().Next(20, 101);
        }
       
        public void Play(object obj)
        {

            Params param = obj as Params;
           
            param.semaphore.WaitOne();
            Random random = new Random();
            int currentBet;
            int currentNumber;
            lock (this)
            {
                countDayPlayers++;
                currentPlayers++;
            }
            Console.WriteLine("Player: {0} joined the game.", param.player.NumberPlayer);
            Thread.Sleep(1000);
    
            do
            {

               
                Thread.Sleep(500);
                int number= random.Next(1, param.player.FinalAmount);
                currentBet = number;
                Thread.Sleep(10);
                number= random.Next(1, 37);
                currentNumber = number;
                Console.WriteLine("Player: {0} \t Sum: {1} \t Number: {2}", param.player.NumberPlayer, currentBet, currentNumber);
                Thread.Sleep(500);
                int casinoCurrentNumber = random.Next(1,37);
                Console.WriteLine("Current number:"+casinoCurrentNumber);
                    if (currentNumber == casinoCurrentNumber)
                {
                        param.player.FinalAmount += currentBet * 2;
                    Console.WriteLine("Player: {0} \t Win: {1} \t Amount: {2}", param.player.NumberPlayer, currentBet * 2, param.player.FinalAmount);
                }
                else
                {
                        param.player.FinalAmount -= currentBet;
                    Console.WriteLine("Player: {0} \t Lose: {1} \t Amount: {2}", param.player.NumberPlayer, currentBet, param.player.FinalAmount);
                        if (param.player.FinalAmount == 0)
                            break;

                }
                
                Thread.Sleep(1000);
          
            } while (countDayPlayers < maxCountPlayers && param.player.FinalAmount > 0);
          
            Console.WriteLine("Player: {0} leave from game.", param.player.NumberPlayer);
            lock (this) { currentPlayers--; }
            param.semaphore.Release(1);

        }
    }
}
