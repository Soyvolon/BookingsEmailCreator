using BookingsEmailCreator.Data.Emails;

namespace BookingsEmailCreator.Data.Db;

#nullable disable
public class AccountData
{
    public Guid Key { get; set; }
    public string MsId { get; set; }

    public List<EmailTemplate> EmailTemplates { get; set; } = new();
}
#nullable enable
