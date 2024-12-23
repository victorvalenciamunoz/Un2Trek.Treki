using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Application.Abstractions.Users;
using Un2Trek.Trekis.Domain;
using Un2Trek.Trekis.Domain.ValueObjects;
using Xunit;

public class CaptureTrekiCommandHandlerTests
{
    private readonly ICaptureTrekisRepository _captureTrekisRepository;
    private readonly ITrekisRepository _trekisRepository;
    private readonly IActivitiesTrekiRepository _activitiesTrekiRepository;
    private readonly IConfiguration _configuration;
    private readonly IUsersRepository _usersRepository;
    private readonly CaptureTrekiCommandHandler _handler;

    public CaptureTrekiCommandHandlerTests()
    {
        _captureTrekisRepository = Substitute.For<ICaptureTrekisRepository>();
        _trekisRepository = Substitute.For<ITrekisRepository>();
        _activitiesTrekiRepository = Substitute.For<IActivitiesTrekiRepository>();
        _configuration = Substitute.For<IConfiguration>();
        _usersRepository = Substitute.For<IUsersRepository>();

        // Configuración en memoria
        var inMemorySettings = new Dictionary<string, string> {
        {"Threshold", "100"} // Umbral en metros
    };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _handler = new CaptureTrekiCommandHandler(
            _captureTrekisRepository,
            _trekisRepository,
            _activitiesTrekiRepository,
            _usersRepository,
            _configuration);
    }

    [Fact]
    public async Task Handle_TrekiNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new CaptureTrekiCommand(
            Guid.NewGuid(),
            TrekiId.From(Guid.NewGuid()),
            ActivityId.From(Guid.NewGuid()),
            new Location(0, 0));

        _trekisRepository.GetByIdAsync(command.TrekiId, Arg.Any<CancellationToken>())
            .Returns((Treki)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("Treki no encontrado");
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new CaptureTrekiCommand(
            Guid.NewGuid(),
            TrekiId.From(Guid.NewGuid()),
            ActivityId.From(Guid.NewGuid()),
            new Location(0, 0));

        var treki = new Treki(
            new Location(0, 0),
            "Title",
            "Description",
            true,
            CaptureType.Direct);

        _trekisRepository.GetByIdAsync(command.TrekiId, Arg.Any<CancellationToken>())
            .Returns(treki);

        _usersRepository.GetUserWithCapturesAsync(command.UserId.ToString(), Arg.Any<CancellationToken>())
            .Returns((ApplicationUser)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("Usuario no encontrado");
    }

    [Fact]
    public async Task Handle_TrekiNotInActivity_ReturnsValidationError()
    {
        // Arrange
        var command = new CaptureTrekiCommand(
            Guid.NewGuid(),
            TrekiId.From(Guid.NewGuid()),
            ActivityId.From(Guid.NewGuid()),
            new Location(0, 0));

        var treki = new Treki(
            new Location(0, 0),
            "Title",
            "Description",
            true,
            CaptureType.Direct);

        var user = new ApplicationUser
        {
            Id = command.UserId.ToString(),
            UserName = "TestUser",
            UserTrekiCaptures = new List<UserTrekiCapture>()
        };

        _trekisRepository.GetByIdAsync(command.TrekiId, Arg.Any<CancellationToken>())
            .Returns(treki);

        _usersRepository.GetUserWithCapturesAsync(command.UserId.ToString(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Cambio: Devolver una lista con un Treki diferente
        _activitiesTrekiRepository.GetTrekisByActivityIdAsync(
            command.ActivityId,
            Arg.Any<CancellationToken>())
            .Returns(new List<Treki> { new Treki(new Location(1, 1), "Other Treki", "Description", true, CaptureType.Direct) });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("Treki no pertenece a la actividad");
    }

    [Fact]
    public async Task Handle_TrekiOutOfRange_ReturnsInvalidDistance()
    {
        // Arrange
        var treki = new Treki(
            new Location(10, 10), // Ubicación lejos del usuario
            "Title",
            "Description",
            true,
            CaptureType.Direct);

        var command = new CaptureTrekiCommand(
            Guid.NewGuid(),
            treki.Id,
            ActivityId.From(Guid.NewGuid()),
            new Location(0, 0));

      

        var user = new ApplicationUser
        {
            Id = command.UserId.ToString(),
            UserName = "TestUser",
            UserTrekiCaptures = new List<UserTrekiCapture>()
        };

        _trekisRepository.GetByIdAsync(command.TrekiId, Arg.Any<CancellationToken>())
            .Returns(treki);

        _usersRepository.GetUserWithCapturesAsync(command.UserId.ToString(), Arg.Any<CancellationToken>())
            .Returns(user);

        _activitiesTrekiRepository.GetTrekisByActivityIdAsync(
            command.ActivityId,
            Arg.Any<CancellationToken>())
            .Returns(new List<Treki> { treki });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.InvalidDistance);
    }

    [Fact]
    public async Task Handle_ValidCapture_ReturnsSuccess()
    {
        // Arrange
        var treki = new Treki(
          new Location(0, 0),
          "Title",
          "Description",
          true,
          CaptureType.Direct);

        var command = new CaptureTrekiCommand(
            Guid.NewGuid(),
            treki.Id,
            ActivityId.From(Guid.NewGuid()),
            new Location(0, 0));

        var user = new ApplicationUser
        {
            Id = command.UserId.ToString(),
            UserName = "TestUser",
            UserTrekiCaptures = new List<UserTrekiCapture>()
        };

        _trekisRepository.GetByIdAsync(command.TrekiId, Arg.Any<CancellationToken>())
            .Returns(treki);

        _usersRepository.GetUserWithCapturesAsync(command.UserId.ToString(), Arg.Any<CancellationToken>())
            .Returns(user);

        
        _activitiesTrekiRepository.GetTrekisByActivityIdAsync(
            command.ActivityId,
            Arg.Any<CancellationToken>())
            .Returns(new List<Treki> { treki });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);
    }
}
