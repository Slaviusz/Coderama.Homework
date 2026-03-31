namespace Coderama.Tests.Validators;

using Abstractions.Contracts.Requests;
using Api.Validators;

public class ValidatorsTest {

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_AllData_Success()
    {
        // Arrange
        var request = new DocumentPostRequest(
            Guid.NewGuid().ToString(),
            ["tag1", "tag2"],
            JsonDocument.Parse("""{"test":"value"}""")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_NoTags_Success()
    {
        // Arrange
        var request = new DocumentPostRequest(
            Guid.NewGuid().ToString(),
            [],
            JsonDocument.Parse("""{"test":"value"}""")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_EmptyTags_Success()
    {
        // Arrange
        var request = new DocumentPostRequest(
        Guid.NewGuid().ToString(),
        ["", string.Empty],
        JsonDocument.Parse("""{"test":"value"}""")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_EmptyJsonDocument_Success()
    {
        // Arrange
        var request = new DocumentPostRequest(
        Guid.NewGuid().ToString(),
        [],
        JsonDocument.Parse("{}")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_EmptyId_Failure()
    {
        // Arrange
        var request = new DocumentPostRequest(
        "",
        [],
        JsonDocument.Parse("{}")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_BlankId_Failure()
    {
        // Arrange
        var request = new DocumentPostRequest(
        "   ",
        [],
        JsonDocument.Parse("{}")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task Test_DocumentPostRequestvalidator_TabsInId_Failure()
    {
        // Arrange
        var request = new DocumentPostRequest(
        "       ",
        [],
        JsonDocument.Parse("{}")
        );

        var sut = new DocumentPostRequestValidator();

        // Act
        var result = await sut.ValidateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
    }
}
