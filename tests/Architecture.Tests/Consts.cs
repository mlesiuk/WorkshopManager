namespace Architecture.Tests;

public static class Consts
{
    public const string DomainNamespace = "workshopManager.Domain";
    public const string ApplicationNamespace = "workshopManager.Application";
    public const string InfrastructureNamespace = "workshopManager.Infrastructure";
    public const string ApiNamespace = "workshopManager.Api";

    public const string ApplicationCommandsNamespace = "workshopManager.Application.Commands";
    public const string ApplicationQueriesNamespace = "workshopManager.Application.Queries";
    public const string ApplicationValidatorsNamespace = "workshopManager.Application.Validators";

    public const string Command = nameof(Command);
    public const string Handler = nameof(Handler);
    public const string Query = nameof(Query);
    public const string Validator = nameof(Validator);
}
