using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Services.Admin;

public interface IAdminService
{
    Task<ScheduleOfEventsEntity> AddNewEventAsync(ScheduleOfEventsEntity scheduleOfEventsEntity);
    
    Task<UserEntity> SetStateDefaultAsync(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitNewEventAsync(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitAdminAddNewsAsync(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitAdminAddVotingQuestionAsync(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitAdminAddFirstQuestionAsync(UserEntity userEntity);
    
    Task<UserEntity> SetStateWaitAdminAddSecondQuestionAsync(UserEntity userEntity);
    
    Task<List<UserQuestionEntity>> GetQuestionsAsync();
    
    Task<QuestionEntity> DeleteQuestionsAsync(QuestionEntity questionEntity);

    Task SetQuizStartAsync();
    
    Task SetQuizEndAsync();
    
    Task<VotingEntity> CreateVotingAsync(VotingEntity votingEntity);
    
    Task<VotingEntity> SetVotingStartAsync(VotingEntity votingEntity);
    
    Task<VotingEntity> SetVotingEndAsync(VotingEntity votingEntity);
    
    Task<List<VotingEntity>> GetAllVotingResultsAsync();
    
    Task<List<VotingUsersEntity>> GetVotesByVotingIdAsync(int votingId);

    Task<AdminPasswordEntity> CreateAdminPasswordAsync(AdminPasswordEntity adminPasswordEntity);
    
    Task DeletePasswordByDateAsync(DateTime dateCreated);
    
    Task DeletePasswordAsync(string password);
    
    //quiz
    Task<List<AdminPasswordEntity>> GetAllAdminPasswordsAsync();
    
    Task<QuizAdminEntity> StartQuizAsync(QuizAdminEntity quizAdminEntity);

    Task<QuizAdminEntity> EndLastQuizAsync();

    Task<List<ResultQuizEntity>> GetResultsForLastTwoQuizzesAsync();

    Task<int?> GetLastQuizIdAsync();
    
    Task<bool> IsLastQuizActiveAsync();

}