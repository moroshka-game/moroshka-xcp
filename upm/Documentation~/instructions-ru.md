# Инструкции по использованию

- [Инструкции по использованию](#инструкции-по-использованию)
  - [Введение](#введение)
  - [Основные концепции](#основные-концепции)
    - [DetailedException](#detailedexception)
    - [Автоматические коды ошибок](#автоматические-коды-ошибок)
  - [Базовое использование](#базовое-использование)
    - [Простое исключение с контекстом](#простое-исключение-с-контекстом)
    - [Исключение с дополнительными данными](#исключение-с-дополнительными-данными)
  - [Дополнительные свойства](#дополнительные-свойства)
    - [Code (Код ошибки)](#code-код-ошибки)
    - [Context (Контекст)](#context-контекст)
    - [Member (Член класса)](#member-член-класса)
    - [Line (Строка)](#line-строка)
  - [Специализированные исключения](#специализированные-исключения)
    - [ArgNullException](#argnullexception)
    - [ArgOutOfRangeException](#argoutofrangeexception)
    - [InvOpException](#invopexception)
    - [ObjDisposedException](#objdisposedexception)
  - [Цепочки исключений](#цепочки-исключений)
    - [Создание цепочки исключений](#создание-цепочки-исключений)
    - [Результат ToString() для цепочки](#результат-tostring-для-цепочки)
  - [Лучшие практики](#лучшие-практики)
    - [1. Всегда указывайте контекст](#1-всегда-указывайте-контекст)
    - [2. Используйте значимые коды ошибок](#2-используйте-значимые-коды-ошибок)
    - [3. Сохраняйте цепочки исключений](#3-сохраняйте-цепочки-исключений)
    - [4. Используйте Data для дополнительной информации](#4-используйте-data-для-дополнительной-информации)
    - [5. Создавайте специализированные классы исключений](#5-создавайте-специализированные-классы-исключений)

## Введение

`Moroshka.Xcp` — это библиотека, которая предоставляет расширенные классы исключений для улучшения стандартных механизмов обработки ошибок в .NET. Эти исключения содержат дополнительную информацию, такую как:

- **Уникальные коды ошибок** (`Code`) для категоризации проблем
- **Контекстную информацию** (`Context`) о выполняемой операции
- **Имена методов** (`Member`) где произошла ошибка
- **Номера строк** (`Line`) для точного определения места ошибки
- **Дополнительные данные** через свойство `Data`

Это делает отладку и логирование более информативными и структурированными.

## Основные концепции

### DetailedException

Базовый класс `DetailedException` расширяет стандартный `Exception` и предоставляет дополнительные свойства для детальной информации об ошибке.

```csharp
using Moroshka.Xcp;

// Создание базового детализированного исключения
var exception = new DetailedException("Произошла ошибка")
{
    Code = "CUSTOM_ERROR",
    Context = "MyService",
    Member = "ProcessData",
    Line = "42"
};

throw exception;
```

### Автоматические коды ошибок

Специализированные исключения автоматически устанавливают соответствующие коды ошибок:

- `ArgNullException` → `"ARG_NULL"`
- `ArgOutOfRangeException` → `"ARG_OUT_OF_RANGE"`
- `InvOpException` → `"INVALID_OPERATION"`
- `ObjDisposedException` → `"OBJECT_DISPOSED"`

## Базовое использование

### Простое исключение с контекстом

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

### Исключение с дополнительными данными

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

## Дополнительные свойства

### Code (Код ошибки)

Уникальный идентификатор типа ошибки для категоризации и обработки:

```csharp
var exception = new DetailedException("Ошибка подключения к базе данных")
{
    Code = "DB_CONNECTION_FAILED"
};
```

### Context (Контекст)

Информация о контексте выполнения, обычно имя класса или сервиса:

```csharp
var exception = new DetailedException("Ошибка аутентификации")
{
    Context = "AuthenticationService"
};
```

### Member (Член класса)

Имя метода, свойства или другого члена класса, где произошла ошибка:

```csharp
var exception = new DetailedException("Ошибка валидации")
{
    Member = "ValidateUserInput"
};
```

### Line (Строка)

Номер строки кода, где произошла ошибка (обычно устанавливается вручную):

```csharp
var exception = new DetailedException("Неожиданная ошибка")
{
    Line = "127"
};
```

## Специализированные исключения

### ArgNullException

Для обработки null-аргументов:

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

Для значений вне допустимого диапазона:

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

Для недопустимых операций:

```csharp
public void StartService()
{
    if (_isRunning)
    {
        throw new InvOpException("Сервис уже запущен")
        {
            Context = nameof(BackgroundService),
            Member = nameof(StartService),
            Data = { ["ServiceState"] = "Running" }
        };
    }
}
```

### ObjDisposedException

Для использования освобожденных объектов:

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

## Цепочки исключений

### Создание цепочки исключений

```csharp
public void ProcessData()
{
    try
    {
        LowLevelOperation();
    }
    catch (Exception e)
    {
        // Оборачиваем низкоуровневое исключение в более контекстное
        throw new InvOpException("Ошибка обработки данных", e)
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

### Результат ToString() для цепочки

``` text
Moroshka.Xcp.InvOpException: Ошибка обработки данных
[Code: "INVALID_OPERATION", Context: "DataProcessor", Member: "ProcessData", Line: "45"]
---> Moroshka.Xcp.ArgNullException: Value cannot be null
[Code: "ARG_NULL", Context: "DataProcessor", Member: "LowLevelOperation", Param: "data"]
--InvOpException
  at DataProcessor.ProcessData() in Program.cs:line 45
--ArgNullException
  at DataProcessor.LowLevelOperation() in Program.cs:line 60
```

## Лучшие практики

### 1. Всегда указывайте контекст

```csharp
// ✅ Хорошо
throw new ArgNullException()
{
    Context = nameof(UserService),
    Member = nameof(ValidateUser),
    Param = nameof(username)
};

// ❌ Плохо
throw new ArgNullException();
```

### 2. Используйте значимые коды ошибок

```csharp
// ✅ Хорошо
var exception = new DetailedException("Пользователь не найден")
{
    Code = "USER_NOT_FOUND"
};

// ❌ Плохо
var exception = new DetailedException("Ошибка")
{
    Code = "ERROR"
};
```

### 3. Сохраняйте цепочки исключений

```csharp
// ✅ Хорошо
try
{
    SomeOperation();
}
catch (Exception e)
{
    throw new InvOpException("Операция не удалась", e)
    {
        Context = nameof(MyService)
    };
}

// ❌ Плохо
try
{
    SomeOperation();
}
catch (Exception e)
{
    throw new InvOpException("Операция не удалась")
    {
        Context = nameof(MyService)
    };
}
```

### 4. Используйте Data для дополнительной информации

```csharp
// ✅ Хорошо
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

### 5. Создавайте специализированные классы исключений

```csharp
/// <summary>
/// Исключение для случаев, когда пользователь не найден
/// </summary>
public class UserNotFoundException : ArgNullException
{
    public UserNotFoundException(string userId, Exception innerException = null)
        : base("Пользователь не найден", innerException)
    {
        Code = "USER_NOT_FOUND";
        Context = "UserService";
        Param = "userId";
        Data["UserId"] = userId;
    }
}

/// <summary>
/// Исключение для недопустимого возраста пользователя
/// </summary>
public class InvalidAgeException : ArgOutOfRangeException
{
    public InvalidAgeException(int age, Exception innerException = null)
        : base("Недопустимый возраст", innerException)
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

// Использование
throw new UserNotFoundException(userId);
throw new InvalidAgeException(age);
```

Это делает код более читаемым и обеспечивает консистентность в обработке ошибок по всему приложению.
