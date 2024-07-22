if (($args.Count -eq 0) -or ($args -ieq "-?") -or ($args -ieq "--help")) 
{
	Write-Output "SYNTAX: .\ef-add <MigrationName>"
}
else
{
	dotnet ef -p .\Libraries\C8S.Database.EFCore -s .\Utilities\C8S.EFCoreSetup migrations add $args
}
