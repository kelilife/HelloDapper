using System;
using System.Reflection;

using KeLi.HelloDapper.SQLite.Models;
using KeLi.HelloDapper.SQLite.Properties;
using KeLi.HelloDapper.SQLite.Utils;
using KeLi.Power.Tool.Extensions;

namespace KeLi.HelloDapper.SQLite
{
    internal class Program
    {
        public static void Main()
        {
            var helper = new DapperHelper(typeof(Program).GetAppSettingsValue(Resources.Key_DatabasePostition));

            // Add data.
            {
                helper.Insert(new Student { Name = "Tom" });
                helper.Insert(new Student { Name = "Jack" });
                helper.Insert(new Student { Name = "Tony" });

                Console.WriteLine("After Added data:");

                foreach (var item in helper.QueryList<Student>())
                    Console.WriteLine(item.Name);
            }

            Console.WriteLine();

            // Delete data.
            {
                helper.Delete<Student>(d => d.Name.Contains("Tom"));

                Console.WriteLine("After Deleted data:");

                foreach (var item in helper.QueryList<Student>())
                    Console.WriteLine(item.Name);
            }

            Console.WriteLine();

            // Update data.
            {
                helper.Update<Student>(s => s.Name = "Alice", u => u.Name.Contains("Jack"));

                Console.WriteLine("After Updated data:");

                foreach (var item in helper.QueryList<Student>())
                    Console.WriteLine(item.Name);
            }

            Console.WriteLine();

            // Query data.
            {
                var students = helper.QueryList<Student>(q => q.Name.Contains("T"));

                Console.WriteLine("Query data:");

                foreach (var item in students)
                    Console.WriteLine(item.Name);
            }

            Console.ReadKey();
        }
    }
}