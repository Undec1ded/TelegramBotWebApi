using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Repositories.Admin;
using TestWebTgBot.Repositories.User;

namespace TestWebTgBot.Services.Admin;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IQuizAdminRepository _quizAdminRepository;

    public AdminService(
        IAdminRepository adminRepository,
        IQuestionsRepository questionsRepository,
        IQuizAdminRepository quizAdminRepository)
    {
        _adminRepository = adminRepository;
        _questionsRepository = questionsRepository;
        _quizAdminRepository = quizAdminRepository;
    }
    public Task<ScheduleOfEventsEntity> AddNewEventAsync(ScheduleOfEventsEntity scheduleOfEventsEntity)
    {
        return _adminRepository.AddNewEventAsync(scheduleOfEventsEntity);
    }

    public Task<UserEntity> SetStateDefaultAsync(UserEntity userEntity)
    {
        return _adminRepository.SetStateDefaultAsync(userEntity);
    }

    public Task<UserEntity> SetStateWaitNewEventAsync(UserEntity userEntity)
    {
        return _adminRepository.SetStateWaitNewEventAsync(userEntity);
    }

    public Task<UserEntity> SetStateWaitAdminAddNewsAsync(UserEntity userEntity)
    {
        return _adminRepository.SetStateWaitAdminAddNewsAsync(userEntity);
    }

    public Task<UserEntity> SetStateWaitAdminAddVotingQuestionAsync(UserEntity userEntity)
    {
        return _adminRepository.SetStateWaitAdminAddVotingQuestionAsync(userEntity);
    }

    public Task<UserEntity> SetStateWaitAdminAddFirstQuestionAsync(UserEntity userEntity)
    {
        return _adminRepository.SetStateWaitAdminAddFirstQuestionAsync(userEntity);
    }

    public Task<UserEntity> SetStateWaitAdminAddSecondQuestionAsync(UserEntity userEntity)
    {
        return _adminRepository.SetStateWaitAdminAddSecondQuestionAsync(userEntity);
    }

    public Task<List<UserQuestionEntity>> GetQuestionsAsync()
    {
        return _questionsRepository.GetQuestionsAsync();
    }

    public Task<QuestionEntity> DeleteQuestionsAsync(QuestionEntity questionEntity)
    {
        return _questionsRepository.DeleteQuestionsAsync(questionEntity);
    }

    public Task SetQuizStartAsync()
    {
        return _adminRepository.SetQuizStartAsync();
    }

    public Task SetQuizEndAsync()
    {
        return _adminRepository.SetQuizEndAsync();
    }

    public Task<VotingEntity> CreateVotingAsync(VotingEntity votingEntity)
    {
        return _adminRepository.CreateVotingAsync(votingEntity);
    }

    public Task<VotingEntity> SetVotingStartAsync(VotingEntity votingEntity)
    {
        return _adminRepository.SetVotingStartAsync(votingEntity);
    }

    public Task<VotingEntity> SetVotingEndAsync(VotingEntity votingEntity)
    {
        return _adminRepository.SetVotingEndAsync(votingEntity);
    }

    public Task<List<VotingEntity>> GetAllVotingResultsAsync()
    {
        return _adminRepository.GetAllVotingResultsAsync();
    }

    public Task<List<VotingUsersEntity>> GetVotesByVotingIdAsync(int votingId)
    {
        return _adminRepository.GetVotesByVotingIdAsync(votingId);
    }

    public Task<AdminPasswordEntity> CreateAdminPasswordAsync(AdminPasswordEntity adminPasswordEntity)
    {
        return _adminRepository.CreateAdminPasswordAsync(adminPasswordEntity);
    }

    public Task DeletePasswordByDateAsync(DateTime dateCreated)
    {
        return _adminRepository.DeletePasswordByDateAsync(dateCreated);
    }

    public Task DeletePasswordAsync(string password)
    {
        return _adminRepository.DeletePasswordAsync(password);
    }

    public Task<List<AdminPasswordEntity>> GetAllAdminPasswordsAsync()
    {
        return _adminRepository.GetAllAdminPasswordsAsync();
    }

    public Task<QuizAdminEntity> StartQuizAsync(QuizAdminEntity quizAdminEntity)
    {
        return _quizAdminRepository.StartQuizAsync(quizAdminEntity);
    }

    public Task<QuizAdminEntity> EndLastQuizAsync()
    {
        return _quizAdminRepository.EndLastQuizAsync();
    }

    public Task<List<ResultQuizEntity>> GetResultsForLastTwoQuizzesAsync()
    {
        return _quizAdminRepository.GetResultsForLastTwoQuizzesAsync();
    }

    public Task<int?> GetLastQuizIdAsync()
    {
        return _quizAdminRepository.GetLastQuizIdAsync();
    }

    public Task<bool> IsLastQuizActiveAsync()
    {
        return _quizAdminRepository.IsLastQuizActiveAsync();
    }
}