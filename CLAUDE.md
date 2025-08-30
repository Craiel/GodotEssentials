# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**Must read reference: `D:\Dev\Claude\CLAUDE_BEST_PRACTICES.md`**

## Project Overview

Craiel Godot Essentials is a C# library for Godot 4.2.2 game development, providing essential systems and utilities for game creation including AI behavior trees, audio management, data structures, and engine core functionality.

## Build Commands

```bash
# Build the project
dotnet build

# Clean build artifacts
dotnet clean

# Restore dependencies
dotnet restore
```

## Code Architecture

### Core Structure
- **EngineCore**: Central engine management with modular game architecture via `IGameModule` interface
- **AI/BTree**: Complete behavior tree implementation with serialization, decorators, and task management
- **Collections**: Extended data structures including temp containers, priority queues, and circular buffers
- **Data/SBT**: Serialized binary tree data format with type-specific node implementations
- **Resource**: Resource management and loading system
- **Event**: Event aggregation system with managed subscriptions

### Key Patterns
- **Module System**: Game functionality organized into modules inheriting from `GameModuleBase<T>`
- **Event Management**: Centralized `GameEvents` with automatic subscription cleanup in modules
- **Resource Pools**: `IPoolable` interface for object reuse patterns
- **Temp Collections**: Memory-efficient temporary data structures (TempList, TempHashSet, TempDictionary)

### Namespace Structure
All code uses `Craiel.Essentials.*` namespace hierarchy matching directory structure.

## Development Standards

### Language Settings
- Target Framework: .NET 6.0
- C# Language Version: 10
- Nullable reference types enabled
- Implicit usings enabled

### Code Conventions
- Follow existing patterns in neighboring files
- Use `IGameModule` interface for modular functionality
- Implement proper disposal in `Destroy()` methods
- Leverage temp collections for performance-critical code
- Use event aggregation pattern for decoupled communication

## Key Directories

- `scripts/`: All C# source code organized by feature area
- `data/`: Game data and assets
- `tools/`: Build tools and utilities (Node.js with Sharp dependency)