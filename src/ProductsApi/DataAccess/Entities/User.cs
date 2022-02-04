namespace RefactorThis.DataAccess.Entities;

public class User
{
    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ApiToken { get; set; } = string.Empty;

    public long ApiTokenExpiry { get; set; }

    public Guid ApiTokenGuid => Guid.Parse(this.ApiToken);
}