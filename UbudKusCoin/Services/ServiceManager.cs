using UbudKusCoin.Services.DB;
using UbudKusCoin.Services.P2P;

namespace UbudKusCoin.Services
{
    public static class ServiceManager
    {

        public static BlockForger Forger {set; get;}
        public static DbService DbService {set; get;}
        public static P2PService P2pService { set; get; }

        public static void AddP2P(P2PService p2p)
        {
            P2pService = p2p;
        }

           public static void AddForger(BlockForger forger)
        {
            Forger = forger;
        }
           public static void AddDB(DbService db)
        {
            DbService = db;
        }
    }
}
