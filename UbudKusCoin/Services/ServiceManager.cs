using UbudKusCoin.Services.DB;
using UbudKusCoin.Services.P2P;

namespace UbudKusCoin.Services
{
    public static class ServiceManager
    {
        public static ChainService ChainService { set; get; }
        public static ForgerService ForgerService { set; get; }
        public static DbService DbService { set; get; }
        public static P2PService P2pService { set; get; }


        public static void Add(
            DbService db,
            ChainService chain,
            ForgerService forger,
            P2PService p2p)
        {
            DbService = db;
            ChainService = chain;
            ForgerService = forger;
            P2pService = p2p;

        }
        public static void Start()
        {
          DbService.Start();
          ChainService.Start();
          ForgerService.Start();
          P2pService.Start();
        }

         public static void Stop()
        {
           
        }


    }
}
