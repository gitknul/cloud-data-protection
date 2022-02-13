namespace CloudDataProtection.Core.Messaging
{
    public static class RoutingKeys
    {
        public static readonly string BackupConfigurationEntered = "BackupConfigurationEntered";

        public static readonly string AdminRegistered = "AdminRegistered";

        public static readonly string ClientRegistered = "ClientRegistered";
        
        public static readonly string ClientDeleted = "ClientDeleted";
        
        public static readonly string ClientDataDeleted = "ClientDataDeleted";

        public static readonly string ClientDeletionComplete = "ClientDeletionComplete";

        public static readonly string GoogleAccountConnected = "GoogleAccountConnected";

        public static readonly string EmailChangeRequested = "EmailChangeRequested";

        public static readonly string PasswordUpdated = "PasswordUpdated";
    }
}