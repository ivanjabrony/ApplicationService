namespace ConfApplicationService.Models.ResultTypes;

public record RepoActResult : ActionResultType
{
    public sealed record SuccessRepoAct : RepoActResult;
    public sealed record FailRepoAct(string Message) : RepoActResult;
}