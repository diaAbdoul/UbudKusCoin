using LiteDB;

namespace Main
{
    public class DbAccess
    {

        public static LiteDatabase DB { set; get; }

        public const string DB_NAME = @"DB//node.db";
        public const string TBL_BLOCKS = "tbl_blocks";
        public const string TBL_TRANSACTION_POOL = "tbl_transaction_pool";
        public const string TBL_TRANSACTIONS = "tbl_transactions";
        public const string TBL_STACKER = "tbl_stacker";

        /**
        it will create db with name node.db
        **/
        public static void Initialize(int port)
        {
            DB = new LiteDatabase(DB_NAME + port);
        }

        /**
        Clear Database, delete all rows in each trable
        **/
        public static void ClearDB()
        {
            var coll = DB.GetCollection<Block>(TBL_BLOCKS);
            coll.DeleteAll();

            var coll2 = DB.GetCollection<Transaction>(TBL_TRANSACTION_POOL);
            coll2.DeleteAll();

            var coll3 = DB.GetCollection<Transaction>(TBL_TRANSACTIONS);
            coll3.DeleteAll();


            var coll4 = DB.GetCollection<Transaction>(TBL_STACKER);
            coll4.DeleteAll();

        }
        /**
         * Close database when app closed
         **/
        public static void CloseDB()
        {
            DB.Dispose();
        }

    }
}