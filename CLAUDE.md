# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**Must read reference: `D:\Dev\Claude\CLAUDE_BEST_PRACTICES.md`**

## Project Overview

Craiel Godot Essentials is a comprehensive collection of utilities, tools, and systems for Godot 4+ game development. The suite contains both standalone utilities and interconnected systems, providing essential functionality for game creation including AI behavior trees, audio management, data structures, engine core functionality, and build tools.

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

## Available Tools and Utilities

### Core Engine Systems
- **EngineCore**: Modular game architecture with `IGameModule` interface for organizing game functionality into manageable modules
- **Events**: Centralized event aggregation system with automatic subscription cleanup and UI event handling
- **Threading**: Synchronization utilities, engine time management, and thread-safe data handling
- **Loading**: Component-based loading system with delayed object initialization and interval triggers
- **Settings**: Comprehensive game settings management with audio settings, general settings, and configuration persistence
- **Commands**: Command pattern implementation with queuing, immediate execution, status tracking, and payload support
- **Contracts**: Extensive interface definitions for all major systems ensuring consistent API contracts across the framework
- **Database**: Game data management system with attribute-based definitions, cross-database lookups, ID management, and state tracking

### AI and Behavior
- **AI/BTree**: Complete behavior tree implementation with serialization, decorators, task management, and various node types (branches, leafs, conditions)
- **FSM**: Stack-based state machine for complex state transitions and management

### Data Structures and Collections
- **Collections**: Memory-efficient temporary data structures (`TempList`, `TempHashSet`, `TempDictionary`) for performance-critical code
- **Data/SBT**: Serialized binary tree data format with type-specific node implementations and synchronization support
- **Pool**: Object pooling system with `IPoolable` interface for memory management and node pooling
- **CSV**: Comprehensive CSV parsing and manipulation with support for multiple delimiters and column definitions

### Spatial and Mathematics
- **Spatial**: Octree and KD-tree implementations for spatial partitioning and navigation with priority-based search
- **Mathematics**: Extended math utilities, random number generation, and mathematical constants
- **Geometry**: Mesh utilities, OBJ import/export, triangle indexing, and dynamic mesh generation
- **Noise**: Multi-type noise generation (Perlin, Simplex, Cellular, Fractal, Cubic) with configurable parameters

### Graphics and Animation
- **TweenLite**: Lightweight tweening system with easing functions for Vector2, Vector3, Float, and Color animations
- **Nodes**: UI components, 2D nodes, draggable sprites with zoom support, and tool nodes for enhanced editor functionality
- **Audio**: Complete audio management system with audio buses, controllers, and player nodes for sound and music management

### Utilities and Extensions
- **Utils**: Hash utilities, math extensions, UTF8 string handling, player preferences, display utilities, and type definitions
- **Extensions**: Vector, string, rectangle, quaternion compression, color utilities, collection extensions, and binary I/O helpers
- **Formatting**: Format handlers for consistent data presentation
- **Grammar**: Text parsing system with tokenization, grammar definitions, and term processing

### I/O and Resource Management
- **Resource**: Resource loading system with reference counting, pooling, and asynchronous loading capabilities
- **IO**: Managed file and directory operations with filtering and result handling
- **Json**: JSON configuration management with serialization support

### Communication and Messaging
- **Msg**: Message management and dispatch system for decoupled component communication
- **I18N**: Internationalization system with localization providers, token management, and file-based translations

### Development and Build Tools (Node.js)
- **GenerateResourceKeys.js**: Automated resource key generation for art and audio assets
- **UnitySpriteToGodotAtlasTextures.js**: Unity to Godot sprite atlas conversion utility
- **GenerateDataResources.js**: Data resource file generation and indexing
- **FixGodotReferences.js**: Godot reference fixing and cleanup utility
- **CleanupProject.js**: Project cleanup and maintenance tool
- **TilesetTools.js**: Tileset processing and optimization utilities

### Debugging and Diagnostics
- **DebugTools**: Development and debugging utilities with event tracking for enhanced development workflow
- **Logging**: Godot-integrated logging system with log relay functionality for debugging and error tracking
- **Exceptions**: Custom exception types including IllegalStateException for better error handling

### Input and Scene Management
- **Input**: Input handling utilities and abstractions
- **Scene**: Base scene classes and scene transition management