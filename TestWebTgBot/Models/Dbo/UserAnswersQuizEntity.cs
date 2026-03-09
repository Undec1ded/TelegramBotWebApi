namespace TestWebTgBot.Models.Dbo;


public class UserAnswersQuizEntity
{
    public int Id { get; set; } 

    public int QuizId { get; set; }

    public long UserId { get; set; }

    public int TotalPoints { get; set; } = 0;

    public int? EndQuizMessageId { get; set; } 

    public int? Answer1 { get; set; }
    public int? Question1MessageId { get; set; } 

    public int? Answer2 { get; set; }
    public int? Question2MessageId { get; set; } 

    public int? Answer3 { get; set; } 
    public int? Question3MessageId { get; set; } 

    public int? Answer4 { get; set; }
    public int? Question4MessageId { get; set; } 

    public int? Answer5 { get; set; }
    public int? Question5MessageId { get; set; } 

    public int? Answer6 { get; set; }
    public int? Question6MessageId { get; set; } 

    public int? Answer7 { get; set; }
    public int? Question7MessageId { get; set; } 

    public int? Answer8 { get; set; }
    public int? Question8MessageId { get; set; } 

    public int? Answer9 { get; set; } 
    public int? Question9MessageId { get; set; } 

    public int? Answer10 { get; set; }
    public int? Question10MessageId { get; set; }

    public int? Answer11 { get; set; }
    public int? Question11MessageId { get; set; }

    public int? Answer12 { get; set; } 
    public int? Question12MessageId { get; set; }
}
