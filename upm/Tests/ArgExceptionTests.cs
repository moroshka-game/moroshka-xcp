using System;
using NUnit.Framework;

namespace Moroshka.Xcp.Tests
{

[TestFixture]
internal sealed class ArgExceptionTests
{
	private const string TestMessage = "Test argument error message";
	private const string InnerTestMessage = "Test inner message";
	private const string TestContext = "Test context";
	private const string TestMemberName = "TestMethod";
	private const string TestLineNumber = "42";
	private const string TestParam = "TestParam";
	private const string CustomKey = "CustomKey";
	private const string CustomValue = "CustomValue";
	private const string ExpectedCode = "ARG_ERROR";

	[Test]
	public void Constructor_WithMessage()
	{
		// Arrange & Act
		var exception = new ArgException(TestMessage);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(TestMessage));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
		Assert.That(exception.Param, Is.Null);
		Assert.That(exception.InnerException, Is.Null);
	}

	[Test]
	public void Constructor_WithMessageAndInnerException()
	{
		// Arrange
		var innerException = new Exception(InnerTestMessage);

		// Act
		var exception = new ArgException(TestMessage, innerException);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(TestMessage));
		Assert.That(exception.InnerException, Is.EqualTo(innerException));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
		Assert.That(exception.Param, Is.Null);
	}

	[Test]
	public void Constructor_WithInnerExceptionOnly()
	{
		// Arrange
		var innerException = new Exception(InnerTestMessage);

		// Act
		var exception = new ArgException(innerException);

		// Assert
		Assert.That(exception.Message, Is.EqualTo("Invalid argument value"));
		Assert.That(exception.InnerException, Is.EqualTo(innerException));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
		Assert.That(exception.Param, Is.Null);
	}

	[Test]
	public void Constructor_WithNoParameters()
	{
		// Arrange & Act
		var exception = new ArgException();

		// Assert
		Assert.That(exception.Message, Is.EqualTo("Invalid argument value"));
		Assert.That(exception.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception.Context, Is.Null);
		Assert.That(exception.Member, Is.Null);
		Assert.That(exception.Line, Is.Null);
		Assert.That(exception.Param, Is.Null);
		Assert.That(exception.InnerException, Is.Null);
	}

	[Test]
	public void Constructor_WithAllPropertiesSet()
	{
		// Arrange && Act
		var innerException = new Exception();
		var testData = new { Key = CustomValue };
		var exception = new ArgException(TestMessage, innerException)
		{
			Context = TestContext,
			Member = TestMemberName,
			Line = TestLineNumber,
			Param = TestParam,
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
		Assert.That(exception.Param, Is.EqualTo(TestParam));
		Assert.That(exception.Data[CustomKey], Is.EqualTo(testData));
	}

	[Test]
	public void ToString_WithAllPropertiesSets_IncludesAllDetail()
	{
		// Arrange
		var innerException = new ArgumentException(InnerTestMessage);
		var exception = new ArgException(TestMessage, innerException)
		{
			Context = TestContext,
			Member = TestMemberName,
			Line = TestLineNumber,
			Param = TestParam,
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
		Assert.That(toStringResult, Does.Contain(TestParam));
		Assert.That(toStringResult, Does.Contain(CustomKey));
		Assert.That(toStringResult, Does.Contain(CustomValue));
		Assert.That(toStringResult, Does.Contain(InnerTestMessage));
	}

	[Test]
	public void ToString_WithEmptyProperties_ExcludesEmptyValues()
	{
		// Arrange
		var exception = new ArgException(TestMessage);

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Not.Contain("Context:"));
		Assert.That(toStringResult, Does.Not.Contain("Member:"));
		Assert.That(toStringResult, Does.Not.Contain("Line:"));
		Assert.That(toStringResult, Does.Not.Contain("Param:"));
	}

	[Test]
	public void ToString_WithNullDataValues_ExcludesNullValues()
	{
		// Arrange
		var exception = new ArgException(TestMessage)
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
		var level1Exception = new ArgException(TestMessage, level2Exception);

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
		ArgException exception;
		try
		{
			throw new ArgException(TestMessage);
		}
		catch (ArgException ex)
		{
			exception = ex;
		}

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain("--ArgException"));
		Assert.That(toStringResult, Does.Contain("at "));
	}

	[Test]
	public void ToString_WithCustomData_IncludesCustomData()
	{
		// Arrange
		var exception = new ArgException(TestMessage)
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
		var exception = new ArgException(TestMessage)
		{
			Context = TestContext,
			Member = TestMemberName,
			Line = TestLineNumber,
			Param = TestParam
		};

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Contain($"Member: \"{TestMemberName}\""));
		Assert.That(toStringResult, Does.Contain($"Line: \"{TestLineNumber}\""));
		Assert.That(toStringResult, Does.Contain($"Context: \"{TestContext}\""));
		Assert.That(toStringResult, Does.Contain($"Param: \"{TestParam}\""));
	}

	[Test]
	public void ToString_WithNoAdditionalData_ExcludesEmptyBrackets()
	{
		// Arrange
		var exception = new ArgException(TestMessage);

		// Act
		var toStringResult = exception.ToString();

		// Assert
		Assert.That(toStringResult, Does.Contain(TestMessage));
		Assert.That(toStringResult, Does.Contain(ExpectedCode));
		Assert.That(toStringResult, Does.Not.Contain("[]"));
		Assert.That(toStringResult, Does.Not.Contain("Context:"));
		Assert.That(toStringResult, Does.Not.Contain("Member:"));
		Assert.That(toStringResult, Does.Not.Contain("Line:"));
		Assert.That(toStringResult, Does.Not.Contain("Param:"));
	}

	[Test]
	public void Inheritance_FromDetailedException_WorksCorrectly()
	{
		// Arrange & Act
		var exception = new ArgException(TestMessage);

		// Assert
		Assert.That(exception, Is.InstanceOf<DetailedException>());
		Assert.That(exception, Is.InstanceOf<Exception>());
		Assert.That(exception, Is.InstanceOf<ArgException>());
	}

	[Test]
	public void Code_IsAlwaysSetToArgError()
	{
		// Arrange & Act
		var exception1 = new ArgException(TestMessage);
		var exception2 = new ArgException();
		var exception3 = new ArgException(new Exception());

		// Assert
		Assert.That(exception1.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception2.Code, Is.EqualTo(ExpectedCode));
		Assert.That(exception3.Code, Is.EqualTo(ExpectedCode));
	}
}

}
