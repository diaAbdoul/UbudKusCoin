using LiteDB;
using System;
using System.Collections.Generic;
using UbudKusCoin;

namespace Main
{

    public class Transaction
    {
        public string ID { get; set;}
        public long TimeStamp { get; set; }
        public string Sender { set; get; }
        public string Recipient { set; get; }
        public double Amount { set; get; }
        public float Fee { set; get; }

        public static void AddToPool(Transaction transaction)
        {
            var trxPool = GetPool();
            trxPool.Insert(transaction);
        }

        public static void Add(Transaction transaction)
        {
            var transactions = GetAll();
            transactions.Insert(transaction);
        }

        public static ILiteCollection<Transaction> GetPool()
        {
            var coll = DbAccess.DB.GetCollection<Transaction>(DbAccess.TBL_TRANSACTION_POOL);
            return coll;
        }

        public static ILiteCollection<Transaction> GetAll()
        {
            var coll = DbAccess.DB.GetCollection<Transaction>(DbAccess.TBL_TRANSACTIONS);
            return coll;
        }

        /**
        * get transaction list by name
        */
        public static IEnumerable<Transaction> GetTransactions(string address)
        {
            var coll = DbAccess.DB.GetCollection<Transaction>(DbAccess.TBL_TRANSACTIONS);
            coll.EnsureIndex(x => x.Sender);
            coll.EnsureIndex(x => x.Recipient);
            var transactions = coll.Find(x => x.Sender == address || x.Recipient == address);
            return transactions;
        }

        /**
        create transaction for each ico account
        **/
        public static List<Transaction> CreateIcoTransction()
        {
            var trx = new List<Transaction>();
            var lst = IcoBalance.GetIcoAccounts();
            foreach (var acc in lst)
            {
                var newTrx = new Transaction()
                {
                    TimeStamp = DateTime.Now.Ticks,
                    Sender = "coinbase",
                    Recipient = acc.Address,
                    Amount = acc.Balance,
                    Fee = 0.01f
                };

                var trxHash = Utils.GetTrxHash(newTrx);
                newTrx.ID = trxHash;

                trx.Add(newTrx);
            }
            return trx;
        }

        /**
        * Get balance by name
        */
        public static double GetBalance(string address)
        {
            double balance = 0;
            double spending = 0;
            double income = 0;

            var collection = GetAll();
            var transactions = collection.Find(x => x.Sender == address || x.Recipient == address);

            foreach (Transaction trx in transactions)
            {
                var sender = trx.Sender;
                var recipient = trx.Recipient;

                if (address.ToLower().Equals(sender.ToLower()))
                {
                    spending += trx.Amount + trx.Fee;
                }

                if (address.ToLower().Equals(recipient.ToLower()))
                {
                    income += trx.Amount;
                }

                balance = income - spending;
            }

            return balance;
        }

    }

}