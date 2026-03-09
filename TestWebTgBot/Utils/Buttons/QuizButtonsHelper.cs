using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.Utils.Buttons;

public static class QuizButtonsHelper
{
    //UserButtons
    public static InlineKeyboardMarkup EndQuizContextButtons()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {_endUserQuizButton}
            }
        };
    }
    public static InlineKeyboardMarkup QuestionBody(
        string numberQuestion,
        string optionAnswer1,
        string optionAnswer2,
        string optionAnswer3,
        string optionAnswer4 )
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {QuestionButton(optionAnswer1, numberQuestion,"1" ) },
                new List<InlineKeyboardButton> {QuestionButton(optionAnswer2, numberQuestion,"2" ) },
                new List<InlineKeyboardButton> {QuestionButton(optionAnswer3, numberQuestion,"3" ) },
                new List<InlineKeyboardButton> {QuestionButton(optionAnswer4, numberQuestion,"4" ) },
            }
        };
    }
    
    public static InlineKeyboardMarkup RemakeUserButton(string numberQuestion)
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {RemakeButton(numberQuestion)}
            }
        };
    }
    
    public static InlineKeyboardMarkup QuizStartContextButtons()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {_startQuizButton , _goToUserMenuButton}
            }
        };
    }
    
    public static InlineKeyboardMarkup GoToMainUserMenuButton()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {_goToUserMenuButton}
            }
        };
    }
    //Button
    //Quiz
    private static InlineKeyboardButton QuestionButton(string text, string numberQuestion, string numberAnswer)
    {
        return new InlineKeyboardButton
        {
            Text = text,
            CallbackData = $"QuizAnswerUser_{numberQuestion}|{numberAnswer}"
        };
    }
    
    private static InlineKeyboardButton RemakeButton(string numberQuestion)
    {
        return new InlineKeyboardButton
        {
            Text = "Выбрать другой вариант",
            CallbackData = $"ChooseAnotherOption_{numberQuestion}"
        };
    }
    
    //Utils
    private static readonly InlineKeyboardButton _startQuizButton = new InlineKeyboardButton
    {
        Text = "Начать",
        CallbackData = "UserStartQuizNow"
    };
    
    private static readonly InlineKeyboardButton _goToUserMenuButton = new InlineKeyboardButton
    {
        Text = "Меню пользователя",
        CallbackData = "StartMenu"
    };
    
    private static readonly InlineKeyboardButton _endUserQuizButton = new InlineKeyboardButton
    {
        Text = "Закончить",
        CallbackData = "EndUserQuiz"
    };
  
}
