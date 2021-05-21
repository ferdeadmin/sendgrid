namespace DataAccessLayer
{
    public static class AzureKeyStoreProvider
    {
        //private static ClientCredential _clientCredential;
        //public static void InitializeAzureKeyVaultProvider(ClientCredential clientCredential)
        //{
        //    _clientCredential = clientCredential;
        //    SqlColumnEncryptionAzureKeyVaultProvider azureKeyVaultProvider =
        //                    new SqlColumnEncryptionAzureKeyVaultProvider(GetToken);
        //    Dictionary<string, SqlColumnEncryptionKeyStoreProvider> providers =
        //                    new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>();
        //    providers.Add(SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider);
        //    SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
        //}


        //public async static Task<string> GetToken(string authority, string resource, string scope)
        //{
        //    var authContext = new AuthenticationContext(authority);
        //    AuthenticationResult result = await authContext.AcquireTokenAsync(resource, _clientCredential).ConfigureAwait(false);

        //    if (result == null)
        //        throw new InvalidOperationException("Failed to obtain the access token");
        //    return result.AccessToken;
        //}
    }
}
