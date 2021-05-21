namespace DomainModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationConfigurations
    {
        /// <summary>
        /// DefaultConncetionString
        /// get and set value of connection string from appsettings.json
        /// </summary>
        public string DefaultConncetionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserConncetionString { get; set; }

        /// <summary>
        /// Secure Key for hashing
        /// </summary>
        public string SecureKey { get; set; }


    }

    public class ConnectionTimeOut
    {
        public int TimeOut { get; set; }
        public static int _bulkCopyConnectionTimeOut { get; set; } = 86400;
    }
}