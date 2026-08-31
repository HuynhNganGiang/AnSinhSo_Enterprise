# Folder Structure

The `AnSinhSo.Tools` project is structured around self-contained vertical slices (Modules).

```text
src/AnSinhSo.Tools/
│
├── Core/
│   ├── IToolCommand.cs         # Interface for all CLI commands
│   ├── ToolRegistry.cs         # Explicit registry for commands
│   └── CommandDispatcher.cs    # Routes the string args to the resolved Command
│
├── Modules/
│   ├── Benchmark/              # Performance tests
│   ├── DemoSeed/               # Enterprise Demo Data Framework execution
│   ├── Diagnose/               # [Reserved] System diagnostics
│   ├── Export/                 # Data export operations
│   ├── Health/                 # [Reserved] Health checks
│   ├── Repair/                 # Data correction and recovery
│   ├── Seed/                   # Base data seeding
│   ├── Statistics/             # Aggregation and reporting
│   ├── Validation/             # Data Integrity Framework execution
│   └── Version/                # [Reserved] System versioning
│
│       └── [Module Name]/
│           ├── Commands/       # CLI command implementations (e.g., ValidateCommand.cs)
│           ├── Services/       # Execution logic and orchestration (e.g., ValidationService.cs)
│           ├── Models/         # Specific DTOs for the tool
│           └── Reports/        # Logic to format/output markdown/JSON reports
│
├── Docs/
│   ├── EnterpriseToolArchitecture.md
│   ├── FolderStructure.md
│   ├── CommandFlow.md
│   └── ToolDeveloperGuide.md
│
└── Program.cs                  # Entry point and DI setup
```
