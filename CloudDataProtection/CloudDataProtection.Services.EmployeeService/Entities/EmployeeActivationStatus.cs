namespace CloudDataProtection.Services.EmployeeService.Entities
{
    public enum EmployeeActivationStatus
    {
        /// <summary>
        /// Employee entity has been created
        /// </summary>
        PendingUserCreation = 0,
        
        /// <summary>
        /// User account has been created
        /// </summary>
        PendingActivation = 1,
        
        /// <summary>
        /// User account has been activated
        /// </summary>
        Activated = 100,
        
        /// <summary>
        /// Creating the user account has failed
        /// </summary>
        UserCreationFailed = 999,
        
        /// <summary>
        /// User account has been deleted
        /// </summary>
        UserDeleted = 1000
    }
}