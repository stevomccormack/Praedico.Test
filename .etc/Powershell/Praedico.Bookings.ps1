

$Praedico = [pscustomobject]@{
    RootPath = "E:\Steve McCormack\Workspaces\GitHub\Praedico"
    SolutionPath = "$($Praedico.RootPath)\Praedico.Bookings.sln"
    ApiDirectoryPath = "$($Praedico.RootPath)\Praedico.Bookings.Api"
    ApiProjectPath = "$($Praedico.ApiDirectoryPath)\Praedico.Bookings.Api.csproj"
    ApiHostAddress = "http://localhost:5000"
}

$Git = [pscustomobject]@{
    OriginUrl = "git@github.com:stevomccormack/Praedico.Test.git"
    Origin = "origin"
    UserName = "Steve McCormack"
    UserEmail = "hello@iamstevo.co"
    BranchMain = "main"
}

$GitHub = [pscustomobject]@{    
    Repo = "Praedico.Test"
    Owner = "stevomccormack"
}


#-----------------------------------------------------------------------------------------------------------------------
# Displaying Variables and Paths
#-----------------------------------------------------------------------------------------------------------------------

Write-Host "-----------------------------------------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "Praedico Configuration:" -ForegroundColor Yellow
Write-Host "-----------------------------------------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "`$Praedico.RootPath: " -ForegroundColor Green -NoNewline; Write-Host "$($Praedico.RootPath)" -ForegroundColor White
Write-Host "`$Praedico.SolutionPath: " -ForegroundColor Green -NoNewline; Write-Host "$($Praedico.SolutionPath)" -ForegroundColor White
Write-Host "`$Praedico.ApiDirectoryPath: " -ForegroundColor Green -NoNewline; Write-Host "$($Praedico.ApiDirectoryPath)" -ForegroundColor White
Write-Host "`$Praedico.ApiProjectPath: " -ForegroundColor Green -NoNewline; Write-Host "$($Praedico.ApiProjectPath)" -ForegroundColor White
Write-Host "`$Praedico.ApiHostAddress: " -ForegroundColor Green -NoNewline; Write-Host "$($Praedico.ApiProjectPath)" -ForegroundColor White

Write-Host "-----------------------------------------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "Git Configuration:" -ForegroundColor Yellow
Write-Host "-----------------------------------------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "`$Git.OriginUrl: " -ForegroundColor Green -NoNewline; Write-Host "$($Git.OriginUrl)" -ForegroundColor White
Write-Host "`$Git.Origin: " -ForegroundColor Green -NoNewline; Write-Host "$($Git.Origin)" -ForegroundColor White
Write-Host "`$Git.UserName: " -ForegroundColor Green -NoNewline; Write-Host "$($Git.UserName)" -ForegroundColor White
Write-Host "`$Git.UserEmail: " -ForegroundColor Green -NoNewline; Write-Host "$($Git.UserEmail)" -ForegroundColor White
Write-Host "`$Git.BranchMain: " -ForegroundColor Green -NoNewline; Write-Host "$($Git.BranchMain)" -ForegroundColor White

Write-Host "-----------------------------------------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "GitHub Configuration:" -ForegroundColor Yellow
Write-Host "-----------------------------------------------------------------------------------------------" -ForegroundColor Yellow
Write-Host "`$GitHub.Repo: " -ForegroundColor Green -NoNewline; Write-Host "$($GitHub.Repo)" -ForegroundColor White
Write-Host "`$GitHub.Owner: " -ForegroundColor Green -NoNewline; Write-Host "$($GitHub.Owner)" -ForegroundColor Cyan




#-----------------------------------------------------------------------------------------------------------------------
# Check .NET versions
#-----------------------------------------------------------------------------------------------------------------------
dotnet --list-sdks
dotnet --list-runtimes

dotnet dev-certs https --trust

Push-Location $Praedico.ApiDirectoryPath

    dotnet add package Serilog.AspNetCore
    dotnet add package Serilog.Sinks.Console
    dotnet add package Serilog.Sinks.File
    dotnet add package Serilog.Settings.Configuration
    dotnet add package Serilog.Enrichers

Pop-Location



#-----------------------------------------------------------------------------------------------------------------------
# Restore and Build Solution
#-----------------------------------------------------------------------------------------------------------------------
Push-Location $Praedico.SolutionPath

    dotnet clean
    dotnet restore
    dotnet build  --no-restore

Pop-Location

#-----------------------------------------------------------------------------------------------------------------------
# Run API
#-----------------------------------------------------------------------------------------------------------------------
Push-Location $Praedico.ApiDirectoryPath

    dotnet run
    # dotnet run --urls $($Praedico.ApiProjectPath) # as variable

Pop-Location






#-----------------------------------------------------------------------------------------------------------------------
# Test SSH Connection
# Hi stevomccormack! You've successfully authenticated, but GitHub does not provide shell access.
#-----------------------------------------------------------------------------------------------------------------------

ssh -T git@github.com

#-----------------------------------------------------------------------------------------------------------------------
# Init repository
#-----------------------------------------------------------------------------------------------------------------------

Push-Location $Praedico.RootPath

    git config --global --add safe.directory $Praedico.RootPath

    # Init repository
    git init

    # Local config
    git config user.name $Git.UserName
    git config user.email $Git.UserEmail
    git config --local --list

    # Global config
    git config --global core.editor "code --wait"

    #-----------------------------------------------------------------------------------------------------------------------
    # Create main branch for new repository
    #-----------------------------------------------------------------------------------------------------------------------
    $mainBranchExists = git branch --list $Git.BranchMain
    if (-not $mainBranchExists) {

        # Create empty commit for main
        git commit --allow-empty -m "Initial commit to create main branch"

        # Create main branch and push
        git branch -M $Git.BranchMain
        git remote add $Git.Origin $Git.OriginUrl
        git remote -v
        git push -u $Git.Origin $Git.BranchMain
    } 
    else {
        Write-Host "Main branch already exists."
    }

    #-----------------------------------------------------------------------------------------------------------------------
    # Push files to repository
    #-----------------------------------------------------------------------------------------------------------------------   
    git commit -m "Draft version" -a
    git push -u $Git.Origin $Git.BranchMain

Pop-Location
