using LiteDB;
using System.Linq;
using System.Collections.Generic;
using System;
using UbudKusCoin.Helpers;
using UbudKusCoin.Models;

namespace UbudKusCoin.Services
{
    public class ChainService
    {
        public const float COINT_REWARD = 0.01f;

        public ChainService()
        {
            Console.WriteLine(" initilize success ...");
        }

        internal object GetGenesisBlock()
        {
           return ServiceManager.DbService.Blocks.GetGenesisBlock();
        }

        public  void Start(){
                 Initialize();
            ServiceManager.DbService.Stakes.Initialize();
        }

        public static float GetCoinReward()
        {
            return COINT_REWARD;
        }

        private void Initialize()
        {

            var blocks = ServiceManager.DbService.Blocks.GetBlocks();

            if (blocks.Count() < 1)
            {
                // crate genesis transaction
                Transaction.CreateGenesisTransction();

                // crate ICO transaction
                Transaction.CreateIcoTransction();

                // get all ico transaction from pool
                var trxPool = ServiceManager.DbService.Transactions.GetPool();
                var transactions = trxPool.FindAll().ToList();

                //convert transaction to json for more easy
                //var strTransactions = JsonConvert.SerializeObject(transactions);

                // create genesis block
                var block = Block.GenesisBlock(transactions);

                // add genesis block to blockchain
                ServiceManager.DbService.Blocks.AddBlock(block);

                // move all record in trx pool to transactions table
                foreach (Transaction trx in transactions)
                {
                    Transaction.Add(trx);
                }

                // clear mempool
                trxPool.DeleteAll();

            }

        }

        public static List<Block> GetBlocks(int pageNumber, int resultPerPage)
        {
            var coll = ServiceManager.DbService.Blocks.GetBlocks();
            coll.EnsureIndex(x => x.Height);
            var query = coll.Query()
                .OrderByDescending(x => x.Height)
                .Offset((pageNumber - 1) * resultPerPage)
                .Limit(resultPerPage).ToList();
            return query;
        }

        public long GetHeight()
        {
            var lastBlock = ServiceManager.DbService.Blocks.GetLastBlock();
            return lastBlock.Height;
        }


        public int GetAdjustedDifficulty(Block latestBlock) {

            var blocks = ServiceManager.DbService.Blocks.GetBlocks();
            Console.WriteLine("==== GetAdjustedDifficulty");
            var prevAdjustmentBlock = ServiceManager.DbService.Blocks.GetBlockByHeight(blocks.Count() - Constants.DIFFICULTY_ADJUSTMENT_INTERVAL);

            Console.WriteLine("prevAdjustmentBlock: " + prevAdjustmentBlock.TimeStamp);
            Console.WriteLine("latestBlock: " + latestBlock.TimeStamp);

            var timeExpected = Constants.BLOCK_GENERATION_INTERVAL * Constants.DIFFICULTY_ADJUSTMENT_INTERVAL;
            Console.WriteLine("timeExpected:" + timeExpected);

            var timeTaken  = latestBlock.TimeStamp - prevAdjustmentBlock.TimeStamp;
            Console.WriteLine("timeTaken:" + timeTaken);

            if (timeTaken < (timeExpected / 2))
            {
                return prevAdjustmentBlock.Difficulty + 1;
            }
            else if (timeTaken > timeExpected * 2)
            {
                return prevAdjustmentBlock.Difficulty - 1;
            }
            else
            {
                return prevAdjustmentBlock.Difficulty;
            }

        }

        public  int GetDifficullty()
        {
            var latestBlock = ServiceManager.DbService.Blocks.GetLastBlock();
            Console.WriteLine("latestBlock.Height:" + latestBlock.Height);
            // Console.WriteLine("Constants.DIFFICULTY_ADJUSTMENT_INTERVAL:" + Constants.DIFFICULTY_ADJUSTMENT_INTERVAL);

            if (latestBlock.Height % Constants.DIFFICULTY_ADJUSTMENT_INTERVAL  == 0 && latestBlock.Height != 0)
            {
                return GetAdjustedDifficulty(latestBlock);
            }
            else
            {
                return latestBlock.Difficulty;
            }
        }

        public void BuildNewBlock()
        {


            // get transaction from pool
            var trxPool = ServiceManager.DbService.Transactions.GetPool();

            //// get last block to get prev hash and last height
            var lastBlock = ServiceManager.DbService.Blocks.GetLastBlock();
            var height = lastBlock.Height + 1;
            var timestamp = Utils.GetTime();
            var prevHash = lastBlock.Hash;
            var validator = ServiceManager.DbService.Stakes.GetValidator();


            var transactions = new List<Transaction>(); // JsonConvert.SerializeObject(new List<Transaction>());


            // validator will get coin reward from genesis account
            // to keep total coin in Blockchain not changed
            var conbaseTrx = new Transaction
            {
                Amount = 0,
                Recipient = "UKC_QPQY9wHP0jxi/0c/YRlch2Uk5ur/T8lcOaawqyoe66o=",
                Fee = COINT_REWARD,
                TimeStamp = timestamp,
                Sender = "UKC_rcyChuW7cQcIVoKi1LfSXKfCxZBHysTwyPm88ZsN0BM="
            };

            if (trxPool.Count() > 0)
            {
                //Get all tx from pool
                conbaseTrx.Recipient = validator;
                conbaseTrx.Amount = GetTotalFees(trxPool.FindAll().ToList());
                conbaseTrx.Build();

                transactions.Add(conbaseTrx);
                transactions.AddRange(trxPool.FindAll());

                // clear mempool
                trxPool.DeleteAll();
            }
            else
            {
                conbaseTrx.Build();
                transactions.Add(conbaseTrx);
            }


            var block = new Block
            {
                Height = height,
                TimeStamp = timestamp,
                PrevHash = prevHash,
                Transactions = transactions,
                Difficulty = GetDifficullty(),
                Validator = validator
            };
            block.Build();
            ServiceManager.DbService.Blocks.AddBlock(block);

            // event
            // Block created 

           // PrintBlock(block);

            // move all record in trx pool to transactions table
            foreach (var trx in transactions)
            {
                Transaction.Add(trx);
            }
        }

        private static float GetTotalFees(IList<Transaction> txs)
        {
            var totFee = txs.AsEnumerable().Sum(x => x.Fee);
            return totFee;
        }

        private static void PrintBlock(Block block)
        {
            Console.WriteLine("\n===========\nNew Block");
            Console.WriteLine(" = Height      : {0}", block.Height);
            Console.WriteLine(" = Version     : {0}", block.Version);
            Console.WriteLine(" = Prev Hash   : {0}", block.PrevHash);
            Console.WriteLine(" = Merkle Hash : {0}", block.MerkleRoot);
            Console.WriteLine(" = Timestamp   : {0}", Utils.ToDateTime(block.TimeStamp));
            Console.WriteLine(" = Difficulty  : {0}", block.Difficulty);
            Console.WriteLine(" = Validator   : {0}", block.Validator);

            Console.WriteLine(" = Number Of Tx: {0}", block.NumOfTx);
            Console.WriteLine(" = Amout       : {0}", block.TotalAmount);
            Console.WriteLine(" = Reward      : {0}", block.TotalReward);

        }
    }
}