namespace Craiel.Essentials.Commands;

using System;
using System.Collections.Generic;
using Contracts;
using Utils;

public class GameCommands : IGameModule
{
	private readonly Queue<IGameCommand> queuedCommands = new();
	
	// -------------------------------------------------------------------
	// Public
	// -------------------------------------------------------------------
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

	public void ExecuteImmediate<T>(T command)
		where T : IGameCommand
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
