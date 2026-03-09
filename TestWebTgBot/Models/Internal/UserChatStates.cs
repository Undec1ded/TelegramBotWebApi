namespace TestWebTgBot.Models.Internal;

public enum UserChatStates
{
    Default = 0,
    WaitUserFullName = 1,
    WaitUserQuestion = 2,
    
    //admin 
    WaitAdminNewEvent = 100,
    WaitAdminAddNews = 101,
    WaitAdminAddVotingQuestion = 102,
    WaitAdminAddFirstQuestion = 103,
    WaitAdminAddSecondQuestion = 104,
    WaitAdminQuestionEntering = 105,
    WaitAdminVariantEntering = 106,
}
