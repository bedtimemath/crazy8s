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

	dotnet ef -p .\Libraries\C8S.Database.EFCore -s .\Utilities\C8S.EFCoreSetup migrations script --idempotent -o $TargetFilePath
	Copy-Item "Libraries\C8S.Database.EFCore\Migrations" -Recurse $TargetFolder
}

