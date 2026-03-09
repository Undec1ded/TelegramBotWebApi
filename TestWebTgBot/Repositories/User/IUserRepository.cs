using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public interface IUserRepository
{
    Task<UserEntity> CreateUserAsync(UserEntity userEntity);
    
    Task<UserEntity> SetUserAdminAsync(UserEntity userEntity);

    Task<UserEntity> SetStateUserFullName(UserEntity userEntity);
    
    Task<UserEntity> SetStateDefault(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitUserQuestion(UserEntity userEntity);

    Task<UserEntity> SetUserFullName(UserEntity userEntity);
    
    Task<UserEntity?> GetUserByTelegramId(long telegramId);

    Task<UserEntity> SetIsSubscribedOnAsync(UserEntity userEntity);
    
    Task<UserEntity> SetIsSubscribedOffAsync(UserEntity userEntity);
    Task<List<UserEntity>> GetAllAdmins();

    Task<List<ScheduleOfEventsEntity>> GetAllEventsAsync();
    
    Task<UserEventsEntity> RegisterUserForEventAsync(UserEventsEntity userEventsEntity);

    Task<List<UserEventsEntity>> GetAllRegisteredEventsAsync();

    Task<List<ScheduleOfEventsEntity>> GetUserRegisteredEventsAsync(long userId);

    Task<bool> UnregisterUserFromEventAsync(long chatId, int eventId);
    
    Task<List<UserEntity>> GetSubscribedUsersAsync();

    Task<VotingUsersEntity> RegisterUserVoteAsync(VotingUsersEntity votingUser);

    Task<VotingUsersEntity> UpdateUserVoteAsync(VotingUsersEntity votingUser);
    
    Task<bool> HasUserVotedAsync(long userId, int votingId);

    Task<VotingUsersEntity> AddVotingMessageIdAsync(VotingUsersEntity votingUsersEntity);
    
    Task<VotingUsersEntity> DeleteVotingMessageIdAsync(VotingUsersEntity votingUsersEntity);

    Task<VotingResultsEntity> GetVotingResultsAsync(int votingId);
    
    Task<int> GetVoteCountByVotingIdAsync(long votingId);
    
    Task<List<VotingUsersEntity>> GetVotingUsersByVotingIdAsync(long votingId);
    
    Task<List<UserEntity>> GetUsersByEventIdAsync(int eventId);


    Task SetStateAdminQuestionEntering(UserEntity userEntity);
    Task SetStateAdminVariantEntering(UserEntity userEntity);
    Task SetEditingQuizQuestionIdForAdmin(UserEntity userEntity, int? questionId);
    Task<long?> GetEditingQuizQuestionIdForAdmin(long userId);
}
