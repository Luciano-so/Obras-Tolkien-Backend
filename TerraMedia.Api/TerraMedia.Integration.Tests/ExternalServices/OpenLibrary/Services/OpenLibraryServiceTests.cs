using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraMedia.Application.Dtos.OpenLibrary;
using TerraMedia.Application.Interfaces;
using TerraMedia.ExternalServices.OpenLibrary.Services;

namespace TerraMedia.Integration.Tests.ExternalServices.OpenLibrary.Services
{
    public class OpenLibraryServiceTests
    {
        private readonly Mock<IOpenLibraryClient> _clientMock;
        private readonly OpenLibraryService _service;

        public OpenLibraryServiceTests()
        {
            _clientMock = new Mock<IOpenLibraryClient>();
            _service = new OpenLibraryService(_clientMock.Object);
        }

        [Fact]
        public async Task SearchBooksAsync_WithValidResult_ShouldMapFields()
        {
            var searchDto = new OpenLibrarySearchDto
            {
                Docs = new List<OpenLibraryBookDto>
            {
                new OpenLibraryBookDto
                {
                    Cover_edition_key = "OL123M",
                    Author_name = new List<string> { "Author One", "Author Two" },
                    Author_key = new List<string> { "Key1", "Key2" }
                }
            }
            };

            _clientMock
                .Setup(c => c.SearchBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchDto);

            _clientMock
                .Setup(c => c.GetCoverUrl("OL123M", "M"))
                .Returns("https://covers.openlibrary.org/b/OL123M-M.jpg");

            var result = await _service.SearchBooksAsync("Some Author");

            Assert.NotNull(result);
            Assert.NotEmpty(result.Docs);

            var book = result.Docs[0];
            Assert.Equal("https://covers.openlibrary.org/b/OL123M-M.jpg", book.CoverUrl);
            Assert.Equal("Author One, Author Two", book.Authors);
            Assert.Equal("Key1, Key2", book.Key);
        }

        [Fact]
        public async Task SearchBooksAsync_WithNullDocs_ShouldReturnEmptyDto()
        {
            var searchDto = new OpenLibrarySearchDto
            {
                Docs = new List<OpenLibraryBookDto>()
                        {
                            new OpenLibraryBookDto
                            {
                                Title = "Domain-Driven Design",
                                Author_name = new List<string> { "Eric Evans" },
                                First_publish_year = 2003,
                                Edition_count = 3,
                                Cover_i = 123456,
                                Cover_edition_key = "OL123M",
                                Author_key = new List<string> { "OL1A" }
                            },
                            new OpenLibraryBookDto
                            {
                                Title = "Clean Code",
                                Author_name = new List<string> { "Robert C. Martin" },
                                First_publish_year = 2008,
                                Edition_count = 5,
                                Cover_i = 654321,
                                Cover_edition_key = "OL456M",
                                Author_key = new List<string> { "OL2A" }
                            }
                        }
            };

            _clientMock
                .Setup(c => c.SearchBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchDto);

            var result = await _service.SearchBooksAsync("Some Author");

            Assert.NotNull(result);
            Assert.NotNull(result.Docs);
            Assert.True(result.Docs.Count == 2);
        }

        [Fact]
        public async Task GetAuthorBioAsync_WithNullResult_ShouldReturnEmptyDto()
        {
            _clientMock
                .Setup(c => c.GetAuthorBioAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((OpenLibraryAuthorDto?)null);

            var result = await _service.GetAuthorBioAsync("OL1A");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAuthorBioAsync_WithValidResult_ShouldReturnDto()
        {
            var authorDto = new OpenLibraryAuthorDto
            {
                Name = "Author Name",
                Bio = "Biography"
            };

            _clientMock
                .Setup(c => c.GetAuthorBioAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authorDto);

            var result = await _service.GetAuthorBioAsync("OL1A");

            Assert.NotNull(result);
            Assert.Equal("Author Name", result.Name);
            Assert.Equal("Biography", result.Bio);
        }

        [Fact]
        public async Task SearchBooksAsync_WithNullDocs_ShouldReturnEmptyDocDto()
        {
            var searchDto = new OpenLibrarySearchDto { Docs = null };

            _clientMock
                .Setup(c => c.SearchBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchDto);

            var result = await _service.SearchBooksAsync("Some Author");

            Assert.NotNull(result);
            Assert.True(result.Docs.Count == 0);
        }

        [Fact]
        public async Task SearchBooksAsync_WithNullDocs_ShouldReturnresutNullDocDto()
        {
            var result = await _service.SearchBooksAsync("Some Author");

            Assert.NotNull(result);
        }
    }
}