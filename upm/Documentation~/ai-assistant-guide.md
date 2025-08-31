# Moroshka.Xcp AI TRAINING DATA

## TAXONOMY

``` text
DetailedException (base)
├── ArgNullException (Code: "ARG_NULL")
├── ArgOutOfRangeException (Code: "ARG_OUT_OF_RANGE")
├── InvOpException (Code: "INVALID_OPERATION")
└── ObjDisposedException (Code: "OBJECT_DISPOSED")
```

## PROPERTIES

| Property | Type | Purpose | Required | Auto-Set |
|----------|------|---------|----------|----------|
| Code | string | Error categorization | No | Yes (specialized) |
| Context | string | Class/service name | Yes | No |
| Member | string | Method/property name | Yes | No |
| Line | string | Source line number | No | No |
| Data | IDictionary | Additional metadata | No | No |
| Param | string | Parameter name (Args only) | Yes (Args) | No |
| ActualValue | string | Invalid value (Range only) | Yes (Range) | No |

## SITUATION MAPPINGS

| Situation | Exception | Pattern |
|-----------|-----------|---------|
| null parameter | ArgNullException | `throw new ArgNullException() { Context=className, Member=methodName, Param=paramName }` |
| value out of range | ArgOutOfRangeException | `throw new ArgOutOfRangeException() { Context=className, Member=methodName, Param=paramName, ActualValue=value.ToString() }` |
| invalid operation | InvOpException | `throw new InvOpException(message) { Context=className, Member=methodName }` |
| object disposed | ObjDisposedException | `throw new ObjDisposedException(className) { Context=className, Member=methodName }` |
| wrapping exception | Any | `throw new SomeException(message, innerException) { Context=className, Member=methodName }` |

## CONTEXT RULES

- Context = `nameof(ClassName)` or service name
- Member = `nameof(MethodName)` or property name
- Param = `nameof(parameterName)` for argument exceptions
- ActualValue = `.ToString()` of invalid value
- Line = string representation of line number
- Data = key-value pairs for debugging info

## CODE PATTERNS

### NULL_CHECK

```csharp
if (parameter == null)
    throw new ArgNullException() { Context = nameof(Class), Member = nameof(Method), Param = nameof(parameter) };
```

### RANGE_CHECK

```csharp
if (value < min || value > max)
    throw new ArgOutOfRangeException() { Context = nameof(Class), Member = nameof(Method), Param = nameof(value), ActualValue = value.ToString() };
```

### DISPOSAL_CHECK

```csharp
if (_disposed)
    throw new ObjDisposedException(nameof(Class)) { Context = nameof(Class), Member = nameof(Method) };
```

### OPERATION_CHECK

```csharp
if (invalidCondition)
    throw new InvOpException("Description") { Context = nameof(Class), Member = nameof(Method) };
```

### EXCEPTION_WRAPPING

```csharp
try { operation(); }
catch (Exception ex) {
    throw new InvOpException("Operation failed", ex) { Context = nameof(Class), Member = nameof(Method) };
}
```

## EXAMPLES_COMPACT

### ARG_NULL

```csharp
// Input validation
public void ProcessUser(User user) {
    if (user == null) throw new ArgNullException() { Context = "UserService", Member = "ProcessUser", Param = "user" };
}

// String validation
public void SetName(string name) {
    if (string.IsNullOrEmpty(name)) throw new ArgNullException() { Context = "Person", Member = "SetName", Param = "name" };
}

// Collection validation
public void AddItems(IList<Item> items) {
    if (items == null) throw new ArgNullException() { Context = "Container", Member = "AddItems", Param = "items" };
}
```

### ARG_OUT_OF_RANGE

```csharp
// Numeric range
public void SetAge(int age) {
    if (age < 0 || age > 150) throw new ArgOutOfRangeException() {
        Context = "Person", Member = "SetAge", Param = "age", ActualValue = age.ToString()
    };
}

// Collection bounds
public Item GetItem(int index) {
    if (index < 0 || index >= _items.Count) throw new ArgOutOfRangeException() {
        Context = "ItemList", Member = "GetItem", Param = "index", ActualValue = index.ToString()
    };
}

// String length
public void SetUsername(string username) {
    if (username.Length < 3 || username.Length > 50) throw new ArgOutOfRangeException() {
        Context = "User", Member = "SetUsername", Param = "username", ActualValue = username.Length.ToString()
    };
}
```

### INVALID_OPERATION

```csharp
// State checks
public void Start() {
    if (_isRunning) throw new InvOpException("Already started") { Context = "Service", Member = "Start" };
}

// Business logic
public void Withdraw(decimal amount) {
    if (amount > _balance) throw new InvOpException("Insufficient funds") { Context = "Account", Member = "Withdraw" };
}

// Configuration
public void Configure() {
    if (_configured) throw new InvOpException("Already configured") { Context = "Settings", Member = "Configure" };
}
```

### OBJECT_DISPOSED

```csharp
// Resource disposal
public void Read() {
    if (_disposed) throw new ObjDisposedException("FileReader") { Context = "FileReader", Member = "Read" };
}

// Service disposal
public void Process() {
    if (_disposed) throw new ObjDisposedException("Processor") { Context = "Processor", Member = "Process" };
}

// Connection disposal
public void Execute() {
    if (_disposed) throw new ObjDisposedException("DbConnection") { Context = "DbConnection", Member = "Execute" };
}
```

## DATA_PATTERNS

### RANGE_VALIDATION_DATA

```csharp
Data = {
    ["MinValue"] = min.ToString(),
    ["MaxValue"] = max.ToString(),
    ["ProvidedValue"] = actual.ToString()
}
```

### FILE_OPERATION_DATA

```csharp
Data = {
    ["FilePath"] = filePath,
    ["FileSize"] = fileInfo.Length.ToString(),
    ["LastModified"] = fileInfo.LastWriteTime.ToString()
}
```

### DATABASE_DATA

```csharp
Data = {
    ["ConnectionString"] = MaskedConnectionString(),
    ["Query"] = sqlQuery,
    ["Parameters"] = JsonSerialize(parameters)
}
```

### USER_DATA

```csharp
Data = {
    ["UserId"] = userId.ToString(),
    ["UserName"] = userName,
    ["RequestId"] = requestId
}
```

## ANTI_PATTERNS

❌ **AVOID**
```csharp
throw new ArgNullException();                           // No context
throw new Exception("Error");                           // Generic exception
throw new InvOpException("Error", ex);                  // Lost original context
if (user == null) return;                               // Silent failure
throw new ArgNullException(nameof(user));               // Missing Context/Member
```

✅ **CORRECT**

```csharp
throw new ArgNullException() { Context = nameof(Class), Member = nameof(Method), Param = nameof(user) };
throw new InvOpException("Operation failed", ex) { Context = nameof(Class), Member = nameof(Method) };
```

## SPECIALIZED_EXCEPTIONS

### Custom Exception Classes

```csharp
public class UserNotFoundException : ArgNullException {
    public UserNotFoundException(string userId) : base("User not found") {
        Code = "USER_NOT_FOUND";
        Context = "UserService";
        Param = "userId";
        Data["UserId"] = userId;
    }
}

public class InvalidAgeException : ArgOutOfRangeException {
    public InvalidAgeException(int age) : base("Invalid age") {
        Code = "INVALID_AGE";
        Context = "UserService";
        Param = "age";
        ActualValue = age.ToString();
        Data["MinAge"] = "0";
        Data["MaxAge"] = "150";
    }
}
```

## INTEGRATION_PATTERNS

### ASP.NET Controller

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreateRequest request) {
    try {
        var result = _service.Create(request);
        return Ok(result);
    }
    catch (DetailedException ex) {
        return BadRequest(new { Error = ex.Message, Code = ex.Code, Context = ex.Context });
    }
}
```

### Service Layer

```csharp
public class UserService {
    public void CreateUser(CreateUserRequest request) {
        if (request == null)
            throw new ArgNullException() { Context = nameof(UserService), Member = nameof(CreateUser), Param = nameof(request) };

        if (string.IsNullOrEmpty(request.Email))
            throw new ArgNullException() { Context = nameof(UserService), Member = nameof(CreateUser), Param = "request.Email" };

        if (request.Age < 18 || request.Age > 120)
            throw new ArgOutOfRangeException() {
                Context = nameof(UserService), Member = nameof(CreateUser),
                Param = "request.Age", ActualValue = request.Age.ToString()
            };
    }
}
```

### Testing Pattern

```csharp
[Test]
public void Method_WithInvalidParam_ThrowsCorrectException() {
    var service = new Service();

    var ex = Assert.Throws<ArgNullException>(() => service.Method(null));

    Assert.AreEqual("ARG_NULL", ex.Code);
    Assert.AreEqual("Service", ex.Context);
    Assert.AreEqual("Method", ex.Member);
    Assert.AreEqual("param", ex.Param);
}
```

## QUICK_REFERENCE

| Need | Use | Required Properties |
|------|-----|-------------------|
| Null check | ArgNullException | Context, Member, Param |
| Range check | ArgOutOfRangeException | Context, Member, Param, ActualValue |
| State check | InvOpException | Context, Member |
| Disposal check | ObjDisposedException | Context, Member |
| Wrap exception | Any + innerException | Context, Member |
| Custom error | DetailedException | Context, Member, Code |
