namespace VacationService.Commands;
/*
public interface ICommand<TOut>
{
    Task<TOut> Execute();
}*/

public interface ICommand<TIn, TOut>
{
    Task<TOut> Execute(TIn request);
}

public interface ICommand<TIn>
{
    Task Execute(TIn request);
}