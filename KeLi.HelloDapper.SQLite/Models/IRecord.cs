namespace KeLi.HelloDapper.SQLite.Models
{
    public interface IRecord
    {
        long Id { get; set; }

        string Name { get; set; }

        string Remark { get; set; }
    }
}