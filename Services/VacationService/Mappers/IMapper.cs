namespace VacationService.Mappers;

public interface IMapper<TIn, TOut>
{
    TOut Map(TIn dbVacation);
}