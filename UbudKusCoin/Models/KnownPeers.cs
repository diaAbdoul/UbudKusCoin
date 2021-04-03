using System;

namespace UbudKusCoin.Models
{
    public class KnownPeer
    {
        public Guid ID { get; set; }
        public string Connection { get; set; }
        public DateTime LastReach { get; set; }
        public bool IsItSelf { get; set; }
   
        public bool IsCanReach { get; set; }
    }
}