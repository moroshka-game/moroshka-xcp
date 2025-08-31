using System;

namespace Moroshka.Xcp
{

/// <summary>
/// Represents an exception that provides detailed information about an invalid operation.
/// This class extends <see cref="DetailedException"/> to include context-specific details.
/// </summary>
[Serializable]
public class InvOpException : DetailedException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvOpException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public InvOpException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Code = "INVALID_OPERATION";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InvOpException"/> class with a default error message.
	/// </summary>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public InvOpException(Exception innerException = null)
		: this("Operation is not valid due to the current state of the object.", innerException)
	{
	}
}

}
