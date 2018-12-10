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
            DataTable dt = new DataTable("Func");
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (var ad = new SqlDataAdapter("SELECT X, Y FROM Func", con))
                    {
                        var comm = new SqlCommand();
                        comm.Connection = con;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = "UPDATE Func SET Y = @Y WHERE X = @X";
                        comm.Parameters.Add(new SqlParameter("@X", SqlDbType.Int, 4, "X"));
                        comm.Parameters.Add(new SqlParameter("@Y", SqlDbType.Int, 4, "Y"));
                        ad.UpdateCommand = comm;

                        comm = new SqlCommand();
                        comm.Connection = con;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = "INSERT INTO Func (X, Y) VALUES (@X, @Y)";
                        comm.Parameters.Add(new SqlParameter("@X", SqlDbType.Int, 4, "X"));
                        comm.Parameters.Add(new SqlParameter("@Y", SqlDbType.Int, 4, "Y"));
                        ad.InsertCommand = comm;

                        ad.Fill(dt);
                        dt.PrimaryKey = new DataColumn[] { dt.Columns["X"] };
                        var row = dt.Rows.Find(x);
                        if (row != null)
                        {
                            row["Y"] = y;
                        }
                        else
                        {
                            row = dt.NewRow();
                            row["X"] = x;
                            row["Y"] = y;
                            dt.Rows.Add(row);
                        }
                        ad.Update(dt);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(e.ToString());
                    return;
                }
            }
        }

        static void Delete(int x)
        {
            Console.WriteLine("Deleting " + x);
            DataTable dt = new DataTable("Func");
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (var ad = new SqlDataAdapter("SELECT X, Y FROM Func", con))
                    {
                        var comm = new SqlCommand();
                        comm.Connection = con;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = "DELETE FROM Func WHERE X = @X";
                        comm.Parameters.Add(new SqlParameter("@X", SqlDbType.Int, 4, "X"));
                        ad.DeleteCommand = comm;

                        ad.Fill(dt);
                        dt.PrimaryKey = new DataColumn[] { dt.Columns["X"] };
                        var row = dt.Rows.Find(x);
                        if (row != null)
                        {
                            row.Delete();
                        }
                        ad.Update(dt);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(e.ToString());
                    return;
                }
            }
        }
    }
}
