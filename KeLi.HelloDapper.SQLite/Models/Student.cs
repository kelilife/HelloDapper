namespace KeLi.HelloDapper.SQLite.Models
{
    public class Student : IRecord
    {
        public string Email { get; set; }

        public string Address { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }
    }
}