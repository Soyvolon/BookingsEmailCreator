using BookingsEmailCreator.Data.Db;

namespace BookingsEmailCreator.Data.Emails;

public class EmailTemplate
{
    public Guid Key { get; set; }
    public string Name { get; set; }
    public string Tempalte { get; set; }

    public AccountData Account { get; set; }
    public Guid AccountKey { get; set; }
}
