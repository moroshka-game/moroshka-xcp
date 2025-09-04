using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Moroshka.Xcp.Benchmark;

[MemoryDiagnoser]
[RankColumn]
[SuppressMessage("Performance", "CA1822")]
public sealed class Benchmark
{
	private const string TestMessage = "Test exception message";

	[Benchmark]
	public void CreateException_System()
	{
		_ = new Exception(TestMessage)
		{
			Data =
			{
				["Code"] = "ERROR",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Value"] = "testValue"
			}
		};
		_ = new ArgumentException(TestMessage)
		{
			Data =
			{
				["Code"] = "ARG_ERROR",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["Value"] = "testValue"
			}
		};
		_ = new ArgumentNullException(TestMessage)
		{
			Data =
			{
				["Code"] = "ARG_NULL",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["Value"] = "testValue"
			}
		};
		_ = new ArgumentOutOfRangeException(TestMessage)
		{
			Data =
			{
				["Code"] = "ARG_OUT_OF_RANGE",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["ActualValue"] = "TestValue",
				["Value"] = "testValue"
			}
		};
		_ = new InvalidOperationException(TestMessage)
		{
			Data =
			{
				["Code"] = "INVALID_OPERATION",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Value"] = "testValue"
			}
		};
		_ = new ObjectDisposedException(TestMessage)
		{
			Data =
			{
				["Code"] = "OBJ_DISPOSED",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Object"] = "TestObject",
				["Value"] = "testValue"
			}
		};
	}

	[Benchmark]
	public void CreateException_Moroshka()
	{
		_ = new DetailedException(TestMessage)
		{
			Code = "ERROR",
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		_ = new ArgException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		_ = new ArgNullException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		_ = new ArgOutOfRangeException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestParam",
			ActualValue = "TestValue",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		_ = new InvOpException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		_ = new ObjDisposedException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Object = "TestObject",
			Data =
			{
				["Value"] = "testValue"
			}
		};
	}

	[Benchmark]
	public void ThrowNestedException_System()
	{
		try
		{
			var ex1 = new Exception(TestMessage)
			{
				Data =
				{
					["Code"] = "ERROR",
					["Context"] = "TestContext",
					["Member"] = "TestMember",
					["Line"] = "123",
					["Value"] = "testValue"
				}
			};
			var ex2 = new ArgumentException(TestMessage, ex1)
			{
				Data =
				{
					["Code"] = "ARG_ERROR",
					["Context"] = "TestContext",
					["Member"] = "TestMember",
					["Line"] = "123",
					["Param"] = "TestArg",
					["Value"] = "testValue"
				}
			};
			var ex3 = new ArgumentNullException(TestMessage, ex2)
			{
				Data =
				{
					["Code"] = "ARG_NULL",
					["Context"] = "TestContext",
					["Member"] = "TestMember",
					["Line"] = "123",
					["Param"] = "TestArg",
					["Value"] = "testValue"
				}
			};
			var ex4 = new ArgumentOutOfRangeException(TestMessage, ex3)
			{
				Data =
				{
					["Code"] = "ARG_OUT_OF_RANGE",
					["Context"] = "TestContext",
					["Member"] = "TestMember",
					["Line"] = "123",
					["Param"] = "TestArg",
					["ActualValue"] = "TestValue",
					["Value"] = "testValue"
				}
			};
			var ex5 = new InvalidOperationException(TestMessage, ex4)
			{
				Data =
				{
					["Code"] = "INVALID_OPERATION",
					["Context"] = "TestContext",
					["Member"] = "TestMember",
					["Line"] = "123",
					["Value"] = "testValue"
				}
			};
			throw new ObjectDisposedException(TestMessage, ex5)
			{
				Data =
				{
					["Code"] = "OBJ_DISPOSED",
					["Context"] = "TestContext",
					["Member"] = "TestMember",
					["Line"] = "123",
					["Object"] = "TestObject",
					["Value"] = "testValue"
				}
			};
		}
		catch (Exception)
		{
			// ignore
		}

	}

	[Benchmark]
	public void ThrowNestedException_Moroshka()
	{
		try
		{
			var ex1 = new DetailedException(TestMessage)
			{
				Code = "ERROR",
				Context = "TestContext",
				Member = "TestMember",
				Line = "123",
				Data =
				{
					["Value"] = "testValue"
				}
			};
			var ex2 = new ArgException(TestMessage, ex1)
			{
				Context = "TestContext",
				Member = "TestMember",
				Line = "123",
				Param = "TestArg",
				Data =
				{
					["Value"] = "testValue"
				}
			};
			var ex3 = new ArgNullException(TestMessage, ex2)
			{
				Context = "TestContext",
				Member = "TestMember",
				Line = "123",
				Param = "TestArg",
				Data =
				{
					["Value"] = "testValue"
				}
			};
			var ex4 = new ArgOutOfRangeException(TestMessage, ex3)
			{
				Context = "TestContext",
				Member = "TestMember",
				Line = "123",
				Param = "TestParam",
				ActualValue = "TestValue",
				Data =
				{
					["Value"] = "testValue"
				}
			};
			var ex5 = new InvOpException(TestMessage, ex4)
			{
				Context = "TestContext",
				Member = "TestMember",
				Line = "123",
				Data =
				{
					["Value"] = "testValue"
				}
			};
			throw new ObjDisposedException(TestMessage, ex5)
			{
				Context = "TestContext",
				Member = "TestMember",
				Line = "123",
				Object = "TestObject",
				Data =
				{
					["Value"] = "testValue"
				}
			};
		}
		catch (Exception)
		{
			// ignore
		}
	}

	[Benchmark]
	public void ToStringException_System()
	{
		_ = new Exception(TestMessage)
		{
			Data =
			{
				["Code"] = "ERROR",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ArgumentException(TestMessage)
		{
			Data =
			{
				["Code"] = "ARG_ERROR",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ArgumentNullException(TestMessage)
		{
			Data =
			{
				["Code"] = "ARG_NULL",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ArgumentOutOfRangeException(TestMessage)
		{
			Data =
			{
				["Code"] = "ARG_OUT_OF_RANGE",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["ActualValue"] = "TestValue",
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new InvalidOperationException(TestMessage)
		{
			Data =
			{
				["Code"] = "INVALID_OPERATION",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ObjectDisposedException(TestMessage)
		{
			Data =
			{
				["Code"] = "OBJ_DISPOSED",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Object"] = "TestObject",
				["Value"] = "testValue"
			}
		}.ToString();
	}

	[Benchmark]
	public void ToStringException_Moroshka()
	{
		_ = new DetailedException(TestMessage)
		{
			Code = "ERROR",
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ArgException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ArgNullException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ArgOutOfRangeException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestParam",
			ActualValue = "TestValue",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new InvOpException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
		_ = new ObjDisposedException(TestMessage)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Object = "TestObject",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
	}

	[Benchmark]
	public void ToStringNestedException_System()
	{
		var ex1 = new Exception(TestMessage)
		{
			Data =
			{
				["Code"] = "ERROR",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Value"] = "testValue"
			}
		};
		var ex2 = new ArgumentException(TestMessage, ex1)
		{
			Data =
			{
				["Code"] = "ARG_ERROR",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["Value"] = "testValue"
			}
		};
		var ex3 = new ArgumentNullException(TestMessage, ex2)
		{
			Data =
			{
				["Code"] = "ARG_NULL",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["Value"] = "testValue"
			}
		};
		var ex4 = new ArgumentOutOfRangeException(TestMessage, ex3)
		{
			Data =
			{
				["Code"] = "ARG_OUT_OF_RANGE",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Param"] = "TestArg",
				["ActualValue"] = "TestValue",
				["Value"] = "testValue"
			}
		};
		var ex5 = new InvalidOperationException(TestMessage, ex4)
		{
			Data =
			{
				["Code"] = "INVALID_OPERATION",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Value"] = "testValue"
			}
		};
		_ = new ObjectDisposedException(TestMessage, ex5)
		{
			Data =
			{
				["Code"] = "OBJ_DISPOSED",
				["Context"] = "TestContext",
				["Member"] = "TestMember",
				["Line"] = "123",
				["Object"] = "TestObject",
				["Value"] = "testValue"
			}
		}.ToString();
	}

	[Benchmark]
	public void ToStringNestedException_Moroshka()
	{
		var ex1 = new DetailedException(TestMessage)
		{
			Code = "ERROR",
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		var ex2 = new ArgException(TestMessage, ex1)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		var ex3 = new ArgNullException(TestMessage, ex2)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		var ex4 = new ArgOutOfRangeException(TestMessage, ex3)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Param = "TestArg",
			ActualValue = "TestValue",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		var ex5 = new InvOpException(TestMessage, ex4)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Data =
			{
				["Value"] = "testValue"
			}
		};
		_ = new ObjDisposedException(TestMessage, ex5)
		{
			Context = "TestContext",
			Member = "TestMember",
			Line = "123",
			Object = "TestObject",
			Data =
			{
				["Value"] = "testValue"
			}
		}.ToString();
	}
}
