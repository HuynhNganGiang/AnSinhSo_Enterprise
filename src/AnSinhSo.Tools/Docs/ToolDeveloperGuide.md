# Tool Developer Guide

Follow these guidelines when adding a new tool/command to `AnSinhSo.Tools`.

## 1. Creating a New Module
If the tool does not logically belong in an existing module, create a new directory inside `Modules/`:
```text
Modules/
  MyNewTool/
    Commands/
    Services/
    Models/
    Reports/
```

## 2. Implement the Service
Always start with the Service layer. The service holds the orchestration logic.
- Place it in `Modules/MyNewTool/Services/MyNewToolService.cs`
- It should expose an `ExecuteAsync` method (or similar).

## 3. Implement the Command
Implement `IToolCommand`.
- Place it in `Modules/MyNewTool/Commands/MyNewToolCommand.cs`
- Inject your `MyNewToolService` into the constructor.
- **Rule:** The `ExecuteAsync` method in the Command must ONLY parse arguments, invoke the Service, and handle console output. It must not touch the database, make API calls, or contain business logic.

## 4. Registration
Open `Program.cs`.
1. Register your Service: `services.AddTransient<MyNewToolService>();`
2. Register your Command explicitly: `.RegisterCommand<MyNewToolCommand>("mytool")`

## 5. Execution
Run your tool from the command line:
`dotnet run --project src/AnSinhSo.Tools/AnSinhSo.Tools.csproj -- mytool`
