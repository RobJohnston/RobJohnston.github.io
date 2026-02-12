---
applyTo: "**/tests/**,**/test/**,**/*Tests.cs,**/*Test.cs"
---

# Testing Instructions

All Government of Canada applications must include comprehensive automated testing to ensure quality, security, and accessibility.

## Testing Standards

- **Minimum code coverage**: 70% for new code
- **Required test types**: Unit, Integration, Accessibility
- **CI/CD integration**: Tests must run on every pull request
- **Test naming**: Clear, descriptive test names explaining what is tested

## Test Project Structure

```
solution/
├── src/
│   └── BenefitsApp/
│       ├── Controllers/
│       ├── Services/
│       └── Models/
└── tests/
    ├── BenefitsApp.UnitTests/
    │   ├── Controllers/
    │   ├── Services/
    │   └── Models/
    ├── BenefitsApp.IntegrationTests/
    │   ├── Api/
    │   └── Database/
    └── BenefitsApp.AccessibilityTests/
        └── Pages/
```

## Unit Testing

### Framework: xUnit

**Installation**:
```bash
dotnet add package xUnit
dotnet add package xunit.runner.visualstudio
dotnet add package Moq  # For mocking
dotnet add package FluentAssertions  # For readable assertions
```

### Unit Test Pattern

```csharp
using Xunit;
using FluentAssertions;
using Moq;

namespace BenefitsApp.UnitTests.Services
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationRepository> _mockRepository;
        private readonly Mock<ILogger<ApplicationService>> _mockLogger;
        private readonly ApplicationService _sut; // System Under Test

        public ApplicationServiceTests()
        {
            _mockRepository = new Mock<IApplicationRepository>();
            _mockLogger = new Mock<ILogger<ApplicationService>>();
            _sut = new ApplicationService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateApplication_WithValidData_ReturnsApplicationId()
        {
            // Arrange
            var application = new Application
            {
                FirstName = "John",
                LastName = "Doe",
                Sin = "123-456-789"
            };

            _mockRepository
                .Setup(r => r.CreateAsync(It.IsAny<Application>()))
                .ReturnsAsync(12345);

            // Act
            var result = await _sut.CreateApplicationAsync(application);

            // Assert
            result.Should().Be(12345);
            _mockRepository.Verify(r => r.CreateAsync(application), Times.Once);
        }

        [Fact]
        public async Task CreateApplication_WithInvalidSin_ThrowsValidationException()
        {
            // Arrange
            var application = new Application
            {
                FirstName = "John",
                LastName = "Doe",
                Sin = "invalid"
            };

            // Act
            Func<Task> act = async () => await _sut.CreateApplicationAsync(application);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Invalid Social Insurance Number");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task CreateApplication_WithEmptyFirstName_ThrowsValidationException(string firstName)
        {
            // Arrange
            var application = new Application
            {
                FirstName = firstName,
                LastName = "Doe",
                Sin = "123-456-789"
            };

            // Act
            Func<Task> act = async () => await _sut.CreateApplicationAsync(application);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
```

### Test Naming Convention

```csharp
// Pattern: MethodName_Scenario_ExpectedBehavior

[Fact]
public void ValidateSin_WithValidFormat_ReturnsTrue()

[Fact]
public void ValidateSin_WithInvalidFormat_ReturnsFalse()

[Fact]
public void CalculateBenefit_WhenEligible_ReturnsCorrectAmount()

[Fact]
public void CalculateBenefit_WhenNotEligible_ReturnsZero()
```

### Mocking Best Practices

```csharp
// ✅ GOOD: Mock interfaces, not concrete classes
private readonly Mock<IUserRepository> _mockUserRepository;

// ❌ BAD: Don't mock concrete classes
private readonly Mock<UserRepository> _mockUserRepository;

// ✅ GOOD: Verify important interactions
_mockRepository.Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Once);

// ✅ GOOD: Use It.IsAny<T>() for parameters you don't care about
_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

// ✅ GOOD: Use specific values when they matter
_mockRepository.Setup(r => r.GetByIdAsync(12345)).ReturnsAsync(user);
```

## Integration Testing

### Framework: xUnit + WebApplicationFactory

**Installation**:
```bash
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

### Integration Test Setup

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BenefitsApp.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real database
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Build service provider and seed test data
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    db.Database.EnsureCreated();
                    SeedTestData(db);
                }
            });
        }

        private void SeedTestData(ApplicationDbContext db)
        {
            db.Users.Add(new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.gc.ca"
            });
            db.SaveChanges();
        }
    }
}
```

### API Integration Tests

```csharp
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BenefitsApp.IntegrationTests.Api
{
    public class ApplicationControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ApplicationControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetApplication_WithValidId_ReturnsApplication()
        {
            // Act
            var response = await _client.GetAsync("/api/applications/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var application = await response.Content.ReadFromJsonAsync<Application>();
            application.Should().NotBeNull();
            application.Id.Should().Be(1);
        }

        [Fact]
        public async Task CreateApplication_WithValidData_ReturnsCreated()
        {
            // Arrange
            var newApplication = new Application
            {
                FirstName = "Jane",
                LastName = "Smith",
                Sin = "987-654-321"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/applications", newApplication);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var created = await response.Content.ReadFromJsonAsync<Application>();
            created.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateApplication_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidApplication = new Application
            {
                FirstName = "", // Invalid: empty
                LastName = "Smith",
                Sin = "invalid-sin"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/applications", invalidApplication);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
```

### Database Integration Tests

```csharp
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BenefitsApp.IntegrationTests.Database
{
    public class ApplicationRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationRepository _repository;

        public ApplicationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ApplicationRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_SavesApplicationToDatabase()
        {
            // Arrange
            var application = new Application
            {
                FirstName = "John",
                LastName = "Doe",
                Sin = "123-456-789"
            };

            // Act
            var id = await _repository.CreateAsync(application);

            // Assert
            var saved = await _context.Applications.FindAsync(id);
            saved.Should().NotBeNull();
            saved.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectApplication()
        {
            // Arrange
            var application = new Application
            {
                FirstName = "Jane",
                LastName = "Smith",
                Sin = "987-654-321"
            };
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(application.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(application.Id);
            result.FirstName.Should().Be("Jane");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
```

## Accessibility Testing

### Framework: Selenium + axe-core

**Installation**:
```bash
dotnet add package Selenium.WebDriver
dotnet add package Selenium.WebDriver.ChromeDriver
dotnet add package Deque.AxeCore.Selenium
```

### Accessibility Test Setup

```csharp
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Deque.AxeCore.Selenium;
using Deque.AxeCore.Commons;
using Xunit;

namespace BenefitsApp.AccessibilityTests
{
    public class PageAccessibilityTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5000";

        public PageAccessibilityTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run without opening browser
            _driver = new ChromeDriver(options);
        }

        [Fact]
        public void HomePage_PassesWCAG_AA_AccessibilityScan()
        {
            // Arrange
            _driver.Navigate().GoToUrl(_baseUrl);

            // Act
            var axeResult = new AxeBuilder(_driver)
                .WithTags("wcag2a", "wcag2aa", "wcag21a", "wcag21aa")
                .Analyze();

            // Assert
            axeResult.Violations.Should().BeEmpty(
                $"Found {axeResult.Violations.Length} accessibility violations:\n" +
                string.Join("\n", axeResult.Violations.Select(v =>
                    $"- {v.Help} ({v.Impact}): {v.Description}"))
            );
        }

        [Fact]
        public void ApplicationForm_PassesAccessibilityScan()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{_baseUrl}/apply");

            // Act
            var axeResult = new AxeBuilder(_driver)
                .WithTags("wcag2a", "wcag2aa")
                .Analyze();

            // Assert
            axeResult.Violations.Should().BeEmpty();
        }

        [Fact]
        public void ApplicationForm_AllFormFieldsHaveLabels()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{_baseUrl}/apply");

            // Act
            var inputs = _driver.FindElements(By.TagName("input"));
            var unlabeledInputs = inputs.Where(input =>
            {
                var id = input.GetAttribute("id");
                var label = _driver.FindElements(By.CssSelector($"label[for='{id}']"));
                return !label.Any();
            }).ToList();

            // Assert
            unlabeledInputs.Should().BeEmpty("All form inputs must have associated labels");
        }

        [Fact]
        public void ApplicationForm_KeyboardNavigationWorks()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{_baseUrl}/apply");
            var body = _driver.FindElement(By.TagName("body"));

            // Act - Tab through all focusable elements
            var focusableElements = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                body.SendKeys(Keys.Tab);
                var activeElement = _driver.SwitchTo().ActiveElement();
                var tagName = activeElement.TagName;
                focusableElements.Add(tagName);
            }

            // Assert
            focusableElements.Should().Contain("input");
            focusableElements.Should().Contain("button");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
```

### Accessibility Test Report

```csharp
[Fact]
public void AllPages_GenerateAccessibilityReport()
{
    var pages = new[]
    {
        "/",
        "/apply",
        "/status",
        "/contact"
    };

    var reportBuilder = new StringBuilder();
    reportBuilder.AppendLine("# Accessibility Scan Report");
    reportBuilder.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    reportBuilder.AppendLine();

    foreach (var page in pages)
    {
        _driver.Navigate().GoToUrl($"{_baseUrl}{page}");
        var axeResult = new AxeBuilder(_driver)
            .WithTags("wcag2a", "wcag2aa", "wcag21a", "wcag21aa")
            .Analyze();

        reportBuilder.AppendLine($"## {page}");
        reportBuilder.AppendLine($"- Violations: {axeResult.Violations.Length}");
        reportBuilder.AppendLine($"- Passes: {axeResult.Passes.Length}");

        if (axeResult.Violations.Any())
        {
            reportBuilder.AppendLine("### Violations:");
            foreach (var violation in axeResult.Violations)
            {
                reportBuilder.AppendLine($"- **{violation.Help}** ({violation.Impact})");
                reportBuilder.AppendLine($"  - {violation.Description}");
                reportBuilder.AppendLine($"  - Affected: {violation.Nodes.Length} element(s)");
            }
        }
        reportBuilder.AppendLine();
    }

    File.WriteAllText("accessibility-report.md", reportBuilder.ToString());
}
```

## Test Configuration

### xunit.runner.json

```json
{
  "$schema": "https://xunit.net/schema/current/xunit.runner.schema.json",
  "methodDisplay": "method",
  "parallelizeAssembly": true,
  "parallelizeTestCollections": true,
  "maxParallelThreads": 4
}
```

### CI/CD Integration (GitHub Actions)

```yaml
# .github/workflows/tests.yml
name: Tests

on:
  pull_request:
    branches: [ main ]
  push:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release

    - name: Run unit tests
      run: dotnet test tests/BenefitsApp.UnitTests --no-build --configuration Release --logger "trx;LogFileName=unit-tests.trx"

    - name: Run integration tests
      run: dotnet test tests/BenefitsApp.IntegrationTests --no-build --configuration Release --logger "trx;LogFileName=integration-tests.trx"

    - name: Install Chrome for accessibility tests
      run: |
        wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | sudo apt-key add -
        sudo sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google-chrome.list'
        sudo apt-get update
        sudo apt-get install -y google-chrome-stable

    - name: Run accessibility tests
      run: dotnet test tests/BenefitsApp.AccessibilityTests --no-build --configuration Release --logger "trx;LogFileName=accessibility-tests.trx"

    - name: Publish test results
      uses: dorny/test-reporter@v1
      if: always()
      with:
        name: Test Results
        path: '**/*.trx'
        reporter: dotnet-trx

    - name: Code coverage
      run: |
        dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage"
        dotnet tool install -g dotnet-reportgenerator-globaltool
        reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:HtmlInline_AzurePipelines

    - name: Upload coverage report
      uses: actions/upload-artifact@v3
      with:
        name: coverage-report
        path: coverage-report/
```

## Testing Best Practices

### 1. Test Independence
Tests should not depend on each other:
```csharp
// ✅ GOOD: Each test has its own setup
[Fact]
public void Test1()
{
    var service = new ApplicationService();
    // Test logic
}

[Fact]
public void Test2()
{
    var service = new ApplicationService();
    // Test logic
}

// ❌ BAD: Tests share state
private static ApplicationService _sharedService = new ApplicationService();

[Fact]
public void Test1()
{
    _sharedService.DoSomething();
}
```

### 2. Arrange-Act-Assert Pattern
```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange - Set up test data and mocks
    var input = new Input();
    var expectedOutput = "result";

    // Act - Execute the method being tested
    var result = _sut.Method(input);

    // Assert - Verify the results
    result.Should().Be(expectedOutput);
}
```

### 3. Test One Thing
```csharp
// ✅ GOOD: Tests one behavior
[Fact]
public void ValidateSin_WithValidFormat_ReturnsTrue()
{
    var result = _validator.ValidateSin("123-456-789");
    result.Should().BeTrue();
}

// ❌ BAD: Tests multiple things
[Fact]
public void ValidateSin_TestsMultipleScenarios()
{
    _validator.ValidateSin("123-456-789").Should().BeTrue();
    _validator.ValidateSin("invalid").Should().BeFalse();
    _validator.ValidateSin(null).Should().BeFalse();
}
```

### 4. Meaningful Test Data
```csharp
// ✅ GOOD: Meaningful test data
var application = new Application
{
    FirstName = "John",
    LastName = "Doe",
    Sin = "123-456-789"
};

// ❌ BAD: Generic test data
var application = new Application
{
    FirstName = "Test",
    LastName = "Test",
    Sin = "111-111-111"
};
```

## Test Coverage Requirements

- **Controllers**: 80% minimum
- **Services**: 90% minimum
- **Repositories**: 80% minimum
- **Models/DTOs**: 70% minimum (property validation)
- **Overall**: 70% minimum

**Generate coverage report**:
```bash
dotnet test --collect:"XPlat Code Coverage"
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report
```

## Common Testing Mistakes

❌ **Don't test framework code** - Test your code, not Entity Framework or ASP.NET
❌ **Don't skip edge cases** - Test null, empty, boundary values
❌ **Don't ignore async/await** - Use async test methods for async code
❌ **Don't use Thread.Sleep** - Use async waits or Task.Delay
❌ **Don't test private methods** - Test through public API
❌ **Don't commit failing tests** - Fix or skip with `[Fact(Skip = "Reason")]`
❌ **Don't mock everything** - Use real objects when simple

## Resources

- **xUnit Documentation**: https://xunit.net/
- **FluentAssertions**: https://fluentassertions.com/
- **Moq Documentation**: https://github.com/moq/moq4
- **axe-core**: https://www.deque.com/axe/core-documentation/
- **Selenium WebDriver**: https://www.selenium.dev/documentation/webdriver/

## Version Information

This guidance is current for:
- .NET 8.0
- xUnit 2.5+
- Selenium WebDriver 4.0+
- axe-core 4.0+

Last updated: 2025-02-11
