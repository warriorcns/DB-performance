using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using DataBaseClient;

namespace DataBaseClient
{
    class Program
    {



        static void Main(string[] args)
        {


            Console.WriteLine("Welcome user, chose an option:");
            Console.WriteLine("1. Populate database with 1M rows (use with caution)");
            Console.WriteLine("2. Simulate SELECT statement on local DB");
            Console.WriteLine("3. Simulate UPDATE statement on local DB");
            Console.WriteLine("4. Simulate INSERT statement on local DB");
            Console.WriteLine("All simulations uses 60 threads per simulation.");

            

            DataBase dbclient = new DataBase();
            //for (int i = 1; i < 10;i++)

            int n = Console.Read();
            dbclient.initializeData();
            switch (n)
            {
                case '1':
                    //temporary commented for safety reasons
                    //dbclient.populateWithRandomData(1000000);
                    break;
                case '2':
                    //add select simulation method
                    Console.WriteLine("You have selected option 2.");
                    //
                    dbclient.selectMultithreaded();
                    break;
                case '3':
                    Console.WriteLine("You have selected option 3.");
                    dbclient.updateMultithreaded();
                    break;
                case '4':
                    dbclient.insertMultithreaded();
                    break;
                default:
                    break;
            }


            

            Console.ReadKey();
        }
    }
}
