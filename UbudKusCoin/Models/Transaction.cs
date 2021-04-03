using EllipticCurve;
using System;
using UbudKusCoin.Helpers;
using UbudKusCoin.Services;

namespace UbudKusCoin.Models
{

    public class Transaction
    {
        public string Hash { get; set;}
        public long TimeStamp { get; set; }
        public string Sender { set; get; }
        public string Recipient { set; get; }
        public double Amount { set; get; }
        public float Fee { set; get; }

        public static bool AddToPool(Transaction transaction)
        {
            try
            {
                var trxPool =  ServiceManager.DbService.Transactions.GetPool();
                trxPool.Insert(transaction);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static void Add(Transaction transaction)
        {
            var transactions = ServiceManager.DbService.Transactions.GetAll();
            transactions.Insert(transaction);
        }


        /**
        create transaction for each ico account
        **/
        public static void CreateIcoTransction()
        {
            var timeStamp = Utils.GetTime();
            foreach (var acc in Account.GetICOs())
            {

                var newTrx = new Transaction()
                {
                    TimeStamp = timeStamp,
                    Sender = "UKC_rcyChuW7cQcIVoKi1LfSXKfCxZBHysTwyPm88ZsN0BM=",
                    Recipient = acc.Address,
                    Amount = acc.Balance,
                    Fee = 0.0f
                };
                newTrx.Build();

                AddToPool(newTrx);
            }
        }

               /**
        create transaction for each ico account
        **/
        public static void CreateGenesisTransction()
        {
            var timeStamp = Utils.GetTime();
            foreach (var acc in Account.GetGenesis())
            {

                var newTrx = new Transaction()
                {
                    TimeStamp = timeStamp,
                    Sender = "Genesis",
                    Recipient = acc.Address,
                    Amount = acc.Balance,
                    Fee = 0.0f
                };
                newTrx.Build();

                AddToPool(newTrx);
            }
        }


        /**
        * Get balance by name
        */
        public static double GetBalance(string address)
        {
            double balance = 0;
            double spending = 0;
            double income = 0;

            var collection =  ServiceManager.DbService.Transactions.GetAll();
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

        public static bool VerifySignature(string publicKeyHex, string message, string signature)
        {
            var byt = Utils.HexToBytes(publicKeyHex);
            var publicKey = PublicKey.fromString(byt);
            return Ecdsa.verify(message, Signature.fromBase64(signature), publicKey);
        }

        public void Build()
        {
            Hash = GetHash();
        }

        public string GetHash()
        {
            var data = TimeStamp + Sender + Amount + Fee + Recipient;
            return Utils.GenHash(Utils.GenHash(data));
        }


    }

}