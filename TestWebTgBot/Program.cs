// See https://aka.ms/new-console-template for more information

using System.Data;
using FluentMigrator.Runner;
using MySql.Data.MySqlClient;
using TestWebTgBot;
using TestWebTgBot.Migration;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Repositories.Admin;
using TestWebTgBot.Repositories.Quiz;
using TestWebTgBot.Repositories.QuizAnswer;
using TestWebTgBot.Repositories.QuizQuestion;
using TestWebTgBot.Repositories.QuizVariants;
using TestWebTgBot.Repositories.User;
using TestWebTgBot.Services;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Services.Telegram;
using TestWebTgBot.Services.TelegramApi;
using TestWebTgBot.Services.User;
using TestWebTgBot.TgBot;
using TestWebTgBot.TgBot.UpdateHandlers;
using TestWebTgBot.TgBot.UpdateHandlers.Admin;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.AddAdmin;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.EventAdmin;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.News;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.QuestionsAdmin;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.AddQuestion;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.EditQuestion;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.EditQuestion.Detailed;
using TestWebTgBot.TgBot.UpdateHandlers.Admin.VotingAdmin;
using TestWebTgBot.TgBot.UpdateHandlers.User;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserEvents;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserInfoProject;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserQuestions;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserQuiz;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserSocialLinks;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserStart;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserSubscribe;
using TestWebTgBot.TgBot.UpdateHandlers.User.UserVoting;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IQuizUsersRepository, QuizUsersRepository>();
builder.Services.AddScoped<IQuizAdminRepository , QuizAdminRepository>();
builder.Services.AddScoped<ITelegramBotService , TelegramBotService>();
builder.Services.AddScoped<IUserRepository , UserRepository>();
builder.Services.AddScoped<IAdminRepository , AdminRepository>();
builder.Services.AddScoped<IQuestionsRepository, QuestionsRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizAnswerRepository, QuizAnswerRepository>();
builder.Services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
builder.Services.AddScoped<IQuizVariantsRepository, QuizVariantsRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ITelegramApiService , TelegramApiService>();
builder.Services.AddScoped<ILinksSocialNetworksRepository , LinksSocialNetworksRepository>();
builder.Services.AddScoped<IEventEntriesRepository , EventEntriesRepository>();
builder.Services.AddScoped<TelegramBot>();

builder.Services.AddHostedService<ReminderBackgroundService>();
builder.Services.AddHostedService<AdminPasswordCleanupService>();

builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));
builder.Services.AddScoped<IUpdateHandler, StartCommandUpdateHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminUpdateHandler>();
builder.Services.AddScoped<IUpdateHandler, SubscribeUpdateHandler>();
builder.Services.AddScoped<IUpdateHandler, UnsubscribeUpdateHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminMainHandler>();
builder.Services.AddScoped<IUpdateHandler, GoToAdminQuizMenuHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminReturnToMainButtonsHandler>();
builder.Services.AddScoped<IUpdateHandler, GoToStartButtonsHandler>();
builder.Services.AddScoped<IUpdateHandler, QuizStartHandler>();
builder.Services.AddScoped<IUpdateHandler, UserAddFullNameHandler>();
builder.Services.AddScoped<IUpdateHandler, ChangeUserFullNameHandler>(); 
builder.Services.AddScoped<IUpdateHandler, UserTryAskQuestionHandler>();
builder.Services.AddScoped<IUpdateHandler, UserWriteQuestionHandler>();
builder.Services.AddScoped<IUpdateHandler, UserSocialLinksHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminAddNewEventHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminWaitNewEventHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminQuestionsHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminDeleteQuestionsHandler>();
builder.Services.AddScoped<IUpdateHandler, AdminCheckToDeleteQuestionsHandler>();
builder.Services.AddScoped<IUpdateHandler, GetInfoAboutProjectsHandler>();
builder.Services.AddScoped<IUpdateHandler, UserAllEventsHandler>();
builder.Services.AddScoped<IUpdateHandler, SubscribeToEventHandler>();
builder.Services.AddScoped<IUpdateHandler, UserWatchEventsHandler>();
builder.Services.AddScoped<IUpdateHandler, UnsubscribeFromEventHandler>();
builder.Services.AddScoped<IUpdateHandler, StartQuizHandler>();
builder.Services.AddScoped<IUpdateHandler, AddNewsHandler>();
builder.Services.AddScoped<IUpdateHandler, CreateNewsHandler>();
builder.Services.AddScoped<IUpdateHandler, OnlineVotingUsersHandler>();
builder.Services.AddScoped<IUpdateHandler, CreateVotingHandler>();
builder.Services.AddScoped<IUpdateHandler, AddQuestionsForVotingHandler>();
builder.Services.AddScoped<IUpdateHandler, OnlineVotingMenuAdminHandler>();
builder.Services.AddScoped<IUpdateHandler, StartVotingHandler>();
builder.Services.AddScoped<IUpdateHandler, EndVotingHandler>();
builder.Services.AddScoped<IUpdateHandler, UserVoteHandler>();
builder.Services.AddScoped<IUpdateHandler, GoToStartButtonsAfterVotingHandler>();
builder.Services.AddScoped<IUpdateHandler, ResultVotingAdminHandler>();
builder.Services.AddScoped<IUpdateHandler, AddAdminHandler>();
builder.Services.AddScoped<IUpdateHandler, ViewEditQuizHandler>();
builder.Services.AddScoped<IUpdateHandler, AddQuestionQuizHandler>();
builder.Services.AddScoped<IUpdateHandler, AddQuestionQuizMessageHandler>();
builder.Services.AddScoped<IUpdateHandler, EditQuestionQuizHandler>();
builder.Services.AddScoped<IUpdateHandler, EditQuizQuestionDetailHandler>();
builder.Services.AddScoped<IUpdateHandler, AddQuizQuestionVariantHandler>();
builder.Services.AddScoped<IUpdateHandler, AddQuizQuestionVariantMessageHandler>();
builder.Services.AddScoped<IUpdateHandler, DeleteQuestionVariantsHandler>();
builder.Services.AddScoped<IUpdateHandler, DeleteQuestionVariantHandler>();
//QuizUser
builder.Services.AddScoped<IUpdateHandler, QuizFirstQuestionHandler>();
builder.Services.AddScoped<IUpdateHandler, QuizQuestionHandler>();

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(runner => runner
        .AddMySql8()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(CreateUserTable).Assembly).For.Migrations())
    .AddLogging(logging => logging
        .AddFluentMigratorConsole());

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/bot", async (Update update, ITelegramBotService telegramBotService) =>
{
    await telegramBotService.OnUpdate(update);
    return Results.Ok();
});

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}
app.Run();

//var bot = new TelegramBot("7861245642:AAExHqbuzIcXWFXQvhoj8eZumKTX2vnk6I8");

//var clt = new CancellationToken();

//bot.AddUpdateListener(new MyListener());

//await bot.Run(clt);
