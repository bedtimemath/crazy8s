if (($args.Count -eq 0) -or ($args -ieq "-?") -or ($args -ieq "--help")) 
{
	Write-Output "SYNTAX: .\ef-add <MigrationName>"
}
else
{
	dotnet ef -p .\Domains\C8S.Domain.EFCore -s .\Utilities\C8S.EFCoreSetup migrations add --context C8SDbContext $args
}
