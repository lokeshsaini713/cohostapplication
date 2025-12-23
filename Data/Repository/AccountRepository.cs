using Dapper;
using Data.IFactory;
using Data.IRepository;
using Shared.Common.Enums;
using Shared.Model.DTO;
using Shared.Model.Entities;
using Shared.Model.Request.Account;
using Shared.Model.Request.Admin;
using Shared.Utility;
using System.Data;

namespace Data.Repository
{
    public class AccountRepository : CurdRepository<UserDetail>, IAccountRepository
    {
        private readonly IDbConnectionFactory _dbConnection;
        public AccountRepository(IDbConnectionFactory dbConnection):base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<UserDetailsDto> FindByEmailAsync(string email)
        {
            using var connection = _dbConnection.CreateDBConnection();
            var parms = new
            {
                @Email = email
            };
            return await connection.QueryFirstOrDefaultAsync<UserDetailsDto>("GetUserByEmail", param: parms, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateDeviceToken(UpdateDeviceTokenRequest request)
        {
            using var connection = _dbConnection.CreateDBConnection();

            var parms = new
            {
                @Email = request.Email,
                @DeviceType = request.DeviceType,
                @DeviceToken = request.DeviceToken,
                @AccessToken = request.AccessToken
            };
            return await connection.ExecuteAsync("ManageDeviceToken", param: parms, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<UsersDto>> UserList(UsersRequestModel request)
        {
            using var connection = _dbConnection.CreateDBConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                @PageNo = request.Start,
                @PageSize = request.Length,
                @SearchKeyword = request.Search?.Value,
                @SortColumn = request.SortColumn,
                @SortOrder = request.SortOrder,
                @Role = (int)UserTypes.User
            });
            return (await connection.QueryAsync<UsersDto>("GetUserByRole", parameters, commandType: CommandType.StoredProcedure)).AsList();
        }

        public async Task<int> ChangePassword(ChangePasswordModel model, long userId)
        {
            using var connection = _dbConnection.CreateDBConnection();

            return await connection.ExecuteAsync("ChangePassword", new
            {
                UserId = userId,
                Password = model.Password,
            }, commandType: CommandType.StoredProcedure);
        }


        public async Task<ForgotPasswordDto> ResetPasswordTokenAsync(long userId, string forgotPasswordToken)
        {
            using var connection = _dbConnection.CreateDBConnection();

            var parms = new
            {
                @UserId = userId,
                @ForgotPasswordToken = forgotPasswordToken
            };
            return await connection.QueryFirstOrDefaultAsync<ForgotPasswordDto>("UpdateForgotPasswordToken", param: parms, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> CheckResetPasswordTokenExist(string token)
        {
            using var connection = _dbConnection.CreateDBConnection();
            return await connection.QueryFirstOrDefaultAsync<bool>("CheckResetPasswordTokenExistByToken", new { Token = token }, commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDetailsDto> GetUserDetailByToken(string token)
        {
            using var connection = _dbConnection.CreateDBConnection();
            return await connection.QueryFirstOrDefaultAsync<UserDetailsDto>("GetUserDetailByToken", new { Token = token }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ResetPassword(ResetPasswordModel model)
        {
            using var connection = _dbConnection.CreateDBConnection();
            return await connection.ExecuteAsync("UpdateUserByToken", new
            {
                ForgotPasswordToken = model.Token,
                Password = Encryption.ComputeHash(model.Password),
            }, commandType: CommandType.StoredProcedure);
        }

        public CheckUserAccessTokenDto CheckUserAccessToken(string accessToken)
        {
            using var connection = _dbConnection.CreateDBConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AccessToken", accessToken);
            return connection.QueryFirstOrDefault<CheckUserAccessTokenDto>("CheckAccessTokenExists", parameters, commandType: CommandType.StoredProcedure);
        }

        public bool CheckAppVersion(string appVersion, short deviceTypeId)
        {
            using var connection = _dbConnection.CreateDBConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AppVersion", appVersion);
            parameters.Add("@DeviceTypeId", deviceTypeId);
            return connection.QueryFirstOrDefault<bool>("CheckAppVersion", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> LogoutUser(int id)
        {
            using var connection = _dbConnection.CreateDBConnection();
            var parameters = new { Id = id };
            return await connection.ExecuteAsync("LogoutUser", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
