using System;

namespace Moroshka.Xcp
{

/// <summary>
/// Represents an exception that provides detailed information about an argument being out of its valid range.
/// This class extends <see cref="DetailedException"/> to include parameter-specific details and the actual value that caused the exception.
/// </summary>
[Serializable]
public class ArgOutOfRangeException : DetailedException
{
	private const string ParamKey = "Param";
	private  const string ActualValueKey = "ActualValue";

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgOutOfRangeException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception. Default is "Specified argument was out of the range of valid values".</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ArgOutOfRangeException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Code = "ARG_OUT_OF_RANGE";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ArgOutOfRangeException"/> class with a default error message.
	/// </summary>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ArgOutOfRangeException(Exception innerException = null)
		: this("Specified argument was out of the range of valid values", innerException)
	{
	}

	/// <summary>
	/// Gets or sets the name of the parameter that was out of range.
	/// </summary>
	public string Param
	{
		get => (string)Data[ParamKey];
		set => Data[ParamKey] = value;
	}

	/// <summary>
	/// Gets or sets the actual value that was out of the valid range.
	/// </summary>
	public string ActualValue
	{
		get => (string)Data[ActualValueKey];
		set => Data[ActualValueKey] = value;
	}
}

}
