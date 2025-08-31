using System;

namespace Moroshka.Xcp
{

/// <summary>
/// Represents an exception that provides detailed information about a null argument error.
/// This class extends <see cref="DetailedException"/> to include parameter-specific details.
/// </summary>
[Serializable]
public class ArgNullException : DetailedException
{
	private const string ParamKey = "Param";

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgNullException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception. Default is "Value cannot be null".</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ArgNullException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Code = "ARG_NULL";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgNullException"/> class with a default error message.
	/// </summary>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ArgNullException(Exception innerException = null)
		: this("Value cannot be null", innerException)
	{
	}

	/// <summary>
	/// Gets or sets the name or value of the null argument that caused the exception.
	/// </summary>
	public string Param
	{
		get => (string)Data[ParamKey];
		set => Data[ParamKey] = value;
	}
}

}
