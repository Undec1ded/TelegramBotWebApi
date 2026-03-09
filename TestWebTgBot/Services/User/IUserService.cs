using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Services.User;

public interface IUserService
{
    Task<UserEntity> CreateUserIfNotExistAsync (UserEntity userEntity);
    
    Task<UserEntity> SetUserAdminAsync(UserEntity userEntity);

    Task<UserEntity> SetStateUserFullName(UserEntity userEntity);
    
    Task<UserEntity> SetStateDefault(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitUserQuestion(UserEntity userEntity);
    
    Task<UserEntity> SetUserFullName(UserEntity userEntity);
    
    Task<UserEntity> SetIsSubscribedOnAsync(UserEntity userEntity);
    
    Task<UserEntity> SetIsSubscribedOffAsync(UserEntity userEntity);
    
    Task<UserEntity?> GetUserByTelegramId(long telegramId);
    
    Task<List<UserEntity>> GetAllAdmins();

    Task<QuestionEntity> CreateQuestionAsync(QuestionEntity questionEntity);
    
    Task<LinksSocialNetworksEntity?> GetLinkEntityByNameAsync(string linkName);
    
    Task<List<ScheduleOfEventsEntity>> GetAllEventsAsync();
    
    Task<UserEventsEntity> RegisterUserForEventAsync(UserEventsEntity userEventsEntity);
    
    Task<List<UserEventsEntity>> GetAllRegisteredEventsAsync();
    
    Task<List<ScheduleOfEventsEntity>> GetUserRegisteredEventsAsync(long userId);
    
    Task<bool> UnregisterUserFromEventAsync(long chatId, int eventId);
    
    Task<List<UserEntity>> GetSubscribedUsersAsync();
    
    Task<List<VotingEntity>> GetAllVotingResultsAsync();
    
    Task<VotingUsersEntity> RegisterUserVoteAsync(VotingUsersEntity votingUser);

    Task<VotingUsersEntity> UpdateUserVoteAsync(VotingUsersEntity votingUser);
    
    Task<VotingUsersEntity> AddVotingMessageIdAsync(VotingUsersEntity votingUsersEntity);
    
    Task<VotingUsersEntity> DeleteVotingMessageIdAsync(VotingUsersEntity votingUsersEntity);
    
    Task<bool> HasUserVotedAsync(long userId, int votingId);
    
    Task<VotingResultsEntity> GetVotingResultsAsync(int votingId);
    
    Task<int> GetVoteCountByVotingIdAsync(long votingId);
    
    Task<List<VotingUsersEntity>> GetVotingUsersByVotingIdAsync(long votingId);
    
    Task<List<UserEntity>> GetUsersByEventIdAsync(int eventId);
    
    Task SetIsNotifiedAsync(int eventId);
    
    Task<int?> GetLastQuizIdAsync();
    
    Task<UserAnswersQuizEntity> CreateUserQuizAsync(UserAnswersQuizEntity userAnswersQuizEntity);

    Task UpdateUserQuizDataAsync(UserAnswersQuizEntity userAnswersQuizEntity);
    
    Task<bool> IsLastQuizActiveAsync();
    
    Task<bool> HasUserParticipatedInLastQuizAsync(long userId, int quizId);
    
    Task<List<int>> GetQuestionMessageIdsAsync(int quizId, long userId);
    
    Task<UserAnswersQuizEntity?> GetUserQuizDataAsync(int quizId, long userId);
    Task SetStateAdminQuestionEntering(UserEntity userEntity);
    Task SetStateAdminVariantEntering(UserEntity userEntity, int questionId);
    Task<long?> GetEditingQuizQuestionIdForAdmin(long userId);
}
