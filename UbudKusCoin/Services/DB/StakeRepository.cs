using System;
using System.Collections.Generic;
using LiteDB;
using UbudKusCoin.Models;

namespace UbudKusCoin.Services.DB
{
    public class StakeRepository
    {
        private readonly LiteDatabase _db;

        public static List<Staker> StakerList { get; set; }
        public StakeRepository(LiteDatabase db)
        {
            this._db = db;
        }
        public ILiteCollection<Staker> GetAll()
        {
            var coll = this._db.GetCollection<Staker>(DbService.TBL_STACKER);
            return coll;
        }


        public void Add(Staker staker)
        {
            var stakes = GetAll();

            // insert into database
            stakes.Insert(staker);

            // update list
            StakerList.Add(staker);
        }

        internal void Initialize()
        {
            StakerList = new List<Staker>();
            var staker = GetAll();
            if (staker.Count() < 1)
            {
                // we asume all ICO account stake their coin each 1000
                Add(new Staker
                {
                    Address = "UKC_JavaPsOANbgT5anGjTg0Ih6qdC4mHgbmpF5ptjAJb0g=",
                    Amount = 1000
                });

                Add(new Staker
                {
                    Address = "UKC_mGyJe2kD3cNs4c8d/KHVe4+DSt9mwrLLqlDejXUgdzA=",
                    Amount = 1000
                });

                Add(new Staker
                {
                    Address = "UKC_ZOm+XeyKAEbIb/L41TPEzRRxwMOsZW6HE2WjdxeCFFI=",
                    Amount = 1000
                });

                Add(new Staker
                {
                    Address = "UKC_rMOHTqvkDCLtaoqbkgF3GmM2lewE3R2ZFYDGfq0A/fI=",
                    Amount = 1000
                });

                StakerList.AddRange(GetAll().FindAll());
            }
            else
            {
                StakerList.AddRange(GetAll().FindAll());
            }

        }

        public double GetStake(string address)
        {
            double balance = 0;
            var collection = GetAll();
            var stakes = collection.Find(x => x.Address == address);
            foreach (var stake in stakes)
            {
                balance += stake.Amount;
            }
            return balance;
        }

        public string GetValidator()
        {
            var numOfStakes = StakerList.Count;
            var random = new Random();
            int choosed = random.Next(0, numOfStakes);
            var staker = StakerList[choosed].Address;
            return staker;
        }
    }
}
