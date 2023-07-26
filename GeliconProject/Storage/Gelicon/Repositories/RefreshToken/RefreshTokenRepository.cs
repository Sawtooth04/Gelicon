using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.RefreshToken;

namespace GeliconProject.Storage.Gelicon.Repositories.RefreshToken
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private IStorageContext storageContext;

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public Models.RefreshToken? GetRefreshTokenByUserID(int userID)
        {
            try
            {
                return storageContext.RefreshTokens.Where(r => r.userID == userID).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int? GetUserIDByRefreshToken(string token)
        {
            try
            {
                return storageContext.RefreshTokens.Where(r => r.token == token).Single().userID;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddOrReplaceRefreshToken(string token, int userID)
        {
            try
            {
                Models.RefreshToken refreshToken = storageContext.RefreshTokens.Where(r => r.userID == userID).Single();
                refreshToken.token = token;
            }
            catch (Exception)
            {
                storageContext.RefreshTokens.Add(new Models.RefreshToken() { token = token, userID = userID });
            }
        }
    }
}
