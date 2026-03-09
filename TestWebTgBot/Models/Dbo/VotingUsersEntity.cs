namespace TestWebTgBot.Models.Dbo;

public class VotingUsersEntity
{
    public int Id { get; set; }

    public int VotingId { get; set; }
    
    public long UserId { get; set; }
    
    public bool Result { get; set; }

    public int? VotingMessageId  { get; set; }
}