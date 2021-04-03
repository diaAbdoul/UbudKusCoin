using System.Collections.Generic;

namespace UbudKusCoin.Models
{
    public class Account
    {
        public string Address { set; get; }
        public string PublicKey { set; get; }
        public double Balance { set; get; }

        public static List<Account> GetICOs()
        {

            var list = new List<Account>
            {

                new Account
                {
                    // secreet nunber
                    // 11520842075416936956337166257543145030894758329506615265245623459159831684481
                    Address = "UKC_JavaPsOANbgT5anGjTg0Ih6qdC4mHgbmpF5ptjAJb0g=",
                    PublicKey = "a5185d90719f52615930a6f0249ef7e1310a90159eef573805577288db66e107f5b71cdfa607eb759c1e43a1e08375b775019a2086794c61a042a6db7ea58af4",
                    Balance = 10000
                },

                new Account
                {
                    // secret number
                    // 50097633609371174574534106620065769324210518368794492873657273912701099632384
                    Address = "UKC_mGyJe2kD3cNs4c8d/KHVe4+DSt9mwrLLqlDejXUgdzA=",
                    PublicKey = "9c8f0d364104cad2d7928e606e7927d23c181297969d95be3e273c4a1aff0f3362f4c9c8b49e5987a13da1102b555392620865eb54f14d938f180d460daeb0d3",
                    Balance = 20000
                },

                new Account
                {
                    // secret number
                    // 17444769289605527965285869029990128769824151562466867025336412934980476651053
                    Address = "UKC_ZOm+XeyKAEbIb/L41TPEzRRxwMOsZW6HE2WjdxeCFFI=",
                    PublicKey = "7b2240cc9370f6446b7df3b5b27a5da32e1d9c657fbfc9860fff8d9a9cbc250ea224536045e289db13f36561e73b6ecad88a977604d98de56623e64431ba165b",
                    Balance = 15000
                },

                new Account
                {
                    // secret number
                    // 7611318794264389102622393469331744367164041724526682955404226892006202219169
                    Address = "UKC_rMOHTqvkDCLtaoqbkgF3GmM2lewE3R2ZFYDGfq0A/fI=",
                    PublicKey = "f19681e8249493fc3b42a2e4de8dc544a136137fa27b46a6feff3be1d849e61d89e4c4b6ebf15ad5a67c7600de55bc39cb71758b96f8e9ec90f81e729b1f8d39",
                    Balance = 15000
                }

            };

            return list;

        }

        public static List<Account> GetGenesis()
        {

            var list = new List<Account>
            {

                new Account
                {
                    // secreet nunber
                    // 37115820268057954843929458901983051845242353300769768346456079873593606626394
                    Address = "UKC_QPQY9wHP0jxi/0c/YRlch2Uk5ur/T8lcOaawqyoe66o=",
                    PublicKey = "b3295dd867da1117b56edf09049daa93cadc2d83b8b17f4f004e8eaef818ae1aae3bd96dfb25eccc6d3227659b1778191f2dfb42a6a5226d054d73d7dd6f9970",
                    Balance = 5000000
                },

                new Account
                {
                    // secret number
                    // 46084958288583143460506686453126733781485555622618603681695930748076603235149
                    Address = "UKC_rcyChuW7cQcIVoKi1LfSXKfCxZBHysTwyPm88ZsN0BM=",
                    PublicKey = "23b3f7b8806d30d765ecef49035249ef96b5f3fab2e6ed5c196c55d1fec9d55e6c04cb21ff078f8c06ddeb2b9a5d37b4396cbb0e01db8d519a25f1816a6fd803",
                    Balance = 5000000
                }

            };

            return list;

        }
    }

}
