namespace UbudKusCoin.Main
{
    public class Constants
    {
        public const int TOTAL_COINS = 1000;
        public const int TRANSACTION_THRESHOLD = 5;
        public const string FIRST_LEADER = "";
        public const int TRANSACTION_FEE = 1;
        public const string CHAIN = "chain";
        public const string STATE = "state";
        public const string KEY = "key";
        public const string VALUE = "value";
        public const string LastHashKey = "l";
        public const string CUSTOMER_TABLE = "tbl_customers";
        public const string TBL_BLOCK_PREFIX = "nrm_bl_";
        public const string TBL_LAST_BLOCK_PREFIX = "lst_bl_";
        public const string TBL_GENESIS_BLOCK_PREFIX = "gns_bl_";
        public const string TBL_ACC_BALANCE_PREFIX = "acc_balance_";
        public const string TBL_VALIDATOR_PREFIX = "vldtr_";
        public const string TRANSACTION_TYPE_STAKE = "trx_type_stake_";
        public const string TRANSACTION_TYPE_TRANSACTION = "trx_type_transaction_";
        public const string TRANSACTION_TYPE_VALIDATOR_FEE = "trx_type_validator_fee_";
        public const string MESSAGE_TYPE_INENTORY = "INVENTORY";
        public const string MESSAGE_TYPE_GET_BLOCKS = "GET_BLOCKS";
        public const string MESSAGE_TYPE_GET_DATA = "GET_DATA";
        public const string MESSAGE_TYPE_VERSION = "VERSION";
        public const string MESSAGE_SEPARATOR = "||";
        public const string MESSAGE_TYPE_BLOCK = "BLOCK";
        public const string MESSAGE_TYPE_TRANSACTION = "TRANSACTION";
        public const int DIFFICULTY_ADJUSTMENT_INTERVAL = 10;
        public const int BLOCK_GENERATION_INTERVAL = 10;
        public const string MESSAGE_TYPE_CLEAR_TRANSACTIONS = "CLEAR_TRANSACTIONS";
        public enum MESSAGE_TYPE { CHAIN, BLOCK, TRANSACTION, CLEAR_TRANSACTIONS };

    }
}