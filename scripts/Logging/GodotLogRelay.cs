using System.Threading;

namespace Craiel.Essentials.Logging;

using System;
using Godot;

public class GodotLogRelay
{
	public void Info(string message)
	{
		GD.Print(FormatMessage(message));
	}
	
	public void Warn(string message)
	{
		GD.PushWarning(FormatMessage(message));
	}

	public void Error(string message)
	{
		GD.PushError(FormatMessage(message));
	}
	
	public void Error<T>(string message, T exception = null)
		where T: Exception
	{
		GD.PushError(FormatMessage(message));
	}

	static string FormatMessage(string message)
	{
		TimeSpan time = TimeSpan.FromMilliseconds(Time.GetTicksMsec());
		return $"[{Thread.CurrentThread.ManagedThreadId}] {time:g}: {message}";
	}
}
