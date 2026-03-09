using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.Utils.Buttons;

public static class AdminButtonsHelper
{
    public static InlineKeyboardMarkup ButtonsGoToQuizMenu()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonAdminQuizMain}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsGoToMainVoting()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonOnlineVoting}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsNoVotingContext()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonCreateVoting , _buttonResultVoting},
                new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsAdminVotingContext(bool isStart)
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { ButtonsStartVoting(isStart)},
                new List<InlineKeyboardButton> { _buttonResultVoting, _buttonCreateVoting},
                new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel},
            }
        };
    }
    public static InlineKeyboardMarkup CreateQuizMenu(bool isQuizStart)
    {
        var result = new InlineKeyboardMarkup();
        result.InlineKeyboard = new List<List<InlineKeyboardButton>>();
        result.InlineKeyboard.Add(new List<InlineKeyboardButton> { ButtonStartEndQuiz(isQuizStart) });
        result.InlineKeyboard.Add(new List<InlineKeyboardButton> { _buttonResultsQuiz });
        if (!isQuizStart)
        {
            result.InlineKeyboard.Add(new List<InlineKeyboardButton> { _buttonViewEditQuiz });
        }

        result.InlineKeyboard.Add(new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel });

        return result;
    }
    
    public static InlineKeyboardMarkup ButtonsAdminQuestionsDeleteCheck()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonDeleteQuestions},
                new List<InlineKeyboardButton> { _buttonAdminQuestionsToSpeaker}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsAdminQuestionsContext()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonDeleteQuestionsCheck},
                new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsAdminAddEventContext()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel, _buttonAdminAddEvent}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonGoToAdminStartPanel()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel }
            }
        };
    }
    public static InlineKeyboardMarkup CrateAdminPanel()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonAdminQuizMain },
                new List<InlineKeyboardButton> { _buttonAdminAddEvent , _buttonAndNews},
                new List<InlineKeyboardButton> { _buttonAdminQuestionsToSpeaker , _buttonOnlineVoting },
                new List<InlineKeyboardButton> { _buttonAdminAddAdministrator },
                new List<InlineKeyboardButton> { _buttonAdminUserMenu }
            }
        };
    }

    public static InlineKeyboardMarkup CreateEditQuizMenu()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonAddQuestion },
                new List<InlineKeyboardButton> { _buttonEditQuestion },
                new List<InlineKeyboardButton> { _buttonGoToAdminStartPanel }
            }
        };
    }

    public static InlineKeyboardMarkup CreateEditQuizQuestionsMenu(QuizEntity quizEntity)
    {
        var result = new InlineKeyboardMarkup();
        result.InlineKeyboard = new List<List<InlineKeyboardButton>>();

        foreach (var question in quizEntity.Questions!)
        {
            result.InlineKeyboard.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton
            {
                Text = question.Question,
                CallbackData = $"EditQuizQuestion_{question.Id}"
            }});
        }

        return result;
    }

    public static InlineKeyboardMarkup CreateEditQuestionMenu(QuizQuestionEntity question)
    {
        var result = new InlineKeyboardMarkup();
        result.InlineKeyboard = new List<List<InlineKeyboardButton>>();

        result.InlineKeyboard.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton
        {
            Text = "Добавить вариант ответа",
            CallbackData = $"AddQuestionVariant_{question.Id}"
        }});

        if (question.Variants is not null && question.Variants.Count > 0)
        {
            result.InlineKeyboard.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton
            {
                Text = "Удалить вариант ответа",
                CallbackData = $"DeleteQuestionVariants_{question.Id}"
            }});
        }

        result.InlineKeyboard.Add(new List<InlineKeyboardButton> { new InlineKeyboardButton
        {
            Text = "Удалить вопрос",
            CallbackData = $"DeleteQuestion_{question.Id}"
        }});
        result.InlineKeyboard.Add(new List<InlineKeyboardButton> { _buttonAdminQuizMain });

        return result;
    }

    public static InlineKeyboardMarkup CreateDeleteVariantsButtons(QuizQuestionEntity question)
    {
        var result = new InlineKeyboardMarkup();
        result.InlineKeyboard = new List<List<InlineKeyboardButton>>();

        if (question.Variants is not null)
        {
            for (int i = 0; i < question.Variants.Count; i++)
            {
                if (i % 2 == 0)
                {
                    result.InlineKeyboard.Add(new List<InlineKeyboardButton>());
                }

                result.InlineKeyboard[^1].Add(new InlineKeyboardButton
                {
                    Text = question.Variants[i].Text,
                    CallbackData = $"Dqv_{question.Variants[i].Id}_{question.Id}"
                });
            }
        }

        return result;
    }

    //Quiz buttons
    private static  InlineKeyboardButton ButtonStartEndQuiz (bool isQuizStart)
    {
        return new InlineKeyboardButton
        {
            Text = isQuizStart ? "End Quiz" : "Start Quiz",
            CallbackData = isQuizStart ? "EndQuiz" : "StartQuiz"
        };
    }

    private static readonly  InlineKeyboardButton _buttonAddQuestion = new InlineKeyboardButton
    {
        Text = "Добавить вопрос",
        CallbackData = "AddQuestion"
    };

    private static readonly  InlineKeyboardButton _buttonEditQuestion = new InlineKeyboardButton
    {
        Text = "Редактировать вопрос",
        CallbackData = "EditQuizQuestion"
    };

    private static readonly  InlineKeyboardButton _buttonResultsQuiz = new InlineKeyboardButton 
    {
        Text = "Результаты Квиза",
        CallbackData = "ResultsQuiz"
    };

    private static readonly  InlineKeyboardButton _buttonViewEditQuiz = new InlineKeyboardButton
    {
        Text = "Просмотреть/Редактировать Квиз",
        CallbackData = "ViewEditQuiz"
    };
    
    //Admin main buttons
    private static  InlineKeyboardButton ButtonsStartVoting (bool isQuizStart)
    {
        return new InlineKeyboardButton
        {
            Text = isQuizStart ? "Закончить голосование" : "Начать голосование",
            CallbackData = isQuizStart ? "EndVotingAdmin" : "StartVotingAdmin"
        };
    }
    private static readonly InlineKeyboardButton _buttonOnlineVoting = new InlineKeyboardButton
    {
        Text = "Меню онлайн голосования",
        CallbackData = "OnlineVotingAdmin"
    };
   
    private static readonly InlineKeyboardButton _buttonResultVoting = new InlineKeyboardButton
    {
        Text = "Результаты голосований",
        CallbackData = "ResultVotingAdmin"
    };
    private static readonly InlineKeyboardButton _buttonCreateVoting = new InlineKeyboardButton
    {
        Text = "Создать голосование",
        CallbackData = "CreateVotingAdmin"
    };
    private static readonly InlineKeyboardButton _buttonAndNews = new InlineKeyboardButton
    {
        Text = "Добавить новость",
        CallbackData = "AndNews"
    };
    private static readonly  InlineKeyboardButton _buttonGoToAdminStartPanel = new InlineKeyboardButton 
    {
        Text = "Меню администратора",
        CallbackData = "GoToAdminStartPanel"
    };
    private static readonly  InlineKeyboardButton _buttonDeleteQuestions = new InlineKeyboardButton 
    {
        Text = "Отчистить",
        CallbackData = "DeleteQuestions"
    };
    private static readonly  InlineKeyboardButton _buttonDeleteQuestionsCheck = new InlineKeyboardButton 
    {
        Text = "Отчистить список вопросов",
        CallbackData = "DeleteQuestionsCheck"
    };
    
    private static readonly  InlineKeyboardButton _buttonAdminQuizMain = new InlineKeyboardButton 
    {
        Text = "Меню упраления квизом",
        CallbackData = "AdminQuizMain"
    };
    private static readonly  InlineKeyboardButton _buttonAdminAddEvent = new InlineKeyboardButton 
    {
        Text = "Добавить мероприятие",
        CallbackData = "AdminAddEvent"
    };
    private static readonly  InlineKeyboardButton _buttonAdminQuestionsToSpeaker = new InlineKeyboardButton 
    {
        Text = "Вопросы Спикеру",
        CallbackData = "AdminQuestionsToSpeaker"
    };
    private static readonly  InlineKeyboardButton _buttonAdminUserMenu = new InlineKeyboardButton 
    {
        Text = "Меню пользователя",
        CallbackData = "StartMenu"
    };
    private static readonly  InlineKeyboardButton _buttonAdminAddAdministrator = new InlineKeyboardButton 
    {
        Text = "Добавить администратора",
        CallbackData = "AdminAddAdministrator"
    };
}
