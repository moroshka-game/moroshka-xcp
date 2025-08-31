using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Moroshka.Xcp
{

/// <summary>
/// An exception that contains detailed information about an error.
/// This class extends the standard Exception class with additional context information
/// such as error codes, context details, member names, and line numbers.
/// </summary>
[Serializable]
public class DetailedException : Exception
{
	private const string CodeKey = "Code";
	private const string ContextKey = "Context";
	private const string MemberKey = "Member";
	private const string LineKey = "Line";

	/// <summary>
	/// Initializes a new instance of the <see cref="DetailedException"/> class with an error message and optional inner exception.
	/// </summary>
	/// <param name="message">The error message that describes the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
	public DetailedException(string message, Exception innerException = null)
		: base(message, innerException)
	{
	}

	/// <summary>
	/// Gets the data dictionary associated with this exception.
	/// This property is sealed to prevent further overriding.
	/// </summary>
	/// <returns>A dictionary containing additional exception data.</returns>
	public sealed override IDictionary Data => base.Data;

	/// <summary>
	/// Gets or sets a unique error code for identifying the type of problem.
	/// This code can be used for error categorization and handling.
	/// </summary>
	/// <value>A string representing the error code. Empty string if not set.</value>
	public string Code
	{
		get => (string)Data[CodeKey];
		set => Data[CodeKey] = value;
	}

	/// <summary>
	/// Gets or sets additional context information about the error.
	/// This can include details about the operation being performed when the error occurred.
	/// </summary>
	/// <value>A string containing context information. Empty string if not set.</value>
	public string Context
	{
		get => (string)Data[ContextKey];
		set => Data[ContextKey] = value;
	}

	/// <summary>
	/// Gets or sets the name of the member (method, property, etc.) where the error occurred.
	/// This helps identify the specific location in the code where the exception was thrown.
	/// </summary>
	/// <value>The name of the member where the error occurred. Empty string if not set.</value>
	public string Member
	{
		get => (string)Data[MemberKey];
		set => Data[MemberKey] = value;
	}

	/// <summary>
	/// Gets or sets the line number where the error occurred.
	/// This provides precise location information for debugging purposes.
	/// </summary>
	/// <value>The line number where the error occurred. Empty string if not set.</value>
	public string Line
	{
		get => (string)Data[LineKey];
		set => Data[LineKey] = value;
	}

	/// <summary>
	/// Returns a string representation of the exception with additional context information.
	/// This includes error codes, context details, member names, line numbers, and stack traces.
	/// </summary>
	/// <returns>A formatted string containing the complete exception details.</returns>
	public override string ToString()
	{
		var sb = new StringBuilder();
		BuildExceptionString(this, sb, 0);
		return sb.ToString();
	}

	private static void BuildExceptionString(Exception ex, StringBuilder sb, int depth)
	{
		var type = ex.GetType();
		var className = type.FullName ?? type.Name;
		var message = ex.Message;
		sb.Append(className);
		sb.Append(": ");
		sb.Append(message);
		sb.AppendLine();

		BuildAdditionalInfoString(ex, sb);

		if (ex.InnerException != null)
		{
			sb.Append("---> ");
			BuildExceptionString(ex.InnerException, sb, depth + 1);
		}

		if (depth == 0) BuildStackTracesString(ex, sb);
	}

	private static void BuildAdditionalInfoString(Exception exception, StringBuilder sb)
	{
		var properties = new List<string>();

		foreach (DictionaryEntry entry in exception.Data)
		{
			if (entry.Value == null) continue;
			var valueString = entry.Value.ToString();
			if (string.IsNullOrEmpty(valueString)) continue;
			properties.Add($"{entry.Key}: \"{entry.Value}\"");
		}

		if (properties.Count <= 0) return;
		sb.Append('[');
		sb.Append(string.Join(", ", properties));
		sb.Append(']');
		sb.AppendLine();
	}

	private static void BuildStackTracesString(Exception exception, StringBuilder sb)
	{
		var currentException = exception;
		var hasStackTraces = false;

		while (currentException != null)
		{
			if (currentException.StackTrace != null)
			{
				if (!hasStackTraces) hasStackTraces = true;
				sb.Append("--");
				sb.Append(currentException.GetType().Name);
				sb.Append('\n');
				var stackTraceLines = currentException.StackTrace.Split('\n');
				foreach (var line in stackTraceLines)
				{
					var trimmedLine = line.TrimEnd();
					if (!string.IsNullOrEmpty(trimmedLine)) sb.AppendLine(trimmedLine);
				}
			}

			currentException = currentException.InnerException;
		}

		if (hasStackTraces) sb.Length -= Environment.NewLine.Length;
	}
}

}
