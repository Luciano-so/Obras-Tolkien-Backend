using TerraMedia.Domain.Exceptions;

namespace TerraMedia.Domain.Tests.Exceptions
{
    public class DomainExceptionTests
    {
        [Fact]
        public void Constructor_Default_ShouldCreateInstance()
        {
            var ex = new DomainException();
            Assert.NotNull(ex);        }

        [Fact]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            var message = "Erro de domínio";
            var ex = new DomainException(message);

            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
        {
            var message = "Erro com inner";
            var inner = new Exception("Inner exception");
            var ex = new DomainException(message, inner);

            Assert.Equal(message, ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }
    }
}
