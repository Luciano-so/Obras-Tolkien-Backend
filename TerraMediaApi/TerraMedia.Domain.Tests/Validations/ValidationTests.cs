using TerraMedia.Domain.Exceptions;
using TerraMedia.Domain.Validations;

namespace TerraMedia.Domain.Tests.Validations
{
    public class ValidationTests
    {
        [Fact]
        public void ValidateIfEqual_ShouldThrow_WhenObjectsAreEqual()
        {
            var obj1 = "test";
            var obj2 = "test";

            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateIfEqual(obj1, obj2, "Valores iguais não são permitidos"));

            Assert.Equal("Valores iguais não são permitidos", ex.Message);
        }

        [Fact]
        public void ValidateIfEqual_ShouldNotThrow_WhenObjectsAreDifferent()
        {
            Validation.ValidateIfEqual("a", "b", "Erro");
        }

        [Theory]
        [InlineData("abcd", 2, 5)]
        [InlineData("  abc  ", 3, 5)] 
        public void ValidateLength_ShouldNotThrow_WhenLengthValid(string value, int min, int max)
        {
            Validation.ValidateLength(value, min, max, "Erro");
        }

        [Theory]
        [InlineData("a", 2, 5)]
        [InlineData("abcdef", 2, 5)]
        public void ValidateLength_ShouldThrow_WhenLengthInvalid(string value, int min, int max)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateLength(value, min, max, "Tamanho deve estar entre {0} e {1}"));

            Assert.Equal($"Tamanho deve estar entre {min} e {max}", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateIfEmpty_ShouldThrow_WhenValueEmptyOrNull(string value)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateIfEmpty(value, "Campo vazio"));

            Assert.Equal("Campo vazio", ex.Message);
        }

        [Fact]
        public void ValidateIfEmpty_ShouldNotThrow_WhenValueNotEmpty()
        {
            Validation.ValidateIfEmpty("abc", "Erro");
        }

        [Fact]
        public void ValidateIfNull_ShouldThrow_WhenObjectNull()
        {
            object obj = null!;
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateIfNull(obj, "Objeto é nulo"));

            Assert.Equal("Objeto é nulo", ex.Message);
        }

        [Fact]
        public void ValidateIfNull_ShouldNotThrow_WhenObjectNotNull()
        {
            object obj = new object();
            Validation.ValidateIfNull(obj, "Erro");
        }

        [Theory]
        [InlineData(5, 1, 10)]
        [InlineData(1, 1, 10)]
        [InlineData(10, 1, 10)]
        public void ValidateMinMax_Int_ShouldNotThrow_WhenValueInRange(int value, int min, int max)
        {
            Validation.ValidateMinMax(value, min, max, "Erro");
        }

        [Theory]
        [InlineData(0, 1, 10)]
        [InlineData(11, 1, 10)]
        public void ValidateMinMax_Int_ShouldThrow_WhenValueOutOfRange(int value, int min, int max)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateMinMax(value, min, max, "Fora do intervalo"));

            Assert.Equal("Fora do intervalo", ex.Message);
        }

        [Theory]
        [InlineData(5.5, 1.0, 10.0)]
        [InlineData(1.0, 1.0, 10.0)]
        [InlineData(10.0, 1.0, 10.0)]
        public void ValidateMinMax_Decimal_ShouldNotThrow_WhenValueInRange(decimal value, decimal min, decimal max)
        {
            Validation.ValidateMinMax(value, min, max, "Erro");
        }

        [Theory]
        [InlineData(0.9, 1.0, 10.0)]
        [InlineData(10.1, 1.0, 10.0)]
        public void ValidateMinMax_Decimal_ShouldThrow_WhenValueOutOfRange(decimal value, decimal min, decimal max)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateMinMax(value, min, max, "Fora do intervalo"));

            Assert.Equal("Fora do intervalo", ex.Message);
        }

        [Fact]
        public void ValidateIfFalse_ShouldThrow_WhenFalse()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateIfFalse(false, "Valor é falso"));

            Assert.Equal("Valor é falso", ex.Message);
        }

        [Fact]
        public void ValidateIfFalse_ShouldNotThrow_WhenTrue()
        {
            Validation.ValidateIfFalse(true, "Erro");
        }

        [Fact]
        public void ValidateIfTrue_ShouldThrow_WhenTrue()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validation.ValidateIfTrue(true, "Valor é verdadeiro"));

            Assert.Equal("Valor é verdadeiro", ex.Message);
        }

        [Fact]
        public void ValidateIfTrue_ShouldNotThrow_WhenFalse()
        {
            Validation.ValidateIfTrue(false, "Erro");
        }
    }
}
