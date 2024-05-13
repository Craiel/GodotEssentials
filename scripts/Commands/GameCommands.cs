namespace Craiel.Essentials.Commands;

using System;
using System.Collections.Generic;
using Contracts;
using Utils;

public class GameCommands : IGameModule
{
	private readonly Queue<QueuedCommand> queuedCommands = new();
	private readonly IDictionary<int, CommandInfo> commandInfos = new System.Collections.Generic.Dictionary<int, CommandInfo>();
	private readonly IDictionary<int, CommandHandlerDelegate> handlers = new Dictionary<int, CommandHandlerDelegate>();

	class CommandInfo
	{
		public CommandInfo(string id, int typeHash, Type commandType)
		{
			this.Id = id;
			this.TypeHash = typeHash;
			this.Type = commandType;
		}

		public readonly string Id;
		public readonly int TypeHash;
		public readonly Type Type;
	}
	
	class QueuedCommand
	{
		public QueuedCommand(int typeHash, IGameCommand command, IGameCommandPayload? payload)
		{
			this.TypeHash = typeHash;
			this.Command = command;
			this.Payload = payload;
		}

		public readonly int TypeHash;
		public readonly IGameCommand Command;
		public readonly IGameCommandPayload? Payload;
	}
	
	// -------------------------------------------------------------------
	// Public
	// -------------------------------------------------------------------
	public delegate GameCommandStatus CommandHandlerDelegate(IGameCommandPayload? payload);
	
	public void Initialize()
	{
	}

	public void Update(double delta)
	{
		while (this.queuedCommands.Count > 0)
		{
			var command = this.queuedCommands.Dequeue();
			if (this.handlers.TryGetValue(command.TypeHash, out var handler))
			{
				handler.Invoke(command.Payload);
				continue;
			}
			
			EssentialCore.Logger.Warn("No handler for Command: " + command.Command.GetType().FullName);
		}
	}

	public void Destroy()
	{
	}
	
	public void Queue<T>(T command, IGameCommandPayload? payload = null)
		where T : IGameCommand
	{
		RegisterCommand<T>();
		
		var entry = new QueuedCommand(TypeDef<T>.Hash, command, payload);
		this.queuedCommands.Enqueue(entry);
	}
	
	public void Queue<T>(IGameCommandPayload? payload = null)
		where T: IGameCommand
	{
		Queue(Activator.CreateInstance<T>(), payload);
	}
	
	public void Register<T>(CommandHandlerDelegate handler)
		where T: IGameCommand
	{
		CommandInfo info = RegisterCommand<T>();
		if (!handlers.TryAdd(info.TypeHash, handler))
		{
			throw new InvalidOperationException("Handler already defined for : " + info.Type.FullName);
		}
	}

	public void ExecuteImmediate<T>(IGameCommandPayload? payload = null)
		where T : IGameCommand
	{
		ExecuteImmediate(Activator.CreateInstance<T>(), payload);
	}

	public void ExecuteImmediate<T>(T command, IGameCommandPayload? payload = null)
		where T : IGameCommand
	{
		if (!this.handlers.TryGetValue(TypeDef<T>.Hash, out var handler))
		{
			EssentialCore.Logger.Warn("Command Execution failed, no handler: " + TypeDef<T>.Value.FullName);
			command.Status = GameCommandStatus.Failure;
			return;
		}

		command.Status = handler(payload);
	}

	// -------------------------------------------------------------------
	// Private
	// -------------------------------------------------------------------
	private CommandInfo RegisterCommand<T>()
		where T : IGameCommand
	{
		int typeHash = TypeDef<T>.Hash;
		if (this.commandInfos.TryGetValue(typeHash, out CommandInfo? info))
		{
			return info;
		}

		var instance = Activator.CreateInstance<T>();
		info = new CommandInfo(instance.Id, typeHash, TypeDef<T>.Value);
		this.commandInfos.Add(typeHash, info);
		return info;
	}
}
