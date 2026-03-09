using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;

namespace TestWebTgBot.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<UserEntity> CreateUserAsync(UserEntity userEntity)
    {
        var sql = "INSERT INTO Users(Id , IsAdmin , IsSubscribed, UserFullName ) VALUES(@Id , @IsAdmin , @IsSubscribed, @UserFullName )";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetUserAdminAsync(UserEntity userEntity)
    {
        var sql = "UPDATE Users SET IsAdmin = 1 WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateUserFullName(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitUserFullName} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateDefault(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.Default} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateWaitUserQuestion(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitUserQuestion} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetUserFullName(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserFullName = @UserFullName WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }
    


    public async Task<UserEntity?> GetUserByTelegramId(long telegramId)
    {
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        var user = await _dbConnection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Id = telegramId });
        return user;
    }

    public async Task<UserEntity> SetIsSubscribedOnAsync(UserEntity userEntity)
    {
        var sql = "UPDATE Users SET IsSubscribed = 1 WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }
    
    public async Task<UserEntity> SetIsSubscribedOffAsync(UserEntity userEntity)
    {
        var sql = "UPDATE Users SET IsSubscribed = 0 WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<List<UserEntity>> GetAllAdmins()
    {
        var sql = "SELECT * FROM Users WHERE IsAdmin = 1";
        var users = await _dbConnection.QueryAsync<UserEntity>(sql);
        return users.ToList();
    }

    public async Task<List<ScheduleOfEventsEntity>> GetAllEventsAsync()
    {
        var sql = "SELECT * From ScheduleOfEvents";
        var events = await _dbConnection.QueryAsync<ScheduleOfEventsEntity>(sql);
        return events.ToList();
    }

    public async Task<UserEventsEntity> RegisterUserForEventAsync(UserEventsEntity userEventsEntity)
    {
        var sql = "INSERT INTO EventEntries (UserId, IdEvent) VALUES (@UserId, @IdEvent)";
        var x = await _dbConnection.ExecuteAsync(sql, userEventsEntity);
        return userEventsEntity;
    }

    public async Task<List<UserEventsEntity>> GetAllRegisteredEventsAsync()
    {
        var sql = "SELECT * From EventEntries";
        var events = await _dbConnection.QueryAsync<UserEventsEntity>(sql);
        return events.ToList();
    }

    public async Task<List<ScheduleOfEventsEntity>> GetUserRegisteredEventsAsync(long userId)
    {
        var sql = @"SELECT soe.Id, soe.NameEvent, soe.DateTimeEvent
                        FROM ScheduleOfEvents soe
                        INNER JOIN EventEntries ee ON soe.Id = ee.IdEvent
                        WHERE ee.UserId = @UserId";

        var events = await _dbConnection.QueryAsync<ScheduleOfEventsEntity>(sql, new { UserId = userId });
        return events.ToList();
    }

    public async Task<bool> UnregisterUserFromEventAsync(long chatId, int eventId)
    {
        const string sql = "DELETE FROM EventEntries WHERE UserId = @UserId AND IdEvent = @EventId";

        var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { UserId = chatId, EventId = eventId });

        return rowsAffected > 0;
    }

    public async Task<List<UserEntity>> GetSubscribedUsersAsync()
    {
        var sql = "SELECT * FROM Users WHERE IsSubscribed = @IsSubscribed";
        var users = await _dbConnection.QueryAsync<UserEntity>(sql, new { IsSubscribed = true });
        return users.AsList();
    }

    public async Task<VotingUsersEntity> RegisterUserVoteAsync(VotingUsersEntity votingUser)
    {
        var sql = @"INSERT INTO VotingUsers (VotingId, UserId, Result, VotingMessageId)
                    VALUES (@VotingId, @UserId, @Result, @VotingMessageId)";
        var x = await _dbConnection.ExecuteAsync(sql, votingUser);
        return votingUser;
    }

    public async Task<VotingUsersEntity> UpdateUserVoteAsync(VotingUsersEntity votingUser)
    {
        var sql = "UPDATE VotingUsers SET Result = @Result WHERE VotingId = @VotingId AND UserId = @UserId";
        var x = await _dbConnection.ExecuteAsync(sql, votingUser);
        return votingUser;
    }

    public async Task<bool> HasUserVotedAsync(long userId, int votingId)
    {
        var sql = "SELECT COUNT(1) FROM VotingUsers WHERE UserId = @UserId AND VotingId = @VotingId";
        var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { UserId = userId, VotingId = votingId });
        return count > 0;
    }

    public async Task<VotingUsersEntity> AddVotingMessageIdAsync(VotingUsersEntity votingUsersEntity)
    {
        var sql =
            "UPDATE VotingUsers SET VotingMessageId = @VotingMessageId WHERE VotingId = @VotingId AND UserId = @UserId";
        var x = await _dbConnection.ExecuteAsync(sql, votingUsersEntity);
        return votingUsersEntity;
    }

    public async Task<VotingUsersEntity> DeleteVotingMessageIdAsync(VotingUsersEntity votingUsersEntity)
    {
        var sql =
            "UPDATE VotingUsers SET VotingMessageId = NULL WHERE VotingId = @VotingId AND UserId = @UserId";
        var x = await _dbConnection.ExecuteAsync(sql, votingUsersEntity);
        return votingUsersEntity;
    }

    public async Task<VotingResultsEntity> GetVotingResultsAsync(int votingId)
    {
        var sql = @"
            SELECT 
                SUM(CASE WHEN Result = 0 THEN 1 ELSE 0 END) AS OptionFirstVotes,
                SUM(CASE WHEN Result = 1 THEN 1 ELSE 0 END) AS OptionSecondVotes
            FROM VotingUsers
            WHERE VotingId = @VotingId";

        return await _dbConnection.QuerySingleAsync<VotingResultsEntity>(sql, new { VotingId = votingId });
    }

    public async Task<int> GetVoteCountByVotingIdAsync(long votingId)
    {
        var sql = "SELECT COUNT(*) FROM VotingUsers WHERE VotingId = @VotingId";
        return await _dbConnection.ExecuteScalarAsync<int>(sql, new { VotingId = votingId });
    }

    public async Task<List<VotingUsersEntity>> GetVotingUsersByVotingIdAsync(long votingId)
    {
        var sql = "SELECT UserId, VotingId, VotingMessageId FROM VotingUsers WHERE VotingId = @VotingId";
        return (await _dbConnection.QueryAsync<VotingUsersEntity>(sql, new { VotingId = votingId })).ToList();
    }

    public async Task<List<UserEntity>> GetUsersByEventIdAsync(int eventId)
    {
        var sql = @"
        SELECT u.Id, u.UserFullName
        FROM Users u
        INNER JOIN EventEntries e ON u.Id = e.UserId
        WHERE e.IdEvent = @IdEvent AND e.IsNotified = 0";
        
        var users = await _dbConnection.QueryAsync<UserEntity>(sql, new { IdEvent = eventId });
        return users.ToList();
    }

    public Task SetStateAdminQuestionEntering(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminQuestionEntering} WHERE Id = @Id";
        return _dbConnection.ExecuteAsync(sql, userEntity);
    }

    public Task SetStateAdminVariantEntering(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminVariantEntering} WHERE Id = @Id";
        return _dbConnection.ExecuteAsync(sql, userEntity);
    }

    public Task SetEditingQuizQuestionIdForAdmin(UserEntity userEntity, int? questionId)
    {
        var sql = "UPDATE Users SET EditingQuizQuestionId = @QuestionId WHERE Id = @Id";
        return _dbConnection.ExecuteAsync(sql, new { QuestionId = questionId, Id = userEntity.Id });
    }

    public Task<long?> GetEditingQuizQuestionIdForAdmin(long userId)
    {
        var sql = "SELECT EditingQuizQuestionId FROM Users WHERE Id = @Id";
        return _dbConnection.QueryFirstOrDefaultAsync<long?>(sql, new { Id = userId });
    }
}
