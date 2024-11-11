if ($args.Count -eq 0) 
{
	dotnet ef -p .\Domains\C8S.Domain.EFCore -s .\Utilities\C8S.EFCoreSetup migrations list --context C8SDbContext
}
else
{
	dotnet ef -p .\Domains\C8S.Domain.EFCore -s .\Utilities\C8S.EFCoreSetup migrations list --context C8SDbContext $args
}


