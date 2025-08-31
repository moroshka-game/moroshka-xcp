using System;
using UnityEngine;

namespace Moroshka.Xcp.Samples
{

public sealed class ChainExceptions : MonoBehaviour
{
	private void Start()
	{
		try
		{
			Run();
		}
		catch (Exception)
		{
			try
			{
				throw;
			}
			catch (Exception e)
			{
				/*
				Moroshka.Xcp.ArgOutOfRangeException: Specified argument was out of the range of valid values
				[Code: "ARG_OUT_OF_RANGE", Context: "ChainExceptions", Member: "Run", Line: "53"]
				---> Moroshka.Xcp.ArgNullException: Value cannot be null
				[Code: "ARG_NULL", Context: "ChainExceptions", Member: "RunLayer1", Line: "70", Value: "value"]
				---> Moroshka.Xcp.InvOpException: Operation is not valid due to the current state of the object.
				[Code: "INVALID_OPERATION", Context: "ChainExceptions", Member: "RunLayer2", Line: "84"]
				--ArgOutOfRangeException
				  at Moroshka.Xcp.Samples.ChainExceptions.Run () [0x0000d] in ...\ChainExceptions\ChainExceptions.cs:53
				  at Moroshka.Xcp.Samples.ChainExceptions.Start () [0x0000f] in ...\ChainExceptions\ChainExceptions.cs:19
				--ArgNullException
				  at Moroshka.Xcp.Samples.ChainExceptions.RunLayer1 () [0x0000d] in ...\ChainExceptions\ChainExceptions.cs:70
				  at Moroshka.Xcp.Samples.ChainExceptions.Run () [0x00002] in ...\ChainExceptions\ChainExceptions.cs:49
				--InvOpException
				  at Moroshka.Xcp.Samples.ChainExceptions.RunLayer2 () [0x00001] in ...\ChainExceptions\ChainExceptions.cs:85
				  at Moroshka.Xcp.Samples.ChainExceptions.RunLayer1 () [0x00002] in ...\ChainExceptions\ChainExceptions.cs:66
				*/
				Debug.Log(e.ToString());
			}
		}
	}

	private void Run()
	{
		try
		{
			RunLayer1();
		}
		catch (Exception e)
		{
			throw new ArgOutOfRangeException(e)
			{
				Context = nameof(ChainExceptions),
				Member = nameof(Run),
				Line = "53"
			};
		}
	}

	private static void RunLayer1()
	{
		try
		{
			RunLayer2();
		}
		catch (Exception e)
		{
			throw new ArgNullException(e)
			{
				Context = nameof(ChainExceptions),
				Member = nameof(RunLayer1),
				Line = "70",
				Data =
				{
					["Value"] = "value"
				}
			};
		}
	}

	private static void RunLayer2()
	{
		throw new InvOpException
		{
			Context = nameof(ChainExceptions),
			Member = nameof(RunLayer2),
			Line = "84"
		};
	}
}

}
