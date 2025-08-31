# Usage Instructions

- [Usage Instructions](#usage-instructions)
  - [Introduction](#introduction)
  - [Core Concepts](#core-concepts)
    - [DetailedException](#detailedexception)
    - [Automatic Error Codes](#automatic-error-codes)
  - [Basic Usage](#basic-usage)
    - [Simple Exception with Context](#simple-exception-with-context)
    - [Exception with Additional Data](#exception-with-additional-data)
  - [Additional Properties](#additional-properties)
    - [Code (Error Code)](#code-error-code)
    - [Context](#context)
    - [Member](#member)
    - [Line](#line)
  - [Specialized Exceptions](#specialized-exceptions)
    - [ArgNullException](#argnullexception)
    - [ArgOutOfRangeException](#argoutofrangeexception)
    - [InvOpException](#invopexception)
    - [ObjDisposedException](#objdisposedexception)
  - [Exception Chains](#exception-chains)
    - [Creating Exception Chains](#creating-exception-chains)
    - [ToString() Result for Chains](#tostring-result-for-chains)
  - [Best Practices](#best-practices)
    - [1. Always Specify Context](#1-always-specify-context)
    - [2. Use Meaningful Error Codes](#2-use-meaningful-error-codes)
    - [3. Preserve Exception Chains](#3-preserve-exception-chains)
    - [4. Use Data for Additional Information](#4-use-data-for-additional-information)
    - [5. Create Specialized Exception Classes](#5-create-specialized-exception-classes)

## Introduction

`Moroshka.Xcp` is a library that provides enhanced exception classes to improve standard error handling mechanisms in .NET. These exceptions contain additional information such as:

- **Unique error codes** (`Code`) for categorizing issues
- **Contextual information** (`Context`) about the operation being performed
- **Method names** (`Member`) where the error occurred
- **Line numbers** (`Line`) for precise error location
- **Additional data** through the `Data` property

This makes debugging and logging more informative and structured.

## Core Concepts

### DetailedException

The base class `DetailedException` extends the standard `Exception` and provides additional properties for detailed error information.

```csharp
using Moroshka.Xcp;

// Creating a basic detailed exception
var exception = new DetailedException("An error occurred")
{
    Code = "CUSTOM_ERROR",
    Context = "MyService",
    Member = "ProcessData",
    Line = "42"
};

throw exception;
```

### Automatic Error Codes

Specialized exceptions automatically set appropriate error codes:

- `ArgNullException` → `"ARG_NULL"`
- `ArgOutOfRangeException` → `"ARG_OUT_OF_RANGE"`
- `InvOpException` → `"INVALID_OPERATION"`
- `ObjDisposedException` → `"OBJECT_DISPOSED"`

## Basic Usage

### Simple Exception with Context

```csharp
using Moroshka.Xcp;

public void ValidateUser(string username)
{
    if (string.IsNullOrEmpty(username))
    {
        throw new ArgNullException()
        {
            Context = nameof(UserService),
            Member = nameof(ValidateUser),
            Line = "12",
            Param = nameof(username)
        };
    }
}
```

### Exception with Additional Data

```csharp
public void ProcessAge(int age)
{
    if (age < 0 || age > 150)
    {
        throw new ArgOutOfRangeException()
        {
            Context = nameof(UserService),
            Member = nameof(ProcessAge),
            Line = "25",
            Param = nameof(age),
            ActualValue = age.ToString(),
            Data =
            {
                ["MinValue"] = "0",
                ["MaxValue"] = "150",
                ["Timestamp"] = DateTime.Now.ToString()
            }
        };
    }
}
```

## Additional Properties

### Code (Error Code)

Unique identifier for the error type for categorization and handling:

```csharp
var exception = new DetailedException("Database connection failed")
{
    Code = "DB_CONNECTION_FAILED"
};
```

### Context

Information about the execution context, usually the class or service name:

```csharp
var exception = new DetailedException("Authentication error")
{
    Context = "AuthenticationService"
};
```

### Member

Name of the method, property, or other class member where the error occurred:

```csharp
var exception = new DetailedException("Validation error")
{
    Member = "ValidateUserInput"
};
```

### Line

Line number in the code where the error occurred (usually set manually):

```csharp
var exception = new DetailedException("Unexpected error")
{
    Line = "127"
};
```

## Specialized Exceptions

### ArgNullException

For handling null arguments:

```csharp
public void ProcessUser(User user)
{
    if (user == null)
    {
        throw new ArgNullException()
        {
            Context = nameof(UserProcessor),
            Member = nameof(ProcessUser),
            Param = nameof(user)
        };
    }
}
```

### ArgOutOfRangeException

For values outside the valid range:

```csharp
public void SetVolume(int volume)
{
    if (volume < 0 || volume > 100)
    {
        throw new ArgOutOfRangeException()
        {
            Context = nameof(AudioPlayer),
            Member = nameof(SetVolume),
            Param = nameof(volume),
            ActualValue = volume.ToString()
        };
    }
}
```

### InvOpException

For invalid operations:

```csharp
public void StartService()
{
    if (_isRunning)
    {
        throw new InvOpException("Service is already running")
        {
            Context = nameof(BackgroundService),
            Member = nameof(StartService),
            Data = { ["ServiceState"] = "Running" }
        };
    }
}
```

### ObjDisposedException

For using disposed objects:

```csharp
public void SendMessage(string message)
{
    if (_disposed)
    {
        throw new ObjDisposedException(nameof(MessageSender))
        {
            Context = nameof(MessageSender),
            Member = nameof(SendMessage)
        };
    }
}
```

## Exception Chains

### Creating Exception Chains

```csharp
public void ProcessData()
{
    try
    {
        LowLevelOperation();
    }
    catch (Exception e)
    {
        // Wrap low-level exception in more contextual one
        throw new InvOpException("Data processing error", e)
        {
            Context = nameof(DataProcessor),
            Member = nameof(ProcessData),
            Line = "45"
        };
    }
}

private void LowLevelOperation()
{
    throw new ArgNullException()
    {
        Context = nameof(DataProcessor),
        Member = nameof(LowLevelOperation),
        Param = "data"
    };
}
```

### ToString() Result for Chains

``` text
Moroshka.Xcp.InvOpException: Data processing error
[Code: "INVALID_OPERATION", Context: "DataProcessor", Member: "ProcessData", Line: "45"]
---> Moroshka.Xcp.ArgNullException: Value cannot be null
[Code: "ARG_NULL", Context: "DataProcessor", Member: "LowLevelOperation", Param: "data"]
--InvOpException
  at DataProcessor.ProcessData() in Program.cs:line 45
--ArgNullException
  at DataProcessor.LowLevelOperation() in Program.cs:line 60
```

## Best Practices

### 1. Always Specify Context

```csharp
// ✅ Good
throw new ArgNullException()
{
    Context = nameof(UserService),
    Member = nameof(ValidateUser),
    Param = nameof(username)
};

// ❌ Bad
throw new ArgNullException();
```

### 2. Use Meaningful Error Codes

```csharp
// ✅ Good
var exception = new DetailedException("User not found")
{
    Code = "USER_NOT_FOUND"
};

// ❌ Bad
var exception = new DetailedException("Error")
{
    Code = "ERROR"
};
```

### 3. Preserve Exception Chains

```csharp
// ✅ Good
try
{
    SomeOperation();
}
catch (Exception e)
{
    throw new InvOpException("Operation failed", e)
    {
        Context = nameof(MyService)
    };
}

// ❌ Bad
try
{
    SomeOperation();
}
catch (Exception e)
{
    throw new InvOpException("Operation failed")
    {
        Context = nameof(MyService)
    };
}
```

### 4. Use Data for Additional Information

```csharp
// ✅ Good
throw new ArgOutOfRangeException()
{
    Param = nameof(age),
    ActualValue = age.ToString(),
    Data =
    {
        ["MinValue"] = "0",
        ["MaxValue"] = "150",
        ["UserInput"] = userInput
    }
};
```

### 5. Create Specialized Exception Classes

```csharp
/// <summary>
/// Exception for cases when user is not found
/// </summary>
public class UserNotFoundException : ArgNullException
{
    public UserNotFoundException(string userId, Exception innerException = null)
        : base("User not found", innerException)
    {
        Code = "USER_NOT_FOUND";
        Context = "UserService";
        Param = "userId";
        Data["UserId"] = userId;
    }
}

/// <summary>
/// Exception for invalid user age
/// </summary>
public class InvalidAgeException : ArgOutOfRangeException
{
    public InvalidAgeException(int age, Exception innerException = null)
        : base("Invalid age", innerException)
    {
        Code = "INVALID_AGE";
        Context = "UserService";
        Param = "age";
        ActualValue = age.ToString();
        Data["MinAge"] = "18";
        Data["MaxAge"] = "120";
        Data["ProvidedAge"] = age.ToString();
    }
}

// Usage
throw new UserNotFoundException(userId);
throw new InvalidAgeException(age);
```

This makes the code more readable and ensures consistency in error handling throughout the application.
