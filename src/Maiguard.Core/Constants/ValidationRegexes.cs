﻿namespace Maiguard.Core.Constants
{
    /// <summary>
    /// </summary>
    public static class ValidationRegexes
    {
        /// <summary>
        /// </summary>
        public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        
        /// <summary>
        /// </summary>
        public const string Address = @"^[A-Za-z0-9.,\- /]+$";
        
        /// <summary>
        /// </summary>
        public const string AdminId = @"^(SELF|A[A-Z]{5}\d{2,})$";

        /// <summary>
        /// </summary>
        public const string ResidentId = @"^R[A-Z]{5}\d{4,}$";

        /// <summary>
        /// </summary>
        public const string CommunityId = @"^C\d{6}[A-Z]+$";

        /// <summary>
        /// </summary>
        public const string PhoneNumber = @"^\+234\d{10,}$";

        /// <summary>
        /// </summary>
        public const string InvitationCode = @"^[0-9]+$";

        /// <summary>
        /// </summary>
        public const string Name = @"^[A-Za-z]+$";
    }
}