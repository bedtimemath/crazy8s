if (($args.Count -eq 0) -or ($args -ieq "-?") -or ($args -ieq "--help")) 
{
	Write-Output "SYNTAX: .\ef-script <MigrationName>"
}
else
{
	$NowString = (Get-Date).ToUniversalTime().ToString("yyyyMMddHH")
	$MigrationName = $args[0]

	$TargetFolder = New-Item -Force -Path "..\Deploy\C8sDb\$NowString-$MigrationName" -ItemType Directory
	$TargetFilePath = "$($TargetFolder.FullName)\$MigrationName-$NowString.sql"

	dotnet ef -p .\Domains\C8S.Domain.EFCore -s .\Utilities\C8S.EFCoreSetup migrations script --context C8SDbContext --idempotent -o $TargetFilePath
	Copy-Item "Domains\C8S.Domain.EFCore\Migrations" -Recurse $TargetFolder
}

