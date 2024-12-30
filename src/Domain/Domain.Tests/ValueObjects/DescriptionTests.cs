using Domain.ValueObjects;
using Xunit;
using static Common.Resources.Constants.DescriptionConstants;
using Domain.ValueObjects.Exceptions.DescriptionsExceptions;

namespace Domain.Tests.ValueObjects;
    public class DescriptionTests
    {
        private readonly string _defaultDescription;
        private readonly string _nullableDescription;
        private readonly string _emptyDescription;
        private readonly string _whiteSpacesDescription;
        private readonly string _lessThanMinLengthDescription;
        private readonly string _biggerThanMaxLengthDescription;

        public DescriptionTests()
        {
            _defaultDescription = "Default Title";
            _nullableDescription = null;
            _emptyDescription = string.Empty;
            _whiteSpacesDescription = "      ";
            _lessThanMinLengthDescription = new string('x', DESCRIPTION_MIN_LENGTH - 1);
            _biggerThanMaxLengthDescription = new string('x', DESCRIPTION_MAX_LENGTH + 1);
        }

        [Fact]
        public void Constructor_WhenIsValidDescription_ShouldCreatingDescription()
        {
            // Arrange
            var expectedTitle = _defaultDescription;

            // Act
            var actual = new Description(expectedTitle);

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Value);
            Assert.Equal(expectedTitle, actual.Value);
        }

        [Fact]
        public void Constructor_WhenDescriptionIsNull_ShouldThrowDescriptionEmptyException()
        {
            // Arrange
            var expectedTitle = _nullableDescription;

            // Act
            var actual = () => new Description(expectedTitle);

            // Assert
            Assert.Throws<DescriptionEmptyException>(actual);
        }

        [Fact]
        public void Constructor_WhenDescriptionIsEmpty_ShouldThrowDescriptionEmptyException()
        {
            // Arrange
            var expectedTitle = _emptyDescription;

            // Act
            var actual = () => new Description(expectedTitle);

            // Assert
            Assert.Throws<DescriptionEmptyException>(actual);
        }

        [Fact]
        public void Constructor_WhenDescriptionIsWhiteSpaces_ShouldThrowDescriptionEmptyException()
        {
            // Arrange
            var expectedTitle = _whiteSpacesDescription;

            // Act
            var actual = () => new Description(expectedTitle);

            // Assert
            Assert.Throws<DescriptionEmptyException>(actual);
        }

        [Fact]
        public void Constructor_WhenDescriptionLengthIsLessThanMinLengthDescription_ShouldDescriptionMinLengthException()
        {
            // Arrange
            var expectedTitle = _lessThanMinLengthDescription;

            // Act
            var actual = () => new Description(expectedTitle);

            // Assert
            Assert.Throws<DescriptionMinLengthException>(actual);
        }

        [Fact]
        public void Constructor_WhenDescriptionLengthIsBiggerThanMaxLengthDescription_ShouldDescriptionMaxLengthException()
        {
            // Arrange
            var expectedTitle = _biggerThanMaxLengthDescription;

            // Act
            var actual = () => new Description(expectedTitle);

            // Assert
            Assert.Throws<DescriptionMaxLengthException>(actual);
        }

    }