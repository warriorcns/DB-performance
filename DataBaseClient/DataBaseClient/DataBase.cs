using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Resources;
using System.Reflection;
using System.Data;
using System.Threading;

namespace DataBaseClient
{
    class DataBase
    {
        //by zabezpieczyc login credensials warto uzyc klas:
        /*
         * System.Security.Cryptography.ProtectedData
         * SecureString 
         */
        private static string login = Properties.Settings.Default.Login.ToString();
        private static string password = Properties.Settings.Default.Password.ToString();
        private static String connStr = Properties.Settings.Default.SQLConnectionString.ToString();
        private static string db = Properties.Settings.Default.DataBaseName.ToString();
        private static string server = Properties.Settings.Default.Server.ToString();
        private static string ISecurity = Properties.Settings.Default.ISecurity.ToString();
        private SqlConnection Connection;
        private static int workersCount = 60;
        private ManualResetEvent _doneEvent;
        private static int proceduresCount = 5;
        private static string[] selectProcedures = new string[proceduresCount];
        private static string[] updateProcedures = new string[proceduresCount];
        private static string[] insertProcedures = new string[proceduresCount];
        private Object thisLock = new Object();
        private ManualResetEvent[] doneEvents = new ManualResetEvent[workersCount];

        public void initializeData()
        {
            Console.WriteLine("Initializing data...");
            for (int i = 0; i < proceduresCount; i++)
            {
                selectProcedures[i] = string.Empty;
                updateProcedures[i] = string.Empty;
            }
            Console.WriteLine("Data initialized.");
        }
        // for each procedure - @rowNumber : parameter
        private static void populateProceduresTable()
        {
            selectProcedures[0] = Queries.selectUser;
            selectProcedures[1] = Queries.selectPesel;
            selectProcedures[2] = Queries.selectThing;
            selectProcedures[3] = Queries.selectCategory;

            updateProcedures[0] = Queries.updateUser;
            updateProcedures[1] = Queries.updatePesels;
            updateProcedures[2] = Queries.updateThing;
            updateProcedures[3] = Queries.updateCategory;

            insertProcedures[0] = Queries.insertUser;
            insertProcedures[1] = Queries.insertPesel;
            insertProcedures[2] = Queries.insertThing;
            insertProcedures[3] = Queries.insertCategory;
        }

        public bool populateWithRandomData(int numberOfRows)
        { 
            bool output = false;
            int gausianRandom = 0;
            int maxUsersTableID = getMaxIDFromTable("dbo.Users");
            int maxCategoriesTableID = getMaxIDFromTable("dbo.Categories");
            int maxThingsTableID = getMaxIDFromTable("dbo.Things");
            int maxPeselsTableID = getMaxIDFromTable("dbo.Pesels");
            deleteDatabaseData();
            Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");
            
                try
                {
                    Connection.Open();
                    Console.WriteLine("Connection open....");
                    //object DbConnection myConn = myDB.CreateConnection();

                    SqlCommand command = Connection.CreateCommand();
                    SqlTransaction transaction;
                    Random randomNumber = new Random();

                    // Start a local transaction.
                    transaction = Connection.BeginTransaction("PopulateDatabaseTransaction");

                    // Must assign both transaction object and connection 
                    // to Command object for a pending local transaction
                    command.Connection = Connection;
                    command.Transaction = transaction;
                    try
                    {
                        command.CommandText = Queries.populateDatabaseProcedure;

                        //parameters into mssql procedure
                        //@userMainID, @userName, @thingMainID, @peselMainID, @categoryMainID, @thingName, @number, @categoryName,	@connectTableMainID
                        command.Parameters.Add("@userMainID", SqlDbType.Int);
                        command.Parameters.Add("@userName", SqlDbType.Text);
                        command.Parameters.Add("@thingMainID", SqlDbType.Int);
                        command.Parameters.Add("@peselMainID", SqlDbType.Int);
                        command.Parameters.Add("@categoryMainID", SqlDbType.Int);
                        command.Parameters.Add("@thingName", SqlDbType.Text);
                        command.Parameters.Add("@number", SqlDbType.Decimal);
                        command.Parameters.Add("@categoryName", SqlDbType.Text);
                        command.Parameters.Add("@connectTableMainID", SqlDbType.Int);


                        if (maxUsersTableID == 0 || maxCategoriesTableID == 0 || maxThingsTableID == 0 || maxPeselsTableID == 0)
                        {
                            maxUsersTableID = 1;
                            maxCategoriesTableID = 1;
                            maxThingsTableID = 1;
                            maxPeselsTableID = 1;
                        }


                        for (int i = 1; i <= numberOfRows; i++)
                        {
                            gausianRandom = randomNumber.Next(1, Convert.ToInt32(numberOfRows * 0.5)) + randomNumber.Next(1, Convert.ToInt32(numberOfRows * 0.5)); 
                            command.Parameters["@userMainID"].Value = i;
                            command.Parameters["@userName"].Value = "jakies name" + gausianRandom.ToString();
                            command.Parameters["@thingMainID"].Value = i;
                            command.Parameters["@peselMainID"].Value = i;
                            command.Parameters["@categoryMainID"].Value = i;
                            command.Parameters["@thingName"].Value = "thing name" + gausianRandom.ToString();
                            command.Parameters["@number"].Value = 8912371123123;
                            command.Parameters["@categoryName"].Value = "sok" + gausianRandom.ToString();
                            command.Parameters["@connectTableMainID"].Value = i;
                            command.ExecuteNonQuery();
                        }

                        // Attempt to commit the transaction.
                        transaction.Commit();
                        Console.WriteLine("Records are written to database.");
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                        Console.WriteLine("  Message: {0}", e.Message);

                        // Attempt to roll back the transaction. 
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred 
                            // on the server that would cause the rollback to fail, such as 
                            // a closed connection.
                            Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            Console.WriteLine("  Message: {0}", ex2.Message);
                        }

                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("You failed!" + ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("InvalidOperationException" + ex2.Message);
                }
                finally
                {
                    Connection.Close();
                }
            
            return output;
        }

        private int getMaxIDFromTable(string tableInDatabase)
        {
            int id = 0;
            Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");
            try
            {

                Connection.Open();

                SqlCommand command = Connection.CreateCommand();
                SqlTransaction transaction;
                SqlDataReader reader;

                transaction = Connection.BeginTransaction("GetMaxID");

                command.Connection = Connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "SELECT MAX(id) FROM " + tableInDatabase;
                    //command.Parameters.Add("@tableName", SqlDbType.Text);
                    //command.Parameters["@tableName"].Value = tableInDatabase;

                    reader = command.ExecuteReader();

                    while (reader.Read() && reader[0] != DBNull.Value)
                    {
                        id = reader.GetInt32(0);
                    }
                    reader.Close();

                    transaction.Commit();
                }

                catch (Exception e)
                {
                    Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                    Console.WriteLine("  Message: {0}", e.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred 
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("You failed!" + ex.Message);
            }
            catch (InvalidOperationException ex2)
            {
                Console.WriteLine("InvalidOperationException" + ex2.Message);
            }
            finally
            {
                Connection.Close();
            }
            return id;
        }

        private void deleteDatabaseData()
        {
            Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");
            try
            {

                Connection.Open();

                SqlCommand command = Connection.CreateCommand();
                SqlTransaction transaction;
                SqlDataReader reader;

                transaction = Connection.BeginTransaction("GetMaxID");

                command.Connection = Connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = Queries.deleteData;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }

                catch (Exception e)
                {
                    Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                    Console.WriteLine("  Message: {0}", e.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("You failed!" + ex.Message);
            }
            catch (InvalidOperationException ex2)
            {
                Console.WriteLine("InvalidOperationException" + ex2.Message);
            }
            finally
            {
                Connection.Close();
            }
        }

        private void doSelectOperationsOnDB(Object threadContext)
        {

            //using (Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";"))
            //{
                int threadIndex = (int)threadContext;
                Console.WriteLine("thread {0} started...", threadIndex);
                _doneEvent = new ManualResetEvent(false);
                try
                {

                    //Connection.Open();
                    //Console.WriteLine("Connection open....");
                    //object DbConnection myConn = myDB.CreateConnection();
                    lock (thisLock)
                    {


                        SqlCommand command = Connection.CreateCommand();
                        SqlTransaction transaction;
                        Random randomNumber = new Random();
                        //select procedure -> random selected from select procedure tables




                        //Console.WriteLine("Executing..");
                        //Console.WriteLine("thread {0} FINISHED...", threadIndex);



                        #region transaction
                        // Start a local transaction.
                        transaction = Connection.BeginTransaction("Select");

                        // Must assign both transaction object and connection 
                        // to Command object for a pending local transaction
                        /////command.Connection = Connection;

                        try
                        {

                            command.Connection = Connection;
                            command.Transaction = transaction;
                            //select random procedure from matrix of select procedures
                            command.CommandText = selectProcedures[randomNumber.Next(0, 3)];

                            //command.CommandText = Queries.populateDatabaseProcedure;
                            //command.CommandText = selectProcedures[0];
                            command.Parameters.Add("@rowNumber", SqlDbType.Int);

                            //symulacja uspienia
                            //Thread.Sleep(1000);
                            command.Parameters["@rowNumber"].Value = randomNumber.Next();
                            command.ExecuteNonQuery();



                            //parameters into mssql procedure
                            //@userMainID, @userName, @thingMainID, @peselMainID, @categoryMainID, @thingName, @number, @categoryName,	@connectTableMainID
                            //command.Parameters.Add("@userMainID", SqlDbType.Int);

                            // Attempt to commit the transaction.
                            transaction.Commit();
                            //Console.WriteLine("Transaction commited.");

                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                            Console.WriteLine("  Message: {0}", e.Message);

                            // Attempt to roll back the transaction. 
                            try
                            {
                                //transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                // This catch block will handle any errors that may have occurred 
                                // on the server that would cause the rollback to fail, such as 
                                // a closed connection.
                                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                                Console.WriteLine("  Message: {0}", ex2.Message);
                            }

                        }
                        #endregion
                        //_doneEvent.Set();
                        doneEvents[threadIndex].Set();
                        Console.WriteLine("_doneEvent.Set(TRUE)" + threadIndex);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("You failed!" + ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("InvalidOperationException" + ex2.Message);
                }
                
            //}
        }

        //wywoluje watki, ktore wykonuja metode doSelectOperationsOnDB
        public void selectMultithreaded()
        {
            

            //Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");
            using (Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";"))
            {
                try
                {
                    Connection.Open();
                    Console.WriteLine("Connection open....");
                    //object DbConnection myConn = myDB.CreateConnection();

                    SqlCommand command = Connection.CreateCommand();
                    SqlTransaction transaction;
                    Random randomNumber = new Random();

                    //insert procedures into matrix
                    populateProceduresTable();

                    // Configure and start threads using ThreadPool.
                    Console.WriteLine("launching {0} tasks...", workersCount);
                    Console.WriteLine("Minions are working.. please wait.");
                    for (int i = 0; i < workersCount; i++)
                    {
                        doneEvents[i] = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(doSelectOperationsOnDB), (object)i);
                    }
                    
                    // Wait for all threads in pool to calculate.
                    
                    WaitHandle.WaitAll(doneEvents);
                    Console.WriteLine("All workers FINISHED.");


                    #region transactions
                    // Start a local transaction.
                    //transaction = Connection.BeginTransaction("Select");

                    // Must assign both transaction object and connection 
                    // to Command object for a pending local transaction
                    //command.Connection = Connection;
                    //command.Transaction = transaction;
                    try
                    {
                        //command.CommandText = Queries.populateDatabaseProcedure;

                        //parameters into mssql procedure
                        //@userMainID, @userName, @thingMainID, @peselMainID, @categoryMainID, @thingName, @number, @categoryName,	@connectTableMainID
                        //command.Parameters.Add("@userMainID", SqlDbType.Int);

                        // Attempt to commit the transaction.
                        //transaction.Commit();

                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                        Console.WriteLine("  Message: {0}", e.Message);

                        // Attempt to roll back the transaction. 
                        try
                        {
                            //transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred 
                            // on the server that would cause the rollback to fail, such as 
                            // a closed connection.
                            Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            Console.WriteLine("  Message: {0}", ex2.Message);
                        }

                    }
                    #endregion
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("You failed!" + ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("InvalidOperationException" + ex2.Message);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        private void doUpdateOperationsOnDB(Object threadContext)
        { 
             //using (Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";"))
            //{
                int threadIndex = (int)threadContext;
                Console.WriteLine("thread {0} started...", threadIndex);
                _doneEvent = new ManualResetEvent(false);
                
                try
                {

                    //Connection.Open();
                    //Console.WriteLine("Connection open....");
                    //object DbConnection myConn = myDB.CreateConnection();
                    lock (thisLock)
                    {
                        Random randomNumber = new Random();
                        int gausianRandom = 0; 
                        gausianRandom = randomNumber.Next(1, 500000) + randomNumber.Next(1, 500000); 
                        SqlCommand command = Connection.CreateCommand();
                        SqlTransaction transaction;
                        
                        //select procedure -> random selected from select procedure tables

                        #region transaction
                        // Start a local transaction.
                        transaction = Connection.BeginTransaction("Update");

                        // Must assign both transaction object and connection 
                        // to Command object for a pending local transaction
                        /////command.Connection = Connection;

                        try
                        {

                            command.Connection = Connection;
                            command.Transaction = transaction;
                            //select random procedure from matrix of select procedures
                            command.CommandText = updateProcedures[randomNumber.Next(0, 3)];

                            //command.CommandText = Queries.populateDatabaseProcedure;
                            //command.CommandText = selectProcedures[0];
                            command.Parameters.Add("@name", SqlDbType.Text);
                            command.Parameters.Add("@rowNumber", SqlDbType.Int);

                            if (command.CommandText == updateProcedures[1])
                            {
                                command.Parameters.Add("@randomNumber", SqlDbType.Int);
                                command.Parameters["@randomNumber"].Value = randomNumber.Next();
                            }

                            //symulacja uspienia
                            //Thread.Sleep(1000);

                            command.Parameters["@name"].Value = "name" + gausianRandom;
                            command.Parameters["@rowNumber"].Value = randomNumber.Next(1, 1000000);
                            command.ExecuteNonQuery();


                            // Attempt to commit the transaction.
                            transaction.Commit();
                            //Console.WriteLine("Transaction commited.");

                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                            Console.WriteLine("  Message: {0}", e.Message);

                            // Attempt to roll back the transaction. 
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                // This catch block will handle any errors that may have occurred 
                                // on the server that would cause the rollback to fail, such as 
                                // a closed connection.
                                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                                Console.WriteLine("  Message: {0}", ex2.Message);
                            }

                        }
                        #endregion
                        //_doneEvent.Set();
                        doneEvents[threadIndex].Set();
                        Console.WriteLine("_doneEvent.Set(TRUE)" + threadIndex);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("You failed!" + ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("InvalidOperationException" + ex2.Message);
                }
        }

        public void updateMultithreaded()
        {
            //Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");
            using (Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";"))
            {
                try
                {
                    Connection.Open();
                    Console.WriteLine("Connection open....");
                    //object DbConnection myConn = myDB.CreateConnection();

                    SqlCommand command = Connection.CreateCommand();
                    //SqlTransaction transaction;
                    Random randomNumber = new Random();

                    //insert procedures into matrix
                    populateProceduresTable();

                    // Configure and start threads using ThreadPool.
                    Console.WriteLine("launching {0} tasks...", workersCount);
                    Console.WriteLine("Minions are working.. please wait.");
                    for (int i = 0; i < workersCount; i++)
                    {
                        doneEvents[i] = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(doUpdateOperationsOnDB), (object)i);
                    }

                    // Wait for all threads in pool to calculate.

                    WaitHandle.WaitAll(doneEvents);
                    Console.WriteLine("All workers FINISHED.");

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("You failed!" + ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("InvalidOperationException" + ex2.Message);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }


        private void doInsertOperationsOnDB(Object threadContext)
        {
            //using (Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";"))
            //{
            int threadIndex = (int)threadContext;
            Console.WriteLine("thread {0} started...", threadIndex);
            _doneEvent = new ManualResetEvent(false);

            try
            {

                //Connection.Open();
                //Console.WriteLine("Connection open....");
                //object DbConnection myConn = myDB.CreateConnection();
                lock (thisLock)
                {
                    Random randomNumber = new Random();
                    int gausianRandom = 0;
                    gausianRandom = randomNumber.Next(1, 500000) + randomNumber.Next(1, 500000);
                    SqlCommand command = Connection.CreateCommand();
                    SqlTransaction transaction;

                    //select procedure -> random selected from select procedure tables

                    #region transaction
                    // Start a local transaction.
                    transaction = Connection.BeginTransaction("Update");

                    // Must assign both transaction object and connection 
                    // to Command object for a pending local transaction
                    /////command.Connection = Connection;

                    try
                    {

                        command.Connection = Connection;
                        command.Transaction = transaction;
                        //select random procedure from matrix of select procedures
                        command.CommandText = insertProcedures[randomNumber.Next(0, 3)];

                        //command.CommandText = Queries.populateDatabaseProcedure;
                        //command.CommandText = selectProcedures[0];
                        command.Parameters.Add("@name", SqlDbType.Text);
                        

                        if (command.CommandText == insertProcedures[1])
                        {
                            command.Parameters.Add("@number", SqlDbType.Int);
                            command.Parameters["@number"].Value = randomNumber.Next();
                        }

                        //symulacja uspienia
                        //Thread.Sleep(1000);

                        command.Parameters["@name"].Value = "name" + gausianRandom;
                        
                        command.ExecuteNonQuery();


                        // Attempt to commit the transaction.
                        transaction.Commit();
                        //Console.WriteLine("Transaction commited.");

                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                        Console.WriteLine("  Message: {0}", e.Message);

                        // Attempt to roll back the transaction. 
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred 
                            // on the server that would cause the rollback to fail, such as 
                            // a closed connection.
                            Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            Console.WriteLine("  Message: {0}", ex2.Message);
                        }

                    }
                    #endregion
                    //_doneEvent.Set();
                    doneEvents[threadIndex].Set();
                    Console.WriteLine("_doneEvent.Set(TRUE)" + threadIndex);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("You failed!" + ex.Message);
            }
            catch (InvalidOperationException ex2)
            {
                Console.WriteLine("InvalidOperationException" + ex2.Message);
            }
        }

        public void insertMultithreaded()
        {
            //Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");
            using (Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";"))
            {
                try
                {
                    Connection.Open();
                    Console.WriteLine("Connection open....");
                    //object DbConnection myConn = myDB.CreateConnection();

                    SqlCommand command = Connection.CreateCommand();
                    //SqlTransaction transaction;
                    Random randomNumber = new Random();

                    //insert procedures into matrix
                    populateProceduresTable();

                    // Configure and start threads using ThreadPool.
                    Console.WriteLine("launching {0} tasks...", workersCount);
                    Console.WriteLine("Minions are working.. please wait.");
                    for (int i = 0; i < workersCount; i++)
                    {
                        doneEvents[i] = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(doInsertOperationsOnDB), (object)i);
                    }

                    // Wait for all threads in pool to calculate.

                    WaitHandle.WaitAll(doneEvents);
                    Console.WriteLine("All workers FINISHED.");

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("You failed!" + ex.Message);
                }
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("InvalidOperationException" + ex2.Message);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }


        //metoda ramowa - szkic
        private void sample()
        {
            Connection = new SqlConnection("Database=" + db + ";Server=" + server + ";Integrated Security=" + ISecurity + ";connect timeout = 30;" + "User id=" + login + ";Password=" + password + ";");

            try
            {
                Connection.Open();
                Console.WriteLine("Connection open....");
                //object DbConnection myConn = myDB.CreateConnection();

                SqlCommand command = Connection.CreateCommand();
                SqlTransaction transaction;
                Random randomNumber = new Random();

                // Start a local transaction.
                transaction = Connection.BeginTransaction("PopulateDatabaseTransaction");

                // Must assign both transaction object and connection 
                // to Command object for a pending local transaction
                command.Connection = Connection;
                command.Transaction = transaction;
                try
                {
                    //command.CommandText = Queries.populateDatabaseProcedure;

                    //parameters into mssql procedure
                    //@userMainID, @userName, @thingMainID, @peselMainID, @categoryMainID, @thingName, @number, @categoryName,	@connectTableMainID
                    //command.Parameters.Add("@userMainID", SqlDbType.Int);

                    // Attempt to commit the transaction.
                    transaction.Commit();

                }

                catch (Exception e)
                {
                    Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                    Console.WriteLine("  Message: {0}", e.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred 
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }

                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("You failed!" + ex.Message);
            }
            catch (InvalidOperationException ex2)
            {
                Console.WriteLine("InvalidOperationException" + ex2.Message);
            }
            finally
            {
                Connection.Close();
            }

        }


    }
}
