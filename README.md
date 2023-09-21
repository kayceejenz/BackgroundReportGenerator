# `Background Report Generator Library`

The Background Report Generator Library is a .NET library that allows you to generate CSV reports from a database in the background. It provides a flexible way to fetch data from a database, generate CSV files, and execute custom actions upon completion.

## `Table of Contents`

- [`Getting Started`](#getting-started)
     - [`Prerequisites`](#prerequisites)
- [`Usage`](#usage)
     - [`Database Context`](#database-context)
     - [`Initialization`](#initialization)
     - [`Callback Actions`](#callback-actions)
     - [`Fetching Data`](#fetching-data)
     - [`Running the Background Task`](#running-the-background-task)

## `Getting Started`

### Prerequisites

To use this library, you'll need the following:

- .NET Core or .NET Framework project.
- Microsoft Entity Framework Core.
- CsvHelper library.

## `Usage`

### `Database Context`

Create a database context that inherits from DbContext and configure it for your database connection.

```csharp
var options = new DbContextOptionsBuilder<YourDbContext>()
    .UseSqlServer("YourConnectionString")
    .Options;

var dbContext = new YourDbContext(options);
```

### `Initialisation`

Initialize the TaskManager using your database context.

```csharp
var process = new TaskProcess(dbContext);
```

### `Callback Actions`

Set an action to execute when the background task is complete.

```csharp
var data = new User()
{
    Username = "Kaycee",
};

process.SetOnCompleteCallback(() =>
{
    process.UpdateTracker((User u) => u.Id == 1, data);
});
```

This action will be executed when the background task, such as generating a CSV report, is completed.

### `Running the Background Task`

Define a predicate for fetching data from the database and specify a limit for records.

Customize the predicate to filter the data you want to fetch and set the limit accordingly.

Start the background task to generate CSV reports.

```csharp
static bool Predicate(User u) => u.Id != 0;
uint limit = 10;

var path = "C://LibraryUserReport/file.csv";
process.PerformDataReadToCSV(Predicate, limit, path);
```

Specify the path where the generated CSV report will be saved and provide the predicate and limit for data retrieval.

```csharp
var backgroundTask = new TaskManager();
backgroundTask.StartAsync(async (cancellationToken) =>
{
    await PreformCSVAsync();
});
```
