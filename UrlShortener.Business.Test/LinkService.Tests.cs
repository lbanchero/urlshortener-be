using Moq;
using UrlShortener.Business.Exceptions;
using UrlShortener.Data.Interfaces;
using UrlShortener.Data.Models;

namespace UrlShortener.Business.Test;

[TestFixture]
public class LinkServiceTests
{
    private Mock<ILinkRepository> _mockLinkRepository;
    private Mock<IClickRepository> _mockClickRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private LinkService _linkService;

    [SetUp]
    public void Setup()
    {
        _mockLinkRepository = new Mock<ILinkRepository>();
        _mockClickRepository = new Mock<IClickRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _linkService = new LinkService(_mockLinkRepository.Object, _mockClickRepository.Object, _mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetAsync_WithValidShortCode_ReturnsLink()
    {
        // Arrange
        var shortCode = "validCode";
        var link = new Link();
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeAsync(shortCode)).ReturnsAsync(link);

        // Act
        var result = await _linkService.GetAsync(shortCode);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.EqualTo(link));
    }

    [Test]
    public void GetAsync_WithInvalidShortCode_ThrowsLinkNotFoundException()
    {
        // Arrange
        var shortCode = "invalidCode";
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeAsync(shortCode)).ReturnsAsync((Link)null);

        // Act & Assert
        Assert.ThrowsAsync<LinkNotFoundException>(async () => await _linkService.GetAsync(shortCode));
    }
    
    [Test]
    public void GetAsync_WithErrorOnRepository_PropagatesException()
    {
        // Arrange
        var shortCode = "someCode";
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeAsync(shortCode)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _linkService.GetAsync(shortCode));
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void GetAsync_WithNullOrEmptyShortCode_ThrowsArgumentException(string shortCode)
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _linkService.GetAsync(shortCode));
    }
    
    [Test]
    public async Task GetAsync_WithValidShortCode_CreatesClick()
    {
        // Arrange
        var shortCode = "validCode";
        var link = new Link();
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeAsync(shortCode)).ReturnsAsync(link);
        _mockClickRepository.Setup(repo => repo.CreateAsync(It.IsAny<Click>())).Verifiable();
        
        // Act
        await _linkService.GetAsync(shortCode);

        // Assert
        _mockClickRepository.Verify(repo => repo.CreateAsync(It.IsAny<Click>()), Times.Once);
    }
    
    [Test]
    public async Task GetStatsAsync_WithValidShortUrl_ReturnsLinkWithClicks()
    {
        // Arrange
        var shortUrl = "validUrl";
        var linkWithClicks = new Link { /* Initialize with test data including clicks */ };
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeWithClicksAsync(shortUrl)).ReturnsAsync(linkWithClicks);

        // Act
        var result = await _linkService.GetStatsAsync(shortUrl);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.EqualTo(linkWithClicks));
    }
    
    [Test]
    public void GetStatsAsync_WithInvalidShortUrl_ThrowsLinkNotFoundException()
    {
        // Arrange
        var shortUrl = "invalidUrl";
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeWithClicksAsync(shortUrl)).ReturnsAsync((Link)null);

        // Act & Assert
        Assert.ThrowsAsync<LinkNotFoundException>(async () => await _linkService.GetStatsAsync(shortUrl));
    }
    
    [Test]
    public void GetStatsAsync_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var shortUrl = "someUrl";
        _mockLinkRepository.Setup(repo => repo.GetByShortCodeWithClicksAsync(shortUrl)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _linkService.GetStatsAsync(shortUrl));
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void GetStatsAsync_WithNullOrEmptyShortUrl_ThrowsArgumentException(string shortUrl)
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _linkService.GetStatsAsync(shortUrl));
    }
    
    [Test]
    public async Task GetStatsAsync_WithValidShortUrl_ReturnsLinkWithCorrectClickData()
    {
        // Arrange
        var shortUrl = "validUrl";
        var expectedClicksCount = 5;
        var linkWithClicks = new Link
        {
            Id = Guid.NewGuid(),
            Url = "https://example.com",
            ShortCode = shortUrl,
            CreatedAt = DateTimeOffset.UtcNow,
            Clicks = new List<Click>()
        };

        for (int i = 0; i < expectedClicksCount; i++)
        {
            linkWithClicks.Clicks.Add(new Click
            {
                Id = Guid.NewGuid(),
                LinkId = linkWithClicks.Id,
                CreatedAt = DateTimeOffset.UtcNow
            });
        }

        _mockLinkRepository.Setup(repo => repo.GetByShortCodeWithClicksAsync(shortUrl)).ReturnsAsync(linkWithClicks);

        // Act
        var result = await _linkService.GetStatsAsync(shortUrl);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Clicks.Count, Is.EqualTo(expectedClicksCount));
    }
}