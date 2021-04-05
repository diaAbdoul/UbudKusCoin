using System;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UbudKusCoin.Models;

namespace UbudKusCoin.Helpers
{
    public static class Utils
    {
        public static string GenHash(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256.Create().ComputeHash(bytes);
            return BytesToHex(hash);
        }

        public static string GenHashHex(string hex)
        {
            byte[] bytes = HexToBytes(hex);
            byte[] hash = SHA256.Create().ComputeHash(bytes);
            return BytesToHex(hash);
        }

        public static string BytesToHex(byte[] bytes)
        {
            return Convert.ToHexString(bytes).ToLower();
        }

        public static byte[] HexToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static DateTime ToDateTime(long unixTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime;
        }

        public static long GetTime()
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long nowTicks = DateTime.UtcNow.Ticks;
            long tmStamp = ((nowTicks - epochTicks) / TimeSpan.TicksPerSecond);
            return tmStamp;

        }
        public static string CreateDbName(string port)
        {
            return "node_" + port + ".db";
        }
        public static string CreateMerkleRoot(string[] txsHash)
        {

            while (true)
            {
                if (txsHash.Length == 0)
                {
                    return string.Empty;
                }

                if (txsHash.Length == 1)
                {
                    return txsHash[0];
                }

                var newHashList = new List<string>();

                int len = (txsHash.Length % 2 != 0) ? txsHash.Length - 1 : txsHash.Length;

                for (int i = 0; i < len; i += 2)
                {
                    newHashList.Add(DoubleHash(txsHash[i], txsHash[i + 1]));
                }

                if (len < txsHash.Length)
                {
                    newHashList.Add(DoubleHash(txsHash[^1], txsHash[^1]));
                }

                txsHash = newHashList.ToArray();
            }
        }

        static string DoubleHash(string leaf1, string leaf2)
        {
            byte[] leaf1Byte = HexToBytes(leaf1);
            //Array.Reverse(leaf1Byte);

            byte[] leaf2Byte = HexToBytes(leaf2);
            //Array.Reverse(leaf2Byte);

            var concatHash = leaf1Byte.Concat(leaf2Byte).ToArray();
            SHA256 sha256 = SHA256.Create();
            byte[] sendHash = sha256.ComputeHash(sha256.ComputeHash(concatHash));

            //Array.Reverse(sendHash);

            return BytesToHex(sendHash).ToLower();
        }


        public static void PrintBlock(Block block)
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