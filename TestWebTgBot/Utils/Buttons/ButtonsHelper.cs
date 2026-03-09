using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.Utils.Buttons;

public static class ButtonsHelper
{
    public static InlineKeyboardMarkup ButtonsGoToVoting()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {_buttonOnlineVoting }
            }
        };
    }
    
    public static InlineKeyboardMarkup ButtonsVotingUsersOptions(string firstOption, string secondOption)
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> {
                    ButtonFirstOptionVotingUsers(firstOption) ,
                    ButtonSecondOptionVotingUsers(secondOption) }
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsVotingNotStartContext()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonVotingUpdate , _buttonMainUser}
            }
        };
    }
    public static InlineKeyboardMarkup CreateEventDeleteButtons(List<ScheduleOfEventsEntity> registeredEventDetails)
    {
        var buttons = registeredEventDetails
            .Select(ev => new InlineKeyboardButton
            {
                Text = $"{ev.NameEvent}",
                CallbackData = $"UnregisterEvent_{ev.Id}"
            })
            .ToList();
            buttons.Add(_buttonMainUser);
            
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = buttons
                .Select(button => new List<InlineKeyboardButton> {button})
                .ToList()
        };
    }
    public static InlineKeyboardMarkup ButtonsEventsAddContext()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonScheduleOfEvents , _buttonMainUser}
            }
        };
    }
    public static InlineKeyboardMarkup CreateEventButtons(List<ScheduleOfEventsEntity> events)
    {
        var buttons = events
            .Select(ev => new InlineKeyboardButton
            {
                Text = $"{ev.NameEvent}",
                CallbackData = $"RegisterEvent_{ev.Id}"
            })
            .ToList();
        buttons.Add(_buttonMainUser);
        
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = buttons
                .Select(button => new List<InlineKeyboardButton> { button })
                .ToList()
            
        };
    }
    public static InlineKeyboardMarkup ButtonsEventsContext()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonScheduleOfEvents },
                new List<InlineKeyboardButton> { _buttonMainUser }
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsLinks(
        string nameFirstButton,
        string linkFirstButton)
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { ButtonLinkSocialNetwork(nameFirstButton, linkFirstButton) },
                new List<InlineKeyboardButton> { _buttonMainUser }
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsRegistration()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonChangeUserFullName , _buttonMainUser}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsNewQuestion()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonNewQuestion , _buttonMainUser}
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsStartPanel(bool isSubscribed)
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { ButtonSubscribe(isSubscribed) },
                new List<InlineKeyboardButton> { _buttonScheduleOfEvents , _buttonRegistrationForEvents },
                new List<InlineKeyboardButton> { _buttonQuestion },
                new List<InlineKeyboardButton> { _buttonQuiz , _buttonLinksToSocialNetworks },
                new List<InlineKeyboardButton> { _buttonOnlineVoting }
            }
        };
    }
    public static InlineKeyboardMarkup ButtonsChoseAdminOrUserStartButtons()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonMainAdmin },
                new List<InlineKeyboardButton> { _buttonMainUser }
            }
        };
    }
    public static InlineKeyboardMarkup ButtonGoToStartUserAfterVotingButtons()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonGoToMainUserAfterVotingButtons }
            }
        };
    }
    public static InlineKeyboardMarkup ButtonGoToStartUserButtons()
    {
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { _buttonGoToMainUserButtons }
            }
        };
    }
    
    //Voting
    private static InlineKeyboardButton ButtonSecondOptionVotingUsers(string secondOption)
    {
        return new InlineKeyboardButton
        {
            Text = secondOption,
            CallbackData = "UserVoted_1"

        };
    }
    private static InlineKeyboardButton ButtonFirstOptionVotingUsers(string firstOption)
    {
        return new InlineKeyboardButton
        {
            Text = firstOption,
            CallbackData = "UserVoted_0"

        };
    }
    private static InlineKeyboardButton ButtonSubscribe(bool isSubscribed)
    {
        return new InlineKeyboardButton
        {
            Text = isSubscribed ? "Отписаться от рассылки новостей" : "Подписаться на рассылку новостей",
            CallbackData = isSubscribed ? "NewsletterOff" : "NewsletterOn"

        };
    }

    private static InlineKeyboardButton ButtonLinkSocialNetwork(string buttonName, string buttonLink)
    {
        return new InlineKeyboardButton
        {
            Text = buttonName,
            Url = buttonLink
        };
    }
    
    private static readonly  InlineKeyboardButton _buttonVotingUpdate = new InlineKeyboardButton
    {
        Text = "Обновить",
        CallbackData = "OnlineVoting"
    };
    
    private static readonly  InlineKeyboardButton _buttonChangeUserFullName = new InlineKeyboardButton
    {
        Text = "Изменить ФИО",
        CallbackData = "ChangeUserFullName"
    };
    private static readonly  InlineKeyboardButton _buttonQuiz = new InlineKeyboardButton
    {
        Text = "Сыграть в квиз\ud83e\udde0",
        CallbackData = "QuizStartUser"
    };
    private static readonly InlineKeyboardButton _buttonQuestion = new InlineKeyboardButton
    {
        Text = "Вопрос спикеру\u2753",
        CallbackData = "AskQuestion"
    };
    private static readonly InlineKeyboardButton _buttonNewQuestion = new InlineKeyboardButton
    {
        Text = "Новый вопрос",
        CallbackData = "AskQuestion"
    };
    private static readonly InlineKeyboardButton _buttonScheduleOfEvents = new InlineKeyboardButton
    {
        Text = "Мероприятия\ud83d\uddd3\ufe0f",
        CallbackData = "ScheduleOfEvents"
    };
    private static readonly InlineKeyboardButton _buttonRegistrationForEvents = new InlineKeyboardButton
    {
        Text = "Мои записи\ud83d\udccc",
        CallbackData = "RegistrationForEvents"
    };
    private static readonly InlineKeyboardButton _buttonInformationAboutProjects = new InlineKeyboardButton
    {
        Text = "Info о проектахℹ\ufe0f",
        CallbackData = "InformationAboutProjects"
    }; 
    private static readonly InlineKeyboardButton _buttonLinksToSocialNetworks = new InlineKeyboardButton
    {
        Text =  "Ссылки на соц.сети📲",
        CallbackData = "LinksToSocialNetworks"
    }; 
    private static readonly InlineKeyboardButton _buttonOnlineVoting = new InlineKeyboardButton
    {
        Text = "Онлайн голосование",
        CallbackData = "OnlineVoting"
    };  
    private static readonly InlineKeyboardButton _buttonMainUser = new InlineKeyboardButton
    {
        Text = "Меню пользователя",
        CallbackData = "StartMenu"
    };
    private static readonly InlineKeyboardButton _buttonMainAdmin = new InlineKeyboardButton
    {
        Text = "Меню администратора",
        CallbackData = "MainAdminMenu"
    };
    private static readonly InlineKeyboardButton _buttonGoToMainUserAfterVotingButtons = new InlineKeyboardButton
    {
        Text = "Вернуться в главное меню",
        CallbackData = "StartMenuAfterVoting"
    };
    private static readonly InlineKeyboardButton _buttonGoToMainUserButtons = new InlineKeyboardButton
    {
        Text = "Вернуться в главное меню",
        CallbackData = "StartMenu"
    };

    public static InlineKeyboardMarkup CreateVariantsButtons(QuizQuestionEntity question)
    {
        var buttons = question.Variants
            .Select(variant => new InlineKeyboardButton
            {
                Text = variant.Text,
                CallbackData = $"answ_{variant.QuestionId}_{variant.Id}"
            })
            .ToList();

        var markup = new InlineKeyboardMarkup{};

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i % 2 == 0)
            {
                markup.InlineKeyboard.Add(new List<InlineKeyboardButton>());
            }
            markup.InlineKeyboard[^1].Add(buttons[i]);
        }

        return markup;
    }
}
// public static InlineKeyboardMarkup CreateButtonTest()
// {
//     return new InlineKeyboardMarkup
//     {
//         InlineKeyboard = new List<List<InlineKeyboardButton>>
//         {
//             new List<InlineKeyboardButton> { }
//         }
//     };
// }
