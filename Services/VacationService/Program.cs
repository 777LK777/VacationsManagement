using Quartz;

using VacationService;
using VacationService.DB;
using VacationService.Mappers;
using VacationService.Commands;
using VacationService.Validators;
using VacationService.Repositories;
using VacationService.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDB(builder.Configuration)
    .AddMappers()
    .AddCommands()
    .AddValidators()
    .AddRepositories()
    .AddTransient<IVacationSchedule, VacationSchedule>()
    .AddTransient<IVacationCheckConditions, VacationCheckConditions>()
    .AddQuartzHostedService()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer()
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app
        .UseSwagger()
        .UseSwaggerUI();

app.MapControllers();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();