using System.Collections.Generic;
using LiteDB;
using UbudKusCoin.Models;

namespace UbudKusCoin.Services.DB
{
    public class TransactionRepository
    {

        private LiteDatabase _db;
        public TransactionRepository(LiteDatabase db)
        {
            this._db = db;
        }

        public ILiteCollection<Transaction> GetPool()
        {
            var coll = this._db.GetCollection<Transaction>(DbService.TBL_TRANSACTION_POOL);
            return coll;
        }


        public ILiteCollection<Transaction> GetAll()
        {
            var coll = this._db.GetCollection<Transaction>(DbService.TBL_TRANSACTIONS);
            return coll;
        }

        /**
        * get transaction list by name
        */
        public IEnumerable<Transaction> GetTransactions(string address)
        {
            var coll = this._db.GetCollection<Transaction>(DbService.TBL_TRANSACTIONS);
            coll.EnsureIndex(x => x.TimeStamp);
            //coll.EnsureIndex(x => x.Sender);
            //coll.EnsureIndex(x => x.Recipient);
            var transactions = coll.Find(x => x.Sender == address || x.Recipient == address);
            return transactions;
        }




    }
}
