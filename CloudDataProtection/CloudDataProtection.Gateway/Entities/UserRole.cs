namespace CloudDataProtection.Entities
{
    public enum UserRole
    {
        /// <summary>
        /// Client who uses CDP services
        /// </summary>
        Client = 0,
        
        /// <summary>
        /// Employee of CDP
        /// </summary>
        Employee = 1,
        
        /// <summary>
        /// Admin of CDP
        /// </summary>
        Admin = 2
    }
}