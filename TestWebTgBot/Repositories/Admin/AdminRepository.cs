using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;

namespace TestWebTgBot.Repositories.Admin;

public class AdminRepository : IAdminRepository
{
    private readonly IDbConnection _dbConnection;

    public AdminRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ScheduleOfEventsEntity> AddNewEventAsync(ScheduleOfEventsEntity scheduleOfEventsEntity)
    {
        var sql = "INSERT INTO ScheduleOfEvents(NameEvent, DateTimeEvent) VALUES(@NameEvent, @DateTimeEvent)";
        var x = await _dbConnection.ExecuteAsync(sql, scheduleOfEventsEntity);
        return scheduleOfEventsEntity;
    }

    public async Task<UserEntity> SetStateDefaultAsync(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.Default} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateWaitNewEventAsync(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminNewEvent} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateWaitAdminAddNewsAsync(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminAddNews} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateWaitAdminAddVotingQuestionAsync(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminAddVotingQuestion} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateWaitAdminAddFirstQuestionAsync(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminAddFirstQuestion} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task<UserEntity> SetStateWaitAdminAddSecondQuestionAsync(UserEntity userEntity)
    {
        var sql = $"UPDATE Users SET UserChatState = {(short)UserChatStates.WaitAdminAddSecondQuestion} WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, userEntity);
        return userEntity;
    }

    public async Task SetQuizStartAsync()
    {
        var sql = @"UPDATE Users SET IsQuizStart = TRUE;";

        await _dbConnection.ExecuteAsync(sql);
    }

    public async Task SetQuizEndAsync()
    {
        var sql = @"UPDATE Users SET IsQuizStart = FALSE;";

        await _dbConnection.ExecuteAsync(sql);
    }

    public async Task<VotingEntity> CreateVotingAsync(VotingEntity votingEntity)
    {
        const string sql = @"
                INSERT INTO Voting (Question, OptionFirst, OptionSecond, Result)
                VALUES (@Question, @OptionFirst, @OptionSecond, @Result)";
        var x = await _dbConnection.ExecuteAsync(sql, votingEntity);
        return votingEntity;
    }

    public async Task<VotingEntity> SetVotingStartAsync(VotingEntity votingEntity)
    {
        var sql = "UPDATE Voting SET IsStart = 1 WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, votingEntity);
        return votingEntity;
    }

    public async Task<VotingEntity> SetVotingEndAsync(VotingEntity votingEntity)
    {
        var sql = "UPDATE Voting SET IsStart = 0 WHERE Id = @Id";
        var x = await _dbConnection.ExecuteAsync(sql, votingEntity);
        return votingEntity;
    }

    public async Task<List<VotingEntity>> GetAllVotingResultsAsync()
    {
        var sql = "SELECT Id, Question, OptionFirst, OptionSecond, Result, IsStart FROM Voting";
        var results = await _dbConnection.QueryAsync<VotingEntity>(sql);
        return results.AsList();
    }

    public async Task<List<VotingUsersEntity>> GetVotesByVotingIdAsync(int votingId)
    {
        var sql = "SELECT * FROM VotingUsers WHERE VotingId = @VotingId";
        var votes = await _dbConnection.QueryAsync<VotingUsersEntity>(sql, new { VotingId = votingId });
        return votes.ToList();
    }

    public async Task<AdminPasswordEntity> CreateAdminPasswordAsync(AdminPasswordEntity adminPasswordEntity)
    {
        const string sql = @"
                INSERT INTO AdminPasswords (Password, TimeCreated)
                VALUES (@Password, @TimeCreated)";
        var x = await _dbConnection.ExecuteAsync(sql, adminPasswordEntity);
        return adminPasswordEntity;
    }
    
    public async Task DeletePasswordByDateAsync(DateTime dateCreated)
    {
        const string sql = @"
                DELETE FROM AdminPasswords
                WHERE TimeCreated = @TimeCreated";
        await _dbConnection.ExecuteAsync(sql, new { TimeCreated = dateCreated });
    }

    public async Task DeletePasswordAsync(string password)
    {
        const string sql = @"
                DELETE FROM AdminPasswords
                WHERE Password = @Password";
        await _dbConnection.ExecuteAsync(sql, new { Password = password });
    }
    
    public async Task<List<AdminPasswordEntity>> GetAllAdminPasswordsAsync()
    {
        const string sql = @"
                SELECT Id, Password, TimeCreated
                FROM AdminPasswords";

        var passwords = await _dbConnection.QueryAsync<AdminPasswordEntity>(sql);
        return passwords.AsList();
    }
}