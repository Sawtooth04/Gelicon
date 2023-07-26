using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RefreshToken
{
    public interface IRefreshTokenRepository : IRepository
    {
        public Models.RefreshToken? GetRefreshTokenByUserID(int userID);

        public int? GetUserIDByRefreshToken(string token);

        public void AddOrReplaceRefreshToken(string token, int userID);
    }
}
