using System;
using CloudDataProtection.Services.Onboarding.Entities;

namespace CloudDataProtection.Services.Onboarding.Controllers.Dto.Output
{
    public class OnboardingOutput
    {
        public long Id { get; set; }
        
        public DateTime Created { get; set; }

        public OnboardingStatus Status { get; set; }
        
        public long UserId { get; set; }
        
        public GoogleLoginInfoOutput LoginInfo { get; set; }
    }
}