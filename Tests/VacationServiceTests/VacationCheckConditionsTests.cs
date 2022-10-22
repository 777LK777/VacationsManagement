using Moq;

using VacationService.DTO.Vacations;
using VacationService.DTO.Organization;
using VacationService.Repositories;
using VacationService.Repositories.Organization;
using VacationService.Implementation;

namespace VacationServiceTests;

public class VacationCheckConditionsTests
{
    private VacationCheckConditions ConfigureConditionsForCheckOkVacationTime()
    {
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();
        var vacationRepositoryMock = new Mock<IVacationRepository>();
        var vacationBalanceRepositoryMock = new Mock<IVacationBalanceRepository>();
        
        return new VacationCheckConditions(
            vacationRepositoryMock.Object,
            vacationBalanceRepositoryMock.Object,
            employeeRepositoryMock.Object);
    }

    #region CheckOkVacationMinTime
    
    [Theory]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(14)]
    public void CheckOkVacationMinTime_ShouldReturnTrue(int days)
    {
        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 01),
            Days = days
        };

        var vacationConditions = ConfigureConditionsForCheckOkVacationTime();

        var result = vacationConditions.CheckOkVacationMinTime(vacation);
        
        Assert.True(result);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public void CheckOkVacationMinTime_ShouldReturnFalse(int days)
    {
        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 01),
            Days = days
        };

        var vacationConditions = ConfigureConditionsForCheckOkVacationTime();
        
        var result = vacationConditions.CheckOkVacationMinTime(vacation);
        
        Assert.False(result);
    }

    #endregion
    
    #region CheckOkVacationMaxTime

    [Theory]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(14)]
    public void CheckOkVacationMaxTime_ShouldReturnTrue(int days)
    {
        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 01),
            Days = days
        };

        var vacationConditions = ConfigureConditionsForCheckOkVacationTime();

        var result = vacationConditions.CheckOkVacationMinTime(vacation);
        
        Assert.True(result);
    }
    
    [Theory]
    [InlineData(15)]
    [InlineData(18)]
    [InlineData(21)]
    public void CheckOkVacationMaxTime_ShouldReturnFalse(int days)
    {
        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 01),
            Days = days
        };

        var vacationConditions = ConfigureConditionsForCheckOkVacationTime();

        var result = vacationConditions.CheckOkVacationMaxTime(vacation);
        
        Assert.False(result);
    }

    #endregion

    #region CheckTotalVacationTimeAsync
    
    private VacationCheckConditions ConfigureConditionsForCheckTotalVacationTimeAsync(VacationBalance vacationBalance)
    {
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();
        var vacationRepositoryMock = new Mock<IVacationRepository>();
        var vacationBalanceRepositoryMock = new Mock<IVacationBalanceRepository>();
        vacationBalanceRepositoryMock
            .Setup(x => x.GetVacationBalanceByEmployeeId(It.IsAny<Guid>()))
            .Returns(() => Task.FromResult(vacationBalance));
        
        return new VacationCheckConditions(
            vacationRepositoryMock.Object,
            vacationBalanceRepositoryMock.Object,
            employeeRepositoryMock.Object);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(14)]
    public async Task CheckTotalVacationTimeAsync_ShouldReturnTrue(int days)
    {
        var vacationBalance = new VacationBalance
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            CurrentYear = 2022,
            Days = 28
        };
        vacationBalance.Days -= 7; //take 7 days
        vacationBalance.Days -= 7; //take 7 days
        
        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 01),
            Days = days
        };

        var vacationConditions = ConfigureConditionsForCheckTotalVacationTimeAsync(vacationBalance);

        var result = await vacationConditions.CheckTotalVacationTimeAsync(vacation);
        
        Assert.True(result);
    }
    
    [Theory]
    [InlineData(15)]
    [InlineData(21)]
    [InlineData(18)]
    public async Task CheckTotalVacationTimeAsync_ShouldReturnFalse(int days)
    {
        var vacationBalance = new VacationBalance
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            CurrentYear = 2022,
            Days = 28
        };
        vacationBalance.Days -= 7; //take 7 days
        vacationBalance.Days -= 7; //take 7 days
        
        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 01),
            Days = days
        };

        var vacationConditions = ConfigureConditionsForCheckTotalVacationTimeAsync(vacationBalance);

        var result = await vacationConditions.CheckTotalVacationTimeAsync(vacation);
        
        Assert.False(result);
    }

    #endregion

    #region CheckWithoutIntersectionsAsync

    private VacationCheckConditions ConfigureConditionsForCheckIntersections(Vacation existingVacation)
    {
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();
        employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<Guid>()))
            .Returns(() => Task.FromResult(new Employee { Id = Guid.NewGuid() }));
        employeeRepositoryMock
            .Setup(x => x.GetEmployeesIdsByTeamIdAsync(It.IsAny<Guid>()))
            .Returns(() => Task.FromResult(It.IsAny<List<Guid>>()));

        var vacationRepositoryMock = new Mock<IVacationRepository>();
        vacationRepositoryMock
            .Setup(x => x.GetVacationsByEmployeesIdsAsync(It.IsAny<List<Guid>>()))
            .Returns(() => Task.FromResult(new List<Vacation> { existingVacation }));
        
        var vacationBalanceRepositoryMock = new Mock<IVacationBalanceRepository>();
        return new VacationCheckConditions(
            vacationRepositoryMock.Object,
            vacationBalanceRepositoryMock.Object,
            employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task CheckWithoutIntersectionsAsync_EarlyVacation_ShouldReturnTrue()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 06),
            Days = 7
        };

        var existingVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 14),
            Days = 7
        };
        
        var vacationConditions = ConfigureConditionsForCheckIntersections(existingVacation);

        var result = await vacationConditions.CheckWithoutIntersectionsAsync(newVacation);
        
        Assert.True(result);
    }

    [Fact]
    public async Task CheckWithoutIntersectionsAsync_LateVacation_ShouldReturnTrue()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 21),
            Days = 7
        };

        var existingVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 14),
            Days = 7
        };
        
        var vacationConditions = ConfigureConditionsForCheckIntersections(existingVacation);

        var result = await vacationConditions.CheckWithoutIntersectionsAsync(newVacation);
        
        Assert.True(result);
    }

    [Fact]
    public async Task CheckWithoutIntersectionsAsync_LeftIntersection_ShouldReturnFalse()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 10),
            Days = 7
        };

        var existingVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 14),
            Days = 7
        };
        
        var vacationConditions = ConfigureConditionsForCheckIntersections(existingVacation);

        var result = await vacationConditions.CheckWithoutIntersectionsAsync(newVacation);
        
        Assert.False(result);
    }

    [Fact]
    public async Task CheckWithoutIntersectionsAsync_RightIntersection_ShouldReturnFalse()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 19),
            Days = 7
        };

        var existingVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 14),
            Days = 7
        };
        
        var vacationConditions = ConfigureConditionsForCheckIntersections(existingVacation);

        var result = await vacationConditions.CheckWithoutIntersectionsAsync(newVacation);
        
        Assert.False(result);
    }

    [Fact]
    public async Task CheckWithoutIntersectionsAsync_FullIntersection_ShouldReturnFalse()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 18),
            Days = 7
        };

        var existingVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 01, 14),
            Days = 14
        };
        
        var vacationConditions = ConfigureConditionsForCheckIntersections(existingVacation);

        var result = await vacationConditions.CheckWithoutIntersectionsAsync(newVacation);
        
        Assert.False(result);
    }
    
    #endregion

    #region CheckVacationTimeTwoWeeksLeastOnceAsync

    private VacationCheckConditions ConfigureConditionsForCheckVacationTimeTwoWeeksLeastOnceAsync(List<Vacation> existingVacations)
    {
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();

        var vacationRepositoryMock = new Mock<IVacationRepository>();
        vacationRepositoryMock
            .Setup(x => x.GetVacationsByEmployeeIdAsync(It.IsAny<Guid>()))
            .Returns(() => Task.FromResult(existingVacations));

        var vacationBalance = new VacationBalance { Id = Guid.NewGuid(), Days = 28};
        foreach (var existingVacation in existingVacations)
            vacationBalance.Days -= existingVacation.Days;
        
        var vacationBalanceRepositoryMock = new Mock<IVacationBalanceRepository>();
        vacationBalanceRepositoryMock
            .Setup(x => x.GetVacationBalanceByEmployeeId(It.IsAny<Guid>()))
            .Returns(() => Task.FromResult(vacationBalance));
        
        return new VacationCheckConditions(
            vacationRepositoryMock.Object,
            vacationBalanceRepositoryMock.Object,
            employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task CheckVacationTimeTwoWeeksLeastOnceAsync_TwoWeeksVacationExists_ShouldReturnTrue()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 22),
            Days = 7
        };

        var existingVacation0 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 1),
            Days = 14
        };

        var existingVacation1 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 15),
            Days = 7
        };
        
        var vacationConditions =
            ConfigureConditionsForCheckVacationTimeTwoWeeksLeastOnceAsync(new List<Vacation> {existingVacation0, existingVacation1});

        var result = await vacationConditions.CheckVacationTimeTwoWeeksLeastOnceAsync(newVacation);
        
        Assert.True(result);
    }

    [Fact]
    public async Task CheckVacationTimeTwoWeeksLeastOnceAsync_TwoWeeksVacationNotExists_AddWeek_ShouldReturnTrue()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 22),
            Days = 7
        };

        var existingVacation0 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 1),
            Days = 7
        };
        
        var vacationConditions =
            ConfigureConditionsForCheckVacationTimeTwoWeeksLeastOnceAsync(new List<Vacation> {existingVacation0});

        var result = await vacationConditions.CheckVacationTimeTwoWeeksLeastOnceAsync(newVacation);
        
        Assert.True(result);
    }

    [Fact]
    public async Task CheckVacationTimeTwoWeeksLeastOnceAsync_TwoWeeksVacationNotExists_AddTwoWeeks_ShouldReturnTrue()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 15),
            Days = 14
        };

        var existingVacation0 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 1),
            Days = 7
        };

        var existingVacation1 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 8),
            Days = 7
        };
        
        var vacationConditions =
            ConfigureConditionsForCheckVacationTimeTwoWeeksLeastOnceAsync(new List<Vacation> {existingVacation0, existingVacation1});

        var result = await vacationConditions.CheckVacationTimeTwoWeeksLeastOnceAsync(newVacation);
        
        Assert.True(result);
    }

    [Fact]
    public async Task CheckVacationTimeTwoWeeksLeastOnceAsync_TwoWeeksVacationExists_AddTwoWeeks_ShouldReturnTrue()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 15),
            Days = 14
        };

        var existingVacation0 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 1),
            Days = 14
        };
        
        var vacationConditions =
            ConfigureConditionsForCheckVacationTimeTwoWeeksLeastOnceAsync(new List<Vacation> {existingVacation0});

        var result = await vacationConditions.CheckVacationTimeTwoWeeksLeastOnceAsync(newVacation);
        
        Assert.True(result);
    }

    [Fact]
    public async Task CheckVacationTimeTwoWeeksLeastOnceAsync_WeekVacationTwiceExists_AddWeekVacation_ShouldReturnFalse()
    {
        var newVacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 15),
            Days = 7
        };

        var existingVacation0 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 1),
            Days = 7
        };

        var existingVacation1 = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            DateStart = new DateOnly(2022, 03, 8),
            Days = 7
        };
        
        var vacationConditions =
            ConfigureConditionsForCheckVacationTimeTwoWeeksLeastOnceAsync(new List<Vacation> {existingVacation0, existingVacation1});

        var result = await vacationConditions.CheckVacationTimeTwoWeeksLeastOnceAsync(newVacation);
        
        Assert.False(result);
    }

    #endregion
}