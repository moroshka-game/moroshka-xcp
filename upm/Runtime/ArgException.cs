using System;

namespace Moroshka.Xcp
{

/// <summary>
/// Represents an exception that provides detailed information about an argument-related error.
/// This class extends <see cref="DetailedException"/> to include parameter-specific details.
/// </summary>
[Serializable]
public class ArgException : DetailedException
{
	private const string ParamKey = "Param";

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ArgException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Code = "ARG_ERROR";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgException"/> class with a default error message.
	/// </summary>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ArgException(Exception innerException = null)
		: this("Invalid argument value", innerException)
	{
	}

	/// <summary>
	/// Gets or sets the name or value of the argument that caused the exception.
	/// </summary>
	public string Param
	{
		get => (string)Data[ParamKey];
		set => Data[ParamKey] = value;
	}
}

}
