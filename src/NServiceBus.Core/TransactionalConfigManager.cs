namespace NServiceBus
{
    using System;
    using System.Transactions;

    /// <summary>
    /// Contains extension methods to NServiceBus.Configure
    /// </summary>
    [ObsoleteEx(Replacement = "Configure.Transactions.Enable() or Configure.Transactions.Disable()", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]      
    public static class TransactionalConfigManager
    {
        /// <summary>
        /// Sets the transactionality of the endpoint.
        /// If true, the endpoint will not lose messages when exceptions occur.
        /// If false, the endpoint may lose messages when exceptions occur.
        /// </summary>          
        [ObsoleteEx(Replacement = "config.Transactions()", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
        public static Configure IsTransactional(this Configure config, bool value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the transactionality of the endpoint such that 
        /// the endpoint will not lose messages when exceptions occur.
        /// 
        /// Is equivalent to IsTransactional(true);
        /// </summary>
        [ObsoleteEx(Replacement = "config.Transactions.Disable()", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
        public static Configure DontUseTransactions(this Configure config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the isolation level that database transactions on this endpoint will run at.
        /// This value is only relevant when IsTransactional has been set to true.
        /// 
        /// Higher levels like RepeatableRead and Serializable promise a higher level
        /// of consistency, but at the cost of lower parallelism and throughput.
        /// 
        /// If you wish to run sagas on this endpoint, RepeatableRead is the suggested value.
        /// </summary>
        [ObsoleteEx(Replacement = "config.Transactions.Advanced()", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]        
        public static Configure IsolationLevel(this Configure config, IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the time span where a transaction will timeout.
        /// 
        /// Most endpoints should leave it at the default.
        /// </summary>
        [ObsoleteEx(Replacement = "Configure.Transactions.Advanced()", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]                
        public static Configure TransactionTimeout(this Configure config, TimeSpan transactionTimeout)
        {
            throw new NotImplementedException();
        }
    }
}
