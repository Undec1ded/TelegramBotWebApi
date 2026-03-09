using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Repositories.Admin;
using TestWebTgBot.Repositories.User;

namespace TestWebTgBot.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILinksSocialNetworksRepository _linksSocialNetworksRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly IEventEntriesRepository _eventEntriesRepository;
    private readonly IQuizAdminRepository _quizAdminRepository;
    private readonly IQuizUsersRepository _quizUsersRepository;

    public UserService(
        IUserRepository userRepository,
        IQuestionsRepository questionsRepository,
        ILinksSocialNetworksRepository linksSocialNetworksRepository,
        IAdminRepository adminRepository,
        IEventEntriesRepository eventEntriesRepository,
        IQuizAdminRepository quizAdminRepository,
        IQuizUsersRepository quizUsersRepository)
    {
        _userRepository = userRepository;
        _questionsRepository = questionsRepository;
        _linksSocialNetworksRepository = linksSocialNetworksRepository;
        _adminRepository = adminRepository;
        _eventEntriesRepository = eventEntriesRepository;
        _quizAdminRepository = quizAdminRepository;
        _quizUsersRepository = quizUsersRepository;
    }
    public async Task<UserEntity> CreateUserIfNotExistAsync(UserEntity userEntity)
    {
        var user = await _userRepository.GetUserByTelegramId(userEntity.Id);
        if (user !=null)
        {
            return user;
        }

        return await _userRepository.CreateUserAsync(userEntity);
    }
    
    
    public Task<UserEntity> SetUserAdminAsync(UserEntity userEntity)
    {
        return _userRepository.SetUserAdminAsync(userEntity);
    }

    public Task<UserEntity> SetStateUserFullName(UserEntity userEntity)
    {
        return _userRepository.SetStateUserFullName(userEntity);
    }

    public async Task<UserEntity> SetStateDefault(UserEntity userEntity)
    {
        await _userRepository.SetEditingQuizQuestionIdForAdmin(userEntity, null);
        var result = await _userRepository.SetStateDefault(userEntity);
        return result;
    }

    public Task<UserEntity> SetStateWaitUserQuestion(UserEntity userEntity)
    {
        return _userRepository.SetStateWaitUserQuestion(userEntity);
    }

    public Task<UserEntity> SetUserFullName(UserEntity userEntity)
    {
        return _userRepository.SetUserFullName(userEntity);
    }

    public Task<UserEntity> SetIsSubscribedOnAsync(UserEntity userEntity)
    {
        return _userRepository.SetIsSubscribedOnAsync(userEntity);
    }
    
    public Task<UserEntity> SetIsSubscribedOffAsync(UserEntity userEntity)
    {
        return _userRepository.SetIsSubscribedOffAsync(userEntity);
    }

    public Task<UserEntity?> GetUserByTelegramId(long telegramId)
    {
        return _userRepository.GetUserByTelegramId(telegramId);
    }

    public Task<List<UserEntity>> GetAllAdmins()
    {
        return _userRepository.GetAllAdmins();
    }

    public Task<QuestionEntity> CreateQuestionAsync(QuestionEntity questionEntity)
    {
        return _questionsRepository.CreateQuestionAsync(questionEntity);
    }

    public Task<LinksSocialNetworksEntity?> GetLinkEntityByNameAsync(string linkName)
    {
        return _linksSocialNetworksRepository.GetLinkEntityByNameAsync(linkName);
    }

    public Task<List<ScheduleOfEventsEntity>> GetAllEventsAsync()
    {
        return _userRepository.GetAllEventsAsync();
    }

    public Task<UserEventsEntity> RegisterUserForEventAsync(UserEventsEntity userEventsEntity)
    {
        return _userRepository.RegisterUserForEventAsync(userEventsEntity);
    }

    public Task<List<UserEventsEntity>> GetAllRegisteredEventsAsync()
    {
        return _userRepository.GetAllRegisteredEventsAsync();
    }

    public Task<List<ScheduleOfEventsEntity>> GetUserRegisteredEventsAsync(long userId)
    {
        return _userRepository.GetUserRegisteredEventsAsync(userId);
    }

    public Task<bool> UnregisterUserFromEventAsync(long chatId, int eventId)
    {
        return _userRepository.UnregisterUserFromEventAsync(chatId, eventId);
    }

    public Task<List<UserEntity>> GetSubscribedUsersAsync()
    {
        return _userRepository.GetSubscribedUsersAsync();
    }

    public Task<List<VotingEntity>> GetAllVotingResultsAsync()
    {
        return _adminRepository.GetAllVotingResultsAsync();
    }

    public Task<VotingUsersEntity> RegisterUserVoteAsync(VotingUsersEntity votingUser)
    {
        return _userRepository.RegisterUserVoteAsync(votingUser);
    }

    public Task<VotingUsersEntity> UpdateUserVoteAsync(VotingUsersEntity votingUser)
    {
        return _userRepository.UpdateUserVoteAsync(votingUser);
    }

    public Task<VotingUsersEntity> AddVotingMessageIdAsync(VotingUsersEntity votingUsersEntity)
    {
        return _userRepository.AddVotingMessageIdAsync(votingUsersEntity);
    }

    public Task<VotingUsersEntity> DeleteVotingMessageIdAsync(VotingUsersEntity votingUsersEntity)
    {
        return _userRepository.DeleteVotingMessageIdAsync(votingUsersEntity);
    }

    public async Task<bool> HasUserVotedAsync(long userId, int votingId)
    {
        return await _userRepository.HasUserVotedAsync(userId, votingId);
    }

    public Task<VotingResultsEntity> GetVotingResultsAsync(int votingId)
    {
        return _userRepository.GetVotingResultsAsync(votingId);
    }

    public Task<int> GetVoteCountByVotingIdAsync(long votingId)
    {
        return _userRepository.GetVoteCountByVotingIdAsync(votingId);
    }

    public Task<List<VotingUsersEntity>> GetVotingUsersByVotingIdAsync(long votingId)
    {
        return _userRepository.GetVotingUsersByVotingIdAsync(votingId);
    }

    public Task<List<UserEntity>> GetUsersByEventIdAsync(int eventId)
    {
        return _userRepository.GetUsersByEventIdAsync(eventId);
    }

    public Task SetIsNotifiedAsync(int eventId)
    {
        return _eventEntriesRepository.SetIsNotifiedAsync(eventId);
    }

    public Task<int?> GetLastQuizIdAsync()
    {
        return _quizAdminRepository.GetLastQuizIdAsync();
    }

    public Task<UserAnswersQuizEntity> CreateUserQuizAsync(UserAnswersQuizEntity userAnswersQuizEntity)
    {
        return _quizUsersRepository.CreateUserQuizAsync(userAnswersQuizEntity);
    }

    public Task UpdateUserQuizDataAsync(UserAnswersQuizEntity userAnswersQuizEntity)
    {
        return _quizUsersRepository.UpdateUserQuizDataAsync(userAnswersQuizEntity);
    }

    public Task<bool> IsLastQuizActiveAsync()
    {
        return _quizAdminRepository.IsLastQuizActiveAsync();
    }

    public Task<bool> HasUserParticipatedInLastQuizAsync(long userId, int quizId)
    {
        return _quizUsersRepository.HasUserParticipatedInLastQuizAsync(userId, quizId);
    }

    public Task<List<int>> GetQuestionMessageIdsAsync(int quizId, long userId)
    {
        return _quizUsersRepository.GetQuestionMessageIdsAsync(quizId, userId);
    }

    public Task<UserAnswersQuizEntity?> GetUserQuizDataAsync(int quizId, long userId)
    {
        return _quizUsersRepository.GetUserQuizDataAsync(quizId, userId);
    }

    public Task SetStateAdminQuestionEntering(UserEntity userEntity)
    {
        return _userRepository.SetStateAdminQuestionEntering(userEntity);
    }

    public async Task SetStateAdminVariantEntering(UserEntity userEntity, int questionId)
    {
        await _userRepository.SetStateAdminVariantEntering(userEntity);
        await _userRepository.SetEditingQuizQuestionIdForAdmin(userEntity, questionId);
    }

    public Task<long?> GetEditingQuizQuestionIdForAdmin(long userId)
    {
        return _userRepository.GetEditingQuizQuestionIdForAdmin(userId);
    }
}
