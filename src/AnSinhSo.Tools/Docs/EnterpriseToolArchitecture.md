# Enterprise Tool Architecture

## Overview
The `AnSinhSo.Tools` project is the dedicated Enterprise Tool Center for the AnSinhSo system. It acts as an execution platform for all out-of-band infrastructure tools without polluting the core API or Domain.

## Core Architectural Principles

1. **Custom Command Dispatcher**
   - The framework avoids reliance on reflection-based scanning or third-party CLI libraries (e.g., `System.CommandLine`, `Spectre.Console`).
   - Instead, it uses a highly deterministic, custom `ToolRegistry` and `CommandDispatcher`.
   - Every command is explicitly registered in the registry.

2. **Modular Organization**
   - The tool center is partitioned vertically by **Module**, not horizontally by layer.
   - Each module completely encapsulates its `Commands`, `Services`, `Models`, and `Reports`.
   - The reserved modules include `Diagnose`, `Health`, and `Version` for system diagnostics, alongside operational modules like `Seed`, `Validation`, and `Export`.

3. **Service Layer Isolation**
   - Commands (classes implementing `IToolCommand`) are lightweight and strictly handle argument passing and CLI feedback.
   - **No logic** is placed in the Command. Commands must delegate to a dedicated `Service` (e.g., `ValidateCommand` calls `ValidationService`).
   - The Service layer then interacts with internal systems or `AnSinhSo.Infrastructure` engines. Commands NEVER invoke internal infrastructure directly.

## Lifecycle
1. Application boots in `Program.cs`.
2. Services and Commands are registered into the `IServiceCollection` and `ToolRegistry`.
3. `CommandDispatcher` parses the CLI arguments to identify the target command name.
4. The requested `IToolCommand` is resolved from Dependency Injection and executed.
