using System;
using System.Collections.Generic;
using System.Text;

namespace RafIdentityProviderLibrary
{
    public static class OAuth2Constants
    {
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";

        // grants
        public const string GrantType = "grant_type";
        public const string Password = "password";
        public const string RefreshToken = "refresh_token";


        public const string Username = "username";
        public const string ExpiresIn = "expires_in";
        public const string TokenType = "token_type";
        public const string Nonce = "nonce";
        public const string AccessToken = "access_token";
        public const string IdentityToken = "id_token";

        public const string Error = "error";
        public const string AcrValues = "acr_values";
        public const string LoginHint = "login_hint";
        public const string State = "state";

        public const string ResponseMode = "response_mode";
        public const string ResponseType = "response_type";

        public const string RedirectUri = "redirect_uri";
        public const string Code = "code";
        public const string Scope = "scope";
        public const string Assertion = "assertion";
    }
}
