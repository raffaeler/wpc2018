using System;

namespace RafIdentityProviderLibrary
{
    internal class RefreshToken
    {
        public static RefreshToken Create(
            string username,
            string clientId,
            TimeSpan duration,
            string initialGrantType,
            string identityProviderId,
            string identity,
            string extra)
        {
            var instance = new RefreshToken();
            instance.Id = Guid.NewGuid().ToString();
            instance.Username = username;
            instance.ClientId = clientId;
            instance.Timestamp = DateTimeOffset.Now;
            instance.Expiration = duration.TotalSeconds;
            instance.Hash = HashHelper.Create(instance.Id);
            instance.InitialGrantType = initialGrantType;
            instance.IdentityProviderId = identityProviderId;
            instance.Identity = identity;
            instance.Extra = extra;
            return instance;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string ClientId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double Expiration { get; set; }
        public string Hash { get; set; }
        public string InitialGrantType { get; set; }
        public string IdentityProviderId { get; set; }
        public string Identity { get; set; }
        public string Extra { get; set; }
    }
}