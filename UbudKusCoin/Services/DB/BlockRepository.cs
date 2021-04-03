using System.Linq;
using LiteDB;
using UbudKusCoin.Models;

namespace UbudKusCoin.Services.DB
{
    public class BlockRepository
    {

        private LiteDatabase _db;
        public BlockRepository(LiteDatabase db)
        {
            this._db = db;
        }

        public ILiteCollection<Block> GetBlocks()
        {
            var coll = _db.GetCollection<Block>(DbService.TBL_BLOCKS);
            coll.EnsureIndex(x => x.Height);
            return coll;
        }

        public Block GetGenesisBlock()
        {
            var block = GetBlocks().FindAll().FirstOrDefault();
            //var block = blockchain.FindOne(Query.All(Query.Ascending));
            return block;
        }

        public Block GetLastBlock()
        {
            var blockchain = GetBlocks();
            var block = blockchain.FindOne(Query.All(Query.Descending));
            return block;
        }


        public Block GetBlockByHeight(int height)
        {
            var coll = GetBlocks();
            var block = coll.Query().Where(x => x.Height == height).ToEnumerable();
            if (block.Any())
            {
                return block.FirstOrDefault();
            }
            return null;
        }
        public void AddBlock(Block block)
        {
            var blocks = GetBlocks();
            blocks.Insert(block);
        }


    }
}
