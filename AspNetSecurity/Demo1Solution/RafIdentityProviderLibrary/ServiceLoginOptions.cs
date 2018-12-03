using System;
using System.Collections.Generic;
using System.Text;

namespace RafIdentityProviderLibrary
{
    public class ServiceLoginOptions
    {
        /// <summary>
        /// The relative path of the login endpoint
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The secret used to encrypt the singing credentials
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// The token duration
        /// </summary>
        public TimeSpan TokenDuration { get; set; }
    }
}
