namespace Craiel.Essentials.Commands;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Contracts;
using DebugTools;

public class GameCommands : IGameModule
{
	private readonly Queue<IGameCommand> queuedCommands = new();
	
	// -------------------------------------------------------------------
	// Public
	// -------------------------------------------------------------------
#if DEBUG
	public EventDebugTracker<IGameCommand> DebugTracker = new();
#endif
	
	public delegate GameCommandStatus CommandHandlerDelegate(IGameCommandPayload payload);
	
	public void Initialize()
	{
	}

	public void Update(double delta)
	{
		while (this.queuedCommands.Count > 0)
		{
			var command = this.queuedCommands.Dequeue();
			ExecuteImmediate(command);
		}
	}

	public void Destroy()
	{
	}
	
	public void Queue<T>()
		where T : IGameCommand
	{
		this.queuedCommands.Enqueue(Activator.CreateInstance<T>());
	}
	
	public void Queue<T>(T command)
		where T : IGameCommand
	{
		this.queuedCommands.Enqueue(command);
	}

	public void ExecuteImmediate<T>()
		where T : IGameCommand
	{
		var command = Activator.CreateInstance<T>();
		ExecuteImmediate(command);
	}

	public void ExecuteImmediate<TSpecific>(TSpecific command)
		where TSpecific : IGameCommand
	{
#if DEBUG
		var sw = new Stopwatch();
		sw.Start();
#endif
		
		this.DoExecute(command);
		
#if DEBUG
		sw.Stop();
		this.DebugTracker.Track(command.GetType(), 1, 1, sw.Elapsed.TotalSeconds);
#endif
	}

	private void DoExecute<T>(T command)
		where T: IGameCommand
	{
		switch (command.Status)
		{
			case GameCommandStatus.Success:
			case GameCommandStatus.Failure:
			{
				return;
			}

			case GameCommandStatus.Incomplete:
			case GameCommandStatus.NotRun:
			{
				command.Execute();
				switch (command.Status)
				{
					case GameCommandStatus.Incomplete:
					{
						this.Queue(command);
						break;
					}

					case GameCommandStatus.NotRun:
					{
						throw new InvalidOperationException("Command failed to set proper status during execution.");
					}
				}
					
				break;
			}
		}
	}
}
