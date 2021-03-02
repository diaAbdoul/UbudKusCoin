﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Main;
using UbudKusCoin.Main;

namespace UbudKusCoin
{
    public class BlockForger
    {
        private AllEvents Evt;

        public BlockForger(AllEvents evt)
        {
            Evt = evt;
        }

        private CancellationTokenSource cancelTask;
        
        public void Start()
        {
            cancelTask = new CancellationTokenSource();
            Task.Run(() => DoGenerateBlock(), cancelTask.Token);
            Console.WriteLine("Forger started");
        }


        public void Stop()
        {
            cancelTask.Cancel();
            Console.WriteLine("Forger Stoped");
        }





        public void DoGenerateBlock()
        {
            int i = 0;
            while (true)
            {
                var startTime = DateTime.Now.Second;
                //Int32 startTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                Console.WriteLine("Generate Block{0}", i++);              
                Blockchain.BuildNewBlock();

                Evt.InformBlockCreated();

                Random _random = new Random();
                int num = _random.Next(3000, 20000);

                Thread.Sleep(num);


                var endTime = DateTime.Now.Second;

                var remainTime = Constants.BLOCK_GENERATION_INTERVAL - (endTime - startTime);

                Console.WriteLine("remain Time: {0}", remainTime);

                Thread.Sleep(remainTime < 0 ? 0:remainTime * 1000);

            }
          
        }

    }
}
