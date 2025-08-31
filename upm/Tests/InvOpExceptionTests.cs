using System;
using NUnit.Framework;

namespace Moroshka.Xcp.Tests
{

[TestFixture]
internal sealed class InvOpExceptionTests
{
	private const string TestMessage = "Test invalid operation error message";
	private const string InnerTestMessage = "Test inner message";
	private const string TestContext = "Test context";
	private const string TestMemberName = "TestMethod";
	private const string TestLineNumber = "42";
	private const string CustomKey = "CustomKey";
	private const string CustomValue = "CustomValue";
	private const string ExpectedCode = "INVALID_OPERATION";
	private const string DefaultMessage = "Operation is not valid due to the current state of the object.";

	[Test]
	public void Constructor_WithMessage()
	{
		// Arrange & Act
		var exception = new InvOpException(TestMessage);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(TestMessage));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
		Assert.That(exception.InnerException, Is.Null);
	}

	[Test]
	public void Constructor_WithMessageAndInnerException()
	{
		// Arrange
		var innerException = new Exception(InnerTestMessage);

		// Act
		var exception = new InvOpException(TestMessage, innerException);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(TestMessage));
		Assert.That(exception.InnerException, Is.EqualTo(innerException));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
	}

	[Test]
	public void Constructor_WithInnerExceptionOnly()
	{
		// Arrange
		var innerException = new Exception(InnerTestMessage);

		// Act
		var exception = new InvOpException(innerException);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(DefaultMessage));
		Assert.That(exception.InnerException, Is.EqualTo(innerException));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
	}

	[Test]
	public void Constructor_WithNoParameters()
	{
		// Arrange & Act
		var exception = new InvOpException();

		// Assert
		Assert.That(exception.Message, Is.EqualTo(DefaultMessage));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
		Assert.That(exception.InnerException, Is.Null);
	}

	[Test]
	public void Constructor_WithAllPropertiesSet()
	{
		// Arrange && Act
		var innerException = new Exception();
		var testData = new { Key = CustomValue };
		var exception = new InvOpException(TestMessage, innerException)
		{
			Context = TestContext,
			Member = TestMemberName,
			Line = TestLineNumber,
			Data =
			{
				[CustomKey] = testData
			}
		};

		// Assert
		Assert.That(exception.Message, Is.EqualTo(TestMessage));
		Assert.That(exception.InnerException, Is.EqualTo(innerException));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.EqualTo(TestContext));
		Assert.That(exception.Member, Is.EqualTo(TestMemberName));
		Assert.That(exception.Line, Is.EqualTo(TestLineNumber));
		Assert.That(exception.Data[CustomKey], Is.EqualTo(testData));
	}

	[Test]
	public void ToString_WithAllPropertiesSets_IncludesAllDetail()
	{
		// Arrange
		var innerException = new ArgumentException(InnerTestMessage);
		var exception = new InvOpException(TestMessage, innerException)
		{
			Context = TestContext,
			Member = TestMemberName,
			Line = TestLineNumber,
			Data =
			{
				[CustomKey] = CustomValue
			}
		};

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Contain(TestContext));
		Assert.That(toStringResult, Does.Contain(TestMemberName));
		Assert.That(toStringResult, Does.Contain(TestLineNumber));
		Assert.That(toStringResult, Does.Contain(CustomKey));
		Assert.That(toStringResult, Does.Contain(CustomValue));
		Assert.That(toStringResult, Does.Contain(InnerTestMessage));
	}

	[Test]
	public void ToString_WithEmptyProperties_ExcludesEmptyValues()
	{
		// Arrange
		var exception = new InvOpException(TestMessage);

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Not.Contain("Context:"));
		Assert.That(toStringResult, Does.Not.Contain("Member:"));
		Assert.That(toStringResult, Does.Not.Contain("Line:"));
	}

	[Test]
	public void ToString_WithNullDataValues_ExcludesNullValues()
	{
		// Arrange
		var exception = new InvOpException(TestMessage)
		{
			Data =
			{
				["NullKey"] = null
			}
		};

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Not.Contain("NullKey"));
	}

	[Test]
	public void ToString_WithComplexExceptionChain_IncludesAllExceptions()
	{
		// Arrange
		var level3Exception = new InvalidOperationException("Level 3");
		var level2Exception = new ArgumentException("Level 2", level3Exception);
		var level1Exception = new InvOpException(TestMessage, level2Exception);

		// Act
		var toStringResult = level1Exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain("Level 2"));
		Assert.That(toStringResult, Does.Contain("Level 3"));
		Assert.That(toStringResult, Does.Contain("--->"));
	}

	[Test]
	public void ToString_WithStackTrace_IncludesStackTrace()
	{
		// Arrange
		InvOpException exception;
		try
		{
			throw new InvOpException(TestMessage);
		}
		catch (InvOpException ex)
		{
			exception = ex;
		}

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain("--InvOpException"));
		Assert.That(toStringResult, Does.Contain("at "));
	}

	[Test]
	public void ToString_WithCustomData_IncludesCustomData()
	{
		// Arrange
		var exception = new InvOpException(TestMessage)
		{
			Data =
			{
				["UserId"] = 12345,
				["Operation"] = "TestOperation"
			}
		};

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain("UserId: \"12345\""));
		Assert.That(toStringResult, Does.Contain("Operation: \"TestOperation\""));
	}

	[Test]
	public void ToString_WithPredefinedKeys_IncludesOnlyNonEmptyValues()
	{
		// Arrange
		var exception = new InvOpException(TestMessage)
		{
			Context = TestContext,
			Member = TestMemberName,
			Line = TestLineNumber
		};

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Contain($"Member: \"{TestMemberName}\""));
		Assert.That(toStringResult, Does.Contain($"Line: \"{TestLineNumber}\""));
		Assert.That(toStringResult, Does.Contain($"Context: \"{TestContext}\""));
	}

	[Test]
	public void ToString_WithNoAdditionalData_ExcludesEmptyBrackets()
	{
		// Arrange
		var exception = new InvOpException(TestMessage);

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Not.Contain("[]"));
		Assert.That(toStringResult, Does.Not.Contain("Context:"));
		Assert.That(toStringResult, Does.Not.Contain("Member:"));
		Assert.That(toStringResult, Does.Not.Contain("Line:"));
	}

	[Test]
	public void Inheritance_FromDetailedException_WorksCorrectly()
	{
		// Arrange & Act
		var exception = new InvOpException(TestMessage);

		// Assert
		Assert.That(exception, Is.InstanceOf<DetailedException>());
		Assert.That(exception, Is.InstanceOf<Exception>());
		Assert.That(exception, Is.InstanceOf<InvOpException>());
	}

	[Test]
	public void Code_IsAlwaysSetToInvalidOperation()
	{
		// Arrange & Act
		var exception1 = new InvOpException(TestMessage);
		var exception2 = new InvOpException();
		var exception3 = new InvOpException(new Exception());

		// Assert
		Assert.That(exception1.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception2.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception3.Code, Is.EqualTo(ExpectedCode));
	}
}

}
