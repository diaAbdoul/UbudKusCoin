﻿using System;

namespace UbudKusCoin
{
    public delegate void EventHandler(object sender, EventArgs e);
    public class AllEvents
    {
        
        public AllEvents() { }

        public event EventHandler TransactionCreated;
        public event EventHandler BlockCreated;
  

        protected virtual void OnBlockCreated(EventArgs e)
        {
            BlockCreated?.Invoke(this, e);
        }

        protected virtual void OnTransactionCreated(EventArgs e)
        {
            TransactionCreated?.Invoke(this, e);
        }

        public void InformBlockCreated()
        {
            OnBlockCreated(EventArgs.Empty);
        }


        public void InformTxCreated()
        {
            OnTransactionCreated(EventArgs.Empty);
        }


    }


}
