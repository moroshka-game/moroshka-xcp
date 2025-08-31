using System;

namespace Moroshka.Xcp
{

/// <summary>
/// Represents an exception that provides detailed information about an error related to the use of an object after it has been released.
/// This class extends <see cref="DetailedException"/> to include parameter-specific details.
/// </summary>
[Serializable]
public class ObjDisposedException : DetailedException
{
	private const string ObjectKey = "Object";

	/// <summary>
	/// Initializes a new instance of the <see cref="ObjDisposedException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception. Default is "The object has been disposed and cannot be used.".</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ObjDisposedException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Code = "OBJ_DISPOSED";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ObjDisposedException"/> class with default error message.
	/// </summary>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public ObjDisposedException(Exception innerException = null)
		: this("The object has been disposed and cannot be used.", innerException)
	{
	}

	/// <summary>
	/// Gets or sets the name or identifier of the disposed object that caused the exception.
	/// </summary>
	public string Object
	{
		get => (string)Data[ObjectKey];
		set => Data[ObjectKey] = value;
	}
}

}
