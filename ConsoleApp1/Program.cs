using Microsoft.Extensions.Configuration;

using System;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                        .Build();

            Console.WriteLine(config.GetConnectionString("LISP"));
        }
    }
}
