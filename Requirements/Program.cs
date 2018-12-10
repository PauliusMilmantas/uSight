using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requirements
{
    class Program
    {
        static string connectionString;

        static void Main(string[] args)
        {
            connectionString = string.Format(ConfigurationManager.ConnectionStrings["FuncConnection"].ConnectionString, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases"));
            while (true)
            {
                Console.WriteLine("[1] Show function");
                Console.WriteLine("[2] Change mapping");
                Console.WriteLine("[3] Delete mapping");
                Console.WriteLine("[4] Quit");
                Console.WriteLine("Enter your action:");
                int input = int.Parse(Console.ReadLine());

                if (input == 1)
                {
                    Show();
                }
                else if (input == 2)
                {
                    Console.WriteLine("Enter x:");
                    int x = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter y:");
                    int y = int.Parse(Console.ReadLine());
                    Change(x, y);
                }
                else if (input == 3)
                {
                    Console.WriteLine("Enter x:");
                    int x = int.Parse(Console.ReadLine());
                    Delete(x);
                }
                else if (input == 4)
                {
                    break;
                }
            }
        }

        static void Show()
        {
            Console.WriteLine("Showing all:");
            DataTable dt = new DataTable("Func");
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (var ad = new SqlDataAdapter("SELECT X, Y FROM Func", con))
                    {
                        ad.Fill(dt);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(e.ToString());
                    return;
                }
            }

            Console.WriteLine("X -> Y");
            Console.WriteLine("------");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Console.WriteLine(dt.Rows[i]["X"] + " -> " + dt.Rows[i]["Y"]);
            }
        }

        static void Change(int x, int y)
        {
            Console.WriteLine("Changing to reflect " + x + " -> " + y);
        }

        static void Delete(int x)
        {
            Console.WriteLine("Deleting " + x);
        }
    }
}
