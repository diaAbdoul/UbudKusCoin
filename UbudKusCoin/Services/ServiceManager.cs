using UbudKusCoin.Services.P2P;

namespace UbudKusCoin.Services
{
    public static class ServiceManager
    {

        public static BlockForger Forger {set; get;}
        public static P2PService P2pService { set; get; }

        public static void Add(P2PService p2p, BlockForger forger)
        {
            P2pService = p2p;
            Forger = forger;
        }
    }
}
