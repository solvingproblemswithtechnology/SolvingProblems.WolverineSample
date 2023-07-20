# TODO Use --no-build to improve times.
$DEFAULT_PROJECT = "SolvingProblems.WolverineSample";

enum Contexts {
    Backend
}

enum Environments {
    Development
    Beta
}

$CONTEXTS = @{
    [Contexts]::Backend = @{
        Name             = "BackendDbContext"
        MigrationsFolder = "Infrastructure/Data/Migrations"
    }
}

function EfAddMigration(
    [Parameter(Position = 0, Mandatory = $true)]
    [string] $Name,
    [Alias("c")]
    [Parameter(Mandatory = $true)]
    [Contexts] $Context) {

    $selectedContext = $CONTEXTS[$Context];

    dotnet ef migrations add $Name -c $selectedContext.Name -p $DEFAULT_PROJECT -o $selectedContext.MigrationsFolder;
}

function EfRemoveMigration (
    [Alias("c")]
    [Parameter(Mandatory = $true)]
    [Contexts] $Context) {
		
    $selectedContext = $CONTEXTS[$Context];
	
    dotnet ef migrations remove -c $selectedContext.Name -p $DEFAULT_PROJECT
}

function EfUpdate(
    [Parameter(Position = 0, Mandatory = $false)]
    [string] $Name = $null,		
    [Alias("c")]
    [Parameter(Mandatory = $true)]
    [Contexts] $Context,
    [Alias("e")]
    [Parameter(Mandatory = $false)]
    [string] $Environment = "Development") {

    $oldenv = $env:ASPNETCORE_ENVIRONMENT
    $env:ASPNETCORE_ENVIRONMENT = $Environment
	
    $selectedContext = $CONTEXTS[$Context];
	
    if (![string]::IsNullOrWhiteSpace($Name)) {
        dotnet ef database update $Name -c $selectedContext.Name -p $DEFAULT_PROJECT
    }
    else {
        dotnet ef database update -c $selectedContext.Name -p $DEFAULT_PROJECT
    }

    $env:ASPNETCORE_ENVIRONMENT = $oldenv
}

function EfUpdateToPrevious(	
    [Alias("c")]
    [Parameter(Mandatory = $true)]
    [Contexts] $Context,
    [Alias("e")]
    [Parameter(Mandatory = $false)]
    [string] $Environment = "Development") {

    $oldenv = $env:ASPNETCORE_ENVIRONMENT
    $env:ASPNETCORE_ENVIRONMENT = $Environment
	
    $selectedContext = $CONTEXTS[$Context];

    # Take the previous migration
    $LastMigration = (dotnet ef migrations list -c $selectedContext.Name -p $DEFAULT_PROJECT --no-connect --json | Select-Object -Skip 2 | ConvertFrom-Json) | Select-Object -Last 2 | Select-Object -First 1;
	
    dotnet ef database update $LastMigration.Name -c $selectedContext.Name -p $DEFAULT_PROJECT

    $env:ASPNETCORE_ENVIRONMENT = $oldenv
}

function EfMigrate(
    [Parameter(Position = 0, Mandatory = $true)]
    [string] $Name,
    [Alias("c")]
    [Parameter(Mandatory = $true)]
    [Contexts] $Context,
    [Alias("e")]
    [Parameter(Mandatory = $false)]
    [string] $Environment = "Development") {

    EfAddMigration -Name $Name -Context $Context;
    EfUpdate -Name $Name -Context $Context -Environment $Environment;
}

function EfUnmigrate (
    [Alias("c")]
    [Parameter(Mandatory = $true)]
    [Contexts] $Context,
    [Alias("e")]
    [Parameter(Mandatory = $false)]
    [string] $Environment = "Development") {
		
    $selectedContext = $CONTEXTS[$Context];

    # Take the previous migration
    $LastMigration = (dotnet ef migrations list -c $selectedContext.Name -p $DEFAULT_PROJECT --no-connect --json | Select-Object -Skip 2 | ConvertFrom-Json) | Select-Object -Last 2 | Select-Object -First 1;
	
    EfUpdateToPrevious -Context $Context -Environment $Environment;
    EfRemoveMigration -Context $Context;
}