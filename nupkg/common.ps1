# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

function Write-Info   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Black -BackgroundColor Green

	try 
	{
	   $host.UI.RawUI.WindowTitle = $text
	}		
	catch 
	{
		#Changing window title is not suppoerted!
	}
}

function Write-Error   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Red -BackgroundColor Black 
}

function Seperator   
{
	Write-Host ("_" * 100)  -ForegroundColor gray 
}

function Get-Current-Version { 
	$commonPropsFilePath = resolve-path "../common.props"
	$commonPropsXmlCurrent = [xml](Get-Content $commonPropsFilePath ) 
	$currentVersion = $commonPropsXmlCurrent.Project.PropertyGroup.Version.Trim()
	return $currentVersion
}

function Get-Current-Branch {
	return git branch --show-current
}	   

function Read-File {
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $filePath
    )
		
	$pathExists = Test-Path -Path $filePath -PathType Leaf
	if ($pathExists)
	{
		return Get-Content $filePath		
	}
	else{
		Write-Error  "$filePath path does not exist!"
	}
}

# List of solutions
$solutions = (
    ""
)

# List of projects
$projects = (

    # src
    "src/Dignite.Cms.Admin.Application",
    "src/Dignite.Cms.Admin.Application.Contracts",
    "src/Dignite.Cms.Admin.Blazor",
    "src/Dignite.Cms.Admin.Blazor.Server",
    "src/Dignite.Cms.Admin.Blazor.WebAssembly",
    "src/Dignite.Cms.Admin.HttpApi",
    "src/Dignite.Cms.Admin.HttpApi.Client",
    "src/Dignite.Cms.Application",
    "src/Dignite.Cms.Application.Contracts",
    "src/Dignite.Cms.Common.Application.Contracts",
    "src/Dignite.Cms.Domain",
    "src/Dignite.Cms.Domain.Shared",
    "src/Dignite.Cms.EntityFrameworkCore",
    "src/Dignite.Cms.HttpApi",
    "src/Dignite.Cms.HttpApi.Client",
    "src/Dignite.Cms.Installer",
    "src/Dignite.Cms.MongoDB",
    "src/Dignite.Cms.Public.Application",
    "src/Dignite.Cms.Public.Application.Contracts",
    "src/Dignite.Cms.Public.HttpApi",
    "src/Dignite.Cms.Public.HttpApi.Client",
    "src/Dignite.Cms.Public.Web"
)
