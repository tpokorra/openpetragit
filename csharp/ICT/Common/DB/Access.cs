/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using Ict.Common.DB.DBCaching;
using Ict.Common.IO;

namespace Ict.Common.DB
{
    /// <summary>
    /// <see cref="IsolationLevel" /> that needs to be enforced when requesting a
    /// DB Transaction with Methods
    /// <see cref="DB.TDataBase.GetNewOrExistingTransaction(IsolationLevel, out bool)" /> and
    /// <see cref="DB.TDataBase.GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool)" />.
    /// </summary>
    public enum TEnforceIsolationLevel
    {
        /// <summary>
        /// <see cref="IsolationLevel" /> of current Transaction must match the
        /// specified <see cref="IsolationLevel" />  <em>exactly</em>.
        /// </summary>
        eilExact,

        /// <summary>
        /// <see cref="IsolationLevel" /> of current Transaction must match or
        /// exceed the specified <see cref="IsolationLevel" />.
        /// </summary>
        eilMinimum
    }

    /// <summary>
    /// Contains some Constants and a Global Variable for use with Database Access.
    /// </summary>
    public class DBAccess
    {
        /// <summary>DebugLevel for logging the SQL code from DB queries</summary>
        public const Int32 DB_DEBUGLEVEL_QUERY = 3;

        /// <summary>DebugLevel for logging results from DB queries: is 10 (was 4 before)</summary>
        public const Int32 DB_DEBUGLEVEL_RESULT = 10;

        /// <summary>DebugLevel for tracing (most verbose log output): is 10 (was 4 before)</summary>
        public const Int32 DB_DEBUGLEVEL_TRACE = 10;

        /// <summary>Global Object in which the Application can store a reference to an Instance of
        /// <see cref="TDataBase" /></summary>
        public static TDataBase GDBAccessObj;
    }

    /// <summary>
    /// every database system that works for OpenPetra has to implement this functions
    /// </summary>
    public interface IDataBaseRDBMS
    {
        /// <summary>
        /// Create a connection, but not opening it yet
        /// </summary>
        /// <param name="AServer"></param>
        /// <param name="APort"></param>
        /// <param name="ADatabaseName"></param>
        /// <param name="AUsername"></param>
        /// <param name="APassword"></param>
        /// <param name="AConnectionString"></param>
        /// <param name="AStateChangeEventHandler"></param>
        /// <returns></returns>
        IDbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler);

        /// <summary>
        /// this is for special Exceptions that are specific to the database
        /// they are converted to a string message for logging
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns></returns>
        bool LogException(Exception AException, ref string AErrorMessage);

        /// <summary>
        /// Formats a SQL query for a specific RDBMS.
        /// Put the Schema specifier in front of table names! Format: PUB_*
        /// (eg. PUB_p_partner).
        /// </summary>
        /// <remarks>
        /// Always use ANSI SQL-92 commands that are understood by all RDBMS
        /// systems that should be supported - this does no 'translation' of the
        /// SQL commands!
        /// </remarks>
        /// <param name="ASqlQuery">SQL query</param>
        /// <returns>SQL query that is formatted for a specific RDBMS.
        /// </returns>
        String FormatQueryRDBMSSpecific(String ASqlQuery);

        /// <summary>
        /// convert the ODBC
        /// </summary>
        /// <param name="AParameterArray"></param>
        /// <param name="ASqlStatement"></param>
        /// <returns></returns>
        DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement);

        /// <summary>
        /// create a IDbCommand object
        /// this formats the sql query for the database, and transforms the parameters
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        IDbCommand NewCommand(ref string ACommandText, IDbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction);

        /// <summary>
        /// create an IDbDataAdapter
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter NewAdapter();

        /// <summary>
        /// fill an IDbDataAdapter that was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataSet"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="ADataTableName"></param>
        void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataSet AFillDataSet,
            Int32 AStartRecord,
            Int32 AMaxRecords,
            string ADataTableName);

        /// <summary>
        /// fill an IDbDataAdapter that was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataTable"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataTable AFillDataTable,
            Int32 AStartRecord,
            Int32 AMaxRecords);

        /// <summary>
        /// some databases have some problems with certain Isolation levels
        /// </summary>
        /// <param name="AIsolationLevel"></param>
        /// <returns>true if isolation level was modified</returns>
        bool AdjustIsolationLevel(ref IsolationLevel AIsolationLevel);

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection);

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection);
    }

    /// <summary>
    /// Contains functions that open and close the connection to the DB, allow
    /// execution of SQL statements and creation of DB Transactions.
    /// It is designed to support connections to different kinds of databases;
    /// there needs to be an implementation of the interface IDataBaseRDBMS to support an RDBMS.
    ///
    /// Always use ANSI SQL-92 commands that are understood by all RDBMS
    ///   systems that should be supported - TDataBase does no 'translation' of the
    ///   SQL commands!
    ///   The TDataBase class is the only Class that a developer needs to deal with
    ///   when accessing DB's! (The TDBConnection class is a 'low-level' class that
    ///   is intended to be used only by the TDataBase class.)
    ///   Due to the limitations of native ODBC drivers, only one DataTable is ever
    ///   returned when you call IDbDataAdapter.FillSchema. This is true even when
    ///   executing SQL batch statements from which multiple DataTable objects would
    ///   be expected! TODO: this comment needs revising, with native drivers
    /// </summary>
    public class TDataBase : MarshalByRefObject
    {
        /// <summary>References the DBConnection instance</summary>
        private TDBConnection FDBConnectionInstance;

        /// <summary>References an open DB connection</summary>
        private IDbConnection FSqlConnection;

        /// <summary>References the type of RDBMS that we are currently connected to</summary>
        private TDBType FDbType;

        /// <summary> this is a reference to the specific database functions which can be different for each RDBMS</summary>
        private IDataBaseRDBMS FDataBaseRDBMS;

        /// <summary>For logging purposes.</summary>
        /// <remarks>See <see cref="DebugLevel" /> for details.</remarks>
        private Int16 FDebugLevel;

        /// <summary>Tracks the last DB action; is updated with every creation of a Command.</summary>
        private DateTime FLastDBAction;

        /// <summary>References the current Transaction, if there is any.</summary>
        private IDbTransaction FTransaction;

        /// <summary>Tells whether the next Command that is sent to the DB should be a 'prepared' Command.</summary>
        /// <remarks>Automatically reset to false once the Command has been executed against the DB!</remarks>
        private bool FPrepareNextCommand = false;

        /// <summary>Sets a timeout (in seconds) for the next Command that is sent to the
        /// DB that is different from the default timeout for a Command (eg. 20s for a
        /// NpgsqlCommand).</summary>
        /// <remarks>Automatically reset to -1 once the Command has been executed against the DB!</remarks>
        private int FTimeoutForNextCommand = -1;


        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// The Database type will be specified only when one of the <c>EstablishDBConnection</c>
        /// Methods gets called
        /// </summary>
        public TDataBase() : base()
        {
        }

        /// <summary>
        /// Constructor that specifies which Database type will be used with
        /// this Instance of <see cref="TDataBase" />.
        /// </summary>
        /// <param name="ADBType">Type of RDBMS (Relational Database Management System)</param>
        public TDataBase(TDBType ADBType) : base()
        {
            FDbType = ADBType;
        }

        #endregion

        #region Properties

        /// <summary>Returns the type of the RDBMS that the current Instance of
        /// <see cref="TDataBase" /> is connect to.</summary>
        public String DBType
        {
            get
            {
                return FDbType.ToString("G");
            }
        }

        /// <summary>
        /// Sets the DebugLevel (for logging purposes).
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <item>
        ///     <term><b>Level DB_DEBUGLEVEL_QUERY</b></term>
        ///     <description>Prints the SQL Query Level</description>
        /// </item>
        /// <item>
        ///     <term><b>Level DB_DEBUGLEVEL_RESULT</b></term>
        ///     <description>Prints the results Level</description>
        /// </item>
        /// <item>
        ///     <term><b>Level DB_DEBUGLEVEL_TRACE</b></term>
        ///     <description>Prints a trace of all database activities (very verbose!)</description>
        /// </item>
        /// </list>
        /// </remarks>
        public Int16 DebugLevel
        {
            get
            {
                return FDebugLevel;
            }

            set
            {
                FDebugLevel = value;
            }
        }

        /// <summary>Tells whether it's save to execute any SQL command on the DB. It is
        /// updated when the DB connection's State changes.</summary>
        public bool ConnectionOK
        {
            get
            {
                return ConnectionReady();
            }
        }

        /// <summary>Tells when the last Database action was carried out by the caller.</summary>
        public DateTime LastDBAction
        {
            get
            {
                return FLastDBAction;
            }
        }

        /// <summary>
        /// The current Transaction, if there is any.
        /// </summary>
        public TDBTransaction Transaction
        {
            get
            {
                if (FTransaction == null)
                {
                    return null;
                }
                else
                {
                    return new TDBTransaction(FTransaction, FSqlConnection);
                }
            }
        }

        #endregion


        /// <summary>
        /// Establishes (opens) a DB connection to a specified RDBMS.
        /// </summary>
        /// <param name="ADataBaseType">Type of the RDBMS to connect to. At the moment only PostgreSQL is officially supported.</param>
        /// <param name="ADsnOrServer">In case of an ODBC Connection: DSN (Data Source Name). In case of a PostgreSQL connection: Server.</param>
        /// <param name="ADBPort">In case of a PostgreSQL connection: port that the db server is running on.</param>
        /// <param name="ADatabaseName">the database to connect to</param>
        /// <param name="AUsername">User which should be used for connecting to the DB server</param>
        /// <param name="APassword">Password of the User which should be used for connecting to the DB server</param>
        /// <param name="AConnectionString">If this is not empty, it is prefered over the Dsn and Username and Password</param>
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown when a connection cannot be established</exception>
        public void EstablishDBConnection(TDBType ADataBaseType,
            String ADsnOrServer,
            String ADBPort,
            String ADatabaseName,
            String AUsername,
            String APassword,
            String AConnectionString)
        {
            FDbType = ADataBaseType;

            if (FDbType == TDBType.PostgreSQL)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TPostgreSQL();
            }
            else if (FDbType == TDBType.MySQL)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TMySQL();
            }
            else if (FDbType == TDBType.SQLite)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TSQLite();
            }
            else if (FDbType == TDBType.ProgressODBC)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TProgressODBC();
            }

            if (ConnectionReady())
            {
                TLogging.Log("Error establishing connection to Database Server: connection is already open!");
                throw new EDBConnectionNotAvailableException(
                    FSqlConnection != null ? FSqlConnection.State.ToString("G") : "FSqlConnection is null");
            }

            TDBConnection CurrentConnectionInstance;

            if (FSqlConnection == null)
            {
                FDBConnectionInstance = TDBConnection.GetInstance();
                CurrentConnectionInstance = FDBConnectionInstance;

                FSqlConnection = CurrentConnectionInstance.GetConnection(
                    FDataBaseRDBMS,
                    ADsnOrServer,
                    ADBPort,
                    ADatabaseName,
                    AUsername,
                    ref APassword,
                    AConnectionString,
                    new StateChangeEventHandler(this.OnStateChangedHandler));

                if (FSqlConnection == null)
                {
                    throw new EDBConnectionNotEstablishedException();
                }
            }
            else
            {
                CurrentConnectionInstance = FDBConnectionInstance;
            }

            try
            {
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("    Connecting to " + ADataBaseType + ": " + CurrentConnectionInstance.GetConnectionString());
                }
                else
                {
                    // always log to console which type of database and location/port we are connecting to
                    TLogging.Log(
                        "    Connecting to " + ADataBaseType + ": " + CurrentConnectionInstance.GetConnectionString(), TLoggingType.ToConsole);
                }

                FSqlConnection.Open();

                FLastDBAction = DateTime.Now;
            }
            catch (Exception exp)
            {
                FSqlConnection = null;

                LogException(exp,
                    String.Format("Exception occured while establishing a connection to Database Server. DB Type: {0}", FDbType));

                throw new EDBConnectionNotEstablishedException(CurrentConnectionInstance.GetConnectionString() + ' ' + exp.ToString());
            }
        }

        /// <summary>
        /// Closes the DB connection.
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        public void CloseDBConnection()
        {
            CloseDBConnectionInternal(FDbType);
        }

        /// <summary>
        /// Closes the DB connection.
        /// </summary>
        /// <param name="ADbType">The Type of DB whose Connection should be closed</param>
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        private void CloseDBConnectionInternal(TDBType ADbType)
        {
            if (ConnectionReady())
            {
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("  Closing Database connection...");
                }
#endif

                if (FTransaction != null)
                {
                    /* TODO 1 oChristianK cLogging (Console) : Put the following debug messages in a DEBUGMODE conditional compilation directive and raise the DL to >=DB_DEBUGLEVEL_TRACE; these logging statements were inserted to trace problems
                     *in on live installations! */
                    if (FDebugLevel >= 5)
                    {
                        TLogging.Log("TDataBase.CloseDBConnectionInternal: before calling this.RollbackTransaction",
                            TLoggingType.ToConsole | TLoggingType.ToLogfile);
                    }

                    this.RollbackTransaction();

                    if (FDebugLevel >= 5)
                    {
                        TLogging.Log("TDataBase.CloseDBConnectionInternal: after calling this.RollbackTransaction",
                            TLoggingType.ToConsole | TLoggingType.ToLogfile);
                    }
                }

                /* TODO 1 oChristianK cLogging (Console) : Put the following debug messages in a DEBUGMODE conditional compilation directive and raise the DL to >=DB_DEBUGLEVEL_TRACE; these logging statements were inserted to trace problems in on
                 *live installations! */
                if (FDebugLevel >= 5)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: before calling FDBConnectionInstance.CloseODBCConnection(FConnection) in AppDomain: "
                        +
                        AppDomain.CurrentDomain.ToString(),
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FDBConnectionInstance.CloseDBConnection(FSqlConnection);
                FSqlConnection = null;
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: closed DB Connection.");
                }
#endif

                if (FDebugLevel >= 5)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: after calling FDBConnectionInstance.CloseODBCConnection(FConnection) in AppDomain: "
                        +
                        AppDomain.CurrentDomain.ToString(),
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FLastDBAction = DateTime.Now;
            }
            else
            {
                throw new EDBConnectionNotAvailableException();
            }
        }

        /// <summary>
        /// Call this Method to make the next Command that is sent to the DB
        /// a 'Prepared' command.
        /// </summary>
        /// <remarks><see cref="PrepareNextCommand" /> lets you optimise the performance of
        /// frequently used queries. What a RDBMS basically does with a 'Prepared' SQL Command is
        /// that it 'caches' the query plan so that it's used in subsequent calls.
        /// Not supported by all RDBMS, but should just silently fail in case a RDBMS doesn't
        /// support it. PostgreSQL definitely supports it.</remarks>
        /// <returns>void</returns>
        public void PrepareNextCommand()
        {
            FPrepareNextCommand = true;
        }

        /// <summary>
        /// Call this Method to set a timeout (in seconds) for the next Command that is sent to the
        /// DB that is different from the default timeout for a Command (eg. 20s for a
        /// NpgsqlCommand).
        /// </summary>
        /// <returns>void</returns>
        public void SetTimeoutForNextCommand(int ATimeoutInSec)
        {
            FTimeoutForNextCommand = ATimeoutInSec;
        }

        /// <summary>
        /// Means of getting Cache objects.
        /// </summary>
        /// <returns>A new Instance of an <see cref="TSQLCache" /> Object.</returns>
        public TSQLCache GetCache()
        {
            return new TSQLCache();
        }

        #region Command

        /// <summary>
        /// Returns an IDbCommand for a given command text in the context of a
        /// DB transaction. Not suitable for parameterised SQL statements.
        /// </summary>
        /// <remarks>This function does not execute the Command, it just creates it!</remarks>
        /// <param name="ACommandText">Command Text</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />, or nil if the command
        /// should not be enlisted in a transaction.</param>
        /// <returns>Instantiated IDbCommand
        /// </returns>
        public IDbCommand Command(String ACommandText, TDBTransaction ATransaction)
        {
            return Command(ACommandText, ATransaction, new OdbcParameter[0]);
        }

        /// <summary>
        /// Returns an IDbCommand for a given command text in the context of a
        /// DB transaction. Suitable for parameterised SQL statements.
        /// Allows the passing in of Parameters for the SQL statement
        /// </summary>
        /// <remarks>This function does not execute the Command, it just creates it!</remarks>
        /// <param name="ACommandText">Command Text</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />, or nil if the command
        /// should not be enlisted in a transaction.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameter
        /// (including Parameter Value)</param>
        /// <returns>Instantiated IDbCommand
        /// </returns>
        public IDbCommand Command(String ACommandText, TDBTransaction ATransaction, DbParameter[] AParametersArray)
        {
            IDbCommand ObjReturn = null;

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".Command()...");
            }
#endif

            if (!HasAccess(ACommandText))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            try
            {
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".Command: now getting IDbCommand(" + ACommandText + ")...");
                }
#endif

                ObjReturn = FDataBaseRDBMS.NewCommand(ref ACommandText, FSqlConnection, AParametersArray, ATransaction);

                // enlist this command in a DB transaction (does not happen if ATransaction is null)
                if (ATransaction != null)
                {
                    ObjReturn.Transaction = ATransaction.WrappedTransaction;
                }

                // if this is a call to Stored Procedure: set command type accordingly
                if (ACommandText.ToUpper().StartsWith("CALL"))
                {
                    ObjReturn.CommandType = CommandType.StoredProcedure;
                }

                if (FPrepareNextCommand)
                {
                    ObjReturn.Prepare();
                    FPrepareNextCommand = false;

#if DEBUGMODE
                    if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(this.GetType().FullName + ".Command: will 'Prepare' this Command.");
                    }
#endif
                }

                if (FTimeoutForNextCommand != -1)
                {
                    /*
                     * Tricky bit: we need to create a new Object (of Type String) that is disassociated
                     * with FTimeoutForNextCommand, because FTimeoutForNextCommand is reset in the next statement!
                     */
                    ObjReturn.CommandTimeout = Convert.ToInt32(FTimeoutForNextCommand.ToString());
                    FTimeoutForNextCommand = -1;

#if DEBUGMODE
                    if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(
                            this.GetType().FullName + ".Command: set Timeout for this Command to " + ObjReturn.CommandTimeout.ToString() + ".");
                    }
#endif
                }
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ACommandText, "Error creating Command. The command was: ");

                throw;
            }

            FLastDBAction = DateTime.Now;
            return ObjReturn;
        }

        #endregion

        #region Select

        /// <summary>
        /// Returns a <see cref="DataSet" /> containing a <see cref="DataTable" /> with the result of a given SQL
        /// statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <returns>Instantiated <see cref="DataSet" /></returns>
        public DataSet Select(String ASqlStatement, String ADataTableName, TDBTransaction AReadTransaction)
        {
            return Select(ASqlStatement, ADataTableName, AReadTransaction, new OdbcParameter[0]);
        }

        /// <summary>
        /// Returns a <see cref="DataSet" /> containing a <see cref="DataTable" /> with the result of a given SQL
        /// statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="DataSet" /></returns>
        public DataSet Select(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray)
        {
            DataSet InputDataSet;
            DataSet ObjReturn;

            ObjReturn = null;
            InputDataSet = new DataSet();
            ObjReturn = Select(InputDataSet, ASqlStatement, ADataTableName, AReadTransaction, AParametersArray);
            InputDataSet.Dispose();
            return ObjReturn;
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a  given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <param name="AMaxRecords">Maximum number of records that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            System.Int32 AStartRecord,
            System.Int32 AMaxRecords)
        {
            return Select(AFillDataSet, ASqlStatement, ADataTableName, AReadTransaction, new OdbcParameter[0], AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a  given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            System.Int32 AStartRecord)
        {
            return Select(AFillDataSet, ASqlStatement, ADataTableName, AReadTransaction, AStartRecord, 0);
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a  given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" />.</param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" />.</returns>
        public DataSet Select(DataSet AFillDataSet, String ASqlStatement, String ADataTableName, TDBTransaction AReadTransaction)
        {
            return Select(AFillDataSet, ASqlStatement, ADataTableName, AReadTransaction, 0, 0);
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <param name="AMaxRecords">Maximum number of records that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray,
            System.Int32 AStartRecord,
            System.Int32 AMaxRecords)
        {
            DataSet ObjReturn;

            if (AFillDataSet == null)
            {
                throw new ArgumentNullException("AFillDataSet", "AFillDataSet must not be null!");
            }

            if (ADataTableName == "")
            {
                throw new ArgumentException("ADataTableName", "A name for the DataTable must be submitted!");
            }

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".Select()...");
            }
            LogSqlStatement(this.GetType().FullName + ".Select()", ASqlStatement, AParametersArray);
#endif

            ObjReturn = null;

            try
            {
                IDbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, AParametersArray);
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: now filling IDbDataAdapter('" + ADataTableName) + "')..."));
                }
#endif

                FDataBaseRDBMS.FillAdapter(TheAdapter, ref AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: finished filling IDbDataAdapter(DataTable '" +
                                   ADataTableName) + "'). DT Row Count: " + AFillDataSet.Tables[ADataTableName].Rows.Count.ToString()));
#if WITH_POSTGRESQL_LOGGING
                    NpgsqlEventLog.Level = LogLevel.None;
#endif
                }
#endif
                ObjReturn = AFillDataSet;
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ASqlStatement, "Error fetching records.");
            }

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                if ((ObjReturn != null) && (ObjReturn.Tables[ADataTableName] != null))
                {
                    LogTable(ObjReturn.Tables[ADataTableName]);
                }
            }
#endif
            return ObjReturn;
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a  given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated OdbcParameters
        /// (including parameter Value)</param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            OdbcParameter[] AParametersArray,
            System.Int32 AStartRecord)
        {
            return Select(AFillDataSet, ASqlStatement, ADataTableName, AReadTransaction, AParametersArray, AStartRecord, 0);
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a  given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray)
        {
            return Select(AFillDataSet, ASqlStatement, ADataTableName, AReadTransaction, AParametersArray, 0, 0);
        }

        #endregion

        #region SelectDA

        /// <summary>
        /// Returns an <see cref="IDbDataAdapter" /> (eg. <see cref="OdbcDataAdapter" />, NpgsqlDataAdapter) for a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <returns>Instantiated <see cref="IDbDataAdapter" /></returns>
        public IDbDataAdapter SelectDA(String ASqlStatement, TDBTransaction AReadTransaction)
        {
            return SelectDA(ASqlStatement, AReadTransaction, new OdbcParameter[0]);
#if WITH_POSTGRESQL_LOGGING
#if DEBUGMODE
            if (FDebugLevel >= DB_DEBUGLEVEL_TRACE)
            {
                NpgsqlEventLog.Level = LogLevel.None;
            }
#endif
#endif
        }

        /// <summary>
        /// Returns an <see cref="IDbDataAdapter" /> (eg. <see cref="OdbcDataAdapter" />, NpgsqlDataAdapter) for a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="IDbDataAdapter" />
        /// </returns>
        public IDbDataAdapter SelectDA(String ASqlStatement, TDBTransaction AReadTransaction, DbParameter[] AParametersArray)
        {
#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".SelectDA()...");
            }
#endif

            if (!HasAccess(ASqlStatement))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log(this.GetType().FullName + ".SelectDA: now opening IDbDataAdapter(" + ASqlStatement + ")...");
            }
#endif

            IDbDataAdapter TheAdapter = FDataBaseRDBMS.NewAdapter();
            TheAdapter.SelectCommand = FDataBaseRDBMS.NewCommand(ref ASqlStatement, FSqlConnection, AParametersArray, AReadTransaction);
            return TheAdapter;
        }

        #endregion

        #region SelectDT

        /// <summary>
        /// Returns a <see cref="DataTable" /> filled with the result of a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL
        /// statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <returns>Instantiated DataTable</returns>
        public DataTable SelectDT(String ASqlStatement, String ADataTableName, TDBTransaction AReadTransaction)
        {
            return SelectDTInternal(ASqlStatement, ADataTableName, AReadTransaction, new OdbcParameter[0]);
        }

        /// <summary>
        /// Returns a <see cref="DataTable" /> filled with the result of a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL
        /// statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="DataTable" /></returns>
        public DataTable SelectDT(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray)
        {
            return SelectDTInternal(ASqlStatement, ADataTableName,
                AReadTransaction, AParametersArray);
        }

        /// <summary>
        /// Returns a <see cref="DataTable" /> filled with the result of a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="DataTable" /></returns>
        private DataTable SelectDTInternal(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray)
        {
#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".SelectDTInternal()...");
            }
            LogSqlStatement(this.GetType().FullName + ".SelectDTInternal()", ASqlStatement, AParametersArray);
#endif

            if (!HasAccess(ASqlStatement))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            DataTable ObjReturn = new DataTable(ADataTableName);
            try
            {
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".SelectDTInternal: now opening IDbDataAdapter(" + ASqlStatement + ")...");
                }
#endif

                IDbDataAdapter TheAdapter = FDataBaseRDBMS.NewAdapter();
                TheAdapter.SelectCommand = Command(ASqlStatement, AReadTransaction, AParametersArray);
                FDataBaseRDBMS.FillAdapter(TheAdapter, ref ObjReturn, 0, 0);

#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".SelectDTInternal: finished filling IDbDataAdapter(DataTable " +
                                   ADataTableName) + "). DT Row Count: " + ObjReturn.Rows.Count.ToString()));
                }
#endif
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ASqlStatement, "Error fetching records.");
            }

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                LogTable(ObjReturn);
            }
#endif
            return ObjReturn;
        }

        /// <summary>
        /// this loads the result into a typed datatable
        /// </summary>
        /// <param name="ATypedDataTable">this needs to be an object of the typed datatable</param>
        /// <param name="ASqlStatement"></param>
        /// <param name="AReadTransaction"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="AStartRecord">does not have any effect yet</param>
        /// <param name="AMaxRecords">not implemented yet</param>
        /// <returns></returns>
        public DataTable SelectDT(DataTable ATypedDataTable, String ASqlStatement,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray,
            int AStartRecord, int AMaxRecords)
        {
            if (!HasAccess(ASqlStatement))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            try
            {
                IDbDataAdapter TheAdapter = FDataBaseRDBMS.NewAdapter();
                TheAdapter.SelectCommand = Command(ASqlStatement, AReadTransaction, AParametersArray);
                FDataBaseRDBMS.FillAdapter(TheAdapter, ref ATypedDataTable, AStartRecord, AMaxRecords);
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ASqlStatement, "Error fetching records.");
            }

            return ATypedDataTable;
        }

        #endregion


        #region Transactions

        /// <summary>
        /// Starts a Transaction on the current DB connection.
        /// Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified.
        /// This is to be able to migtigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <returns>Started Transaction (null if an error occured)
        /// </returns>
        public TDBTransaction BeginTransaction(Int16 ARetryAfterXSecWhenUnsuccessful)
        {
            TDBTransaction ReturnValue;

            try
            {
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "Trying to open a DB Transaction... (in Appdomain " +
                        AppDomain.CurrentDomain.ToString() + " ).");
                }
#endif
                FTransaction = FSqlConnection.BeginTransaction();

#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                {
                    TLogging.Log("DB Transaction started (in Appdomain " + AppDomain.CurrentDomain.ToString() + " ).");
                }
#endif
            }
            catch (System.InvalidOperationException exp)
            {
                // System.InvalidOperationException is thrown when a transaction is currently active. Parallel transactions are not supported!
                // Retry again if programmer wants that
                if (ARetryAfterXSecWhenUnsuccessful != -1)
                {
                    Thread.Sleep(ARetryAfterXSecWhenUnsuccessful * 1000);

                    /*
                     * Retry again to begin a transaction.
                     * Note: if this fails again, an Exception is thrown as if there was
                     * no ARetryAfterXSecWhenUnsuccessful specfied!
                     */
                    ReturnValue = BeginTransaction(-1);
                    return ReturnValue;
                }
                else
                {
                    throw new EDBTransactionBusyException("", exp);
                }
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, "Error creating Transaction - Server-side error.");
            }

            FLastDBAction = DateTime.Now;
            return new TDBTransaction(FTransaction, FSqlConnection);
        }

        /// <summary>
        /// Starts a Transaction on the current DB connection.
        /// </summary>
        /// <returns>Started Transaction (null if an error occured)</returns>
        public TDBTransaction BeginTransaction()
        {
            return BeginTransaction(-1);
        }

        /// <summary>
        /// Starts a Transaction with a defined <see cref="IsolationLevel" /> on the current DB
        /// connection. Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /></param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified.
        /// This is to be able to migtigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <returns>Started Transaction (null if an error occured)</returns>
        public TDBTransaction BeginTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful)
        {
            TDBTransaction ReturnValue;

            FDataBaseRDBMS.AdjustIsolationLevel(ref AIsolationLevel);

            try
            {
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "Trying to open an DB Transaction with IsolationLevel '" + AIsolationLevel.ToString() +
                        "... (in Appdomain " +
                        AppDomain.CurrentDomain.ToString() + " ).");
                }
#endif
                FTransaction = FSqlConnection.BeginTransaction(AIsolationLevel);

#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                {
                    TLogging.Log(
                        "DB Transaction with IsolationLevel '" + AIsolationLevel.ToString() + "' started (in Appdomain " +
                        AppDomain.CurrentDomain.ToString() + " ).");
                }
#endif
            }
            catch (System.InvalidOperationException exp)
            {
                // System.InvalidOperationException is thrown when a transaction is currently active. Parallel transactions are not supported!
                // Retry again if programmer wants that
                if (ARetryAfterXSecWhenUnsuccessful != -1)
                {
                    Thread.Sleep(ARetryAfterXSecWhenUnsuccessful * 1000);

                    /*
                     * Retry again to begin a transaction.
                     * Note: if this fails again, an Exception is thrown as if there was
                     * no ARetryAfterXSecWhenUnsuccessful specfied!
                     */
                    ReturnValue = BeginTransaction(AIsolationLevel, -1);
                    return ReturnValue;
                }
                else
                {
                    throw new EDBTransactionBusyException("IsolationLevel: " + Enum.GetName(typeof(IsolationLevel), AIsolationLevel), exp);
                }
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, "Error creating Transaction - Server-side error.");
            }

            FLastDBAction = DateTime.Now;
            return new TDBTransaction(FTransaction, FSqlConnection);
        }

        /// <summary>
        /// Starts a Transaction with a defined <see cref="IsolationLevel" /> on the current DB
        /// connection.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /></param>
        /// <returns>Started Transaction (null if an error occured)</returns>
        public TDBTransaction BeginTransaction(IsolationLevel AIsolationLevel)
        {
            return BeginTransaction(AIsolationLevel, -1);
        }

        /// <summary>
        /// Commits a running Transaction on the current DB connection.
        /// </summary>
        /// <returns>void</returns>
        public void CommitTransaction()
        {
            if (FTransaction != null)
            {
#if DEBUGMODE
                String msg = "";
#endif
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                {
                    msg = "DB Transaction with IsolationLevel '" + FTransaction.IsolationLevel.ToString() + "' committed (in Appdomain " +
                          AppDomain.CurrentDomain.ToString() + " ).";
                }
#endif

                FTransaction.Commit();

#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                {
                    TLogging.Log(msg);
                }
#endif
            }

            FTransaction = null;
        }

        /// <summary>
        /// Rolls back a running Transaction on the current DB connection.
        /// </summary>
        /// <returns>void</returns>
        public void RollbackTransaction()
        {
#if DEBUGMODE
            String msg = "";
#endif

            if (FTransaction == null)
            {
                return;
            }

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                msg = "DB Transaction with IsolationLevel '" + FTransaction.IsolationLevel.ToString() + "' rolled back (in Appdomain " +
                      AppDomain.CurrentDomain.ToString() + " ).";
            }
#endif

            FTransaction.Rollback();

#if DEBUGMODE
            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                TLogging.Log(msg);
            }
#endif
            FTransaction = null;
        }

        /// <summary>
        /// Either starts a new Transaction on the current DB connection or returns
        /// a existing <see cref="TDBTransaction" />. What it does depends on two factors: whether a Transaction
        /// is currently running or not, and if so, whether it meets the specified
        /// <paramref name="ADesiredIsolationLevel" />.
        /// <para>If there is a current Transaction but it has a different <see cref="IsolationLevel" />,
        /// <see cref="EDBTransactionIsolationLevelWrongException" />
        /// is thrown.</para>
        /// <para>If there is no current Transaction, a new Transaction with the specified <see cref="IsolationLevel" />
        /// is started.</para>
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired</param>
        /// <param name="ANewTransaction">True if a new Transaction was started and is returned,
        /// false if an already existing Transaction is returned</param>
        /// <returns>Either an existing or a new Transaction that exactly meets the specified <see cref="IsolationLevel" /></returns>
        public TDBTransaction GetNewOrExistingTransaction(IsolationLevel ADesiredIsolationLevel, out Boolean ANewTransaction)
        {
            return GetNewOrExistingTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact, out ANewTransaction);
        }

        /// <summary>
        /// Either starts a new Transaction on the current DB connection or returns
        /// a existing <see cref="TDBTransaction" />. What it does depends on two factors: whether a Transaction
        /// is currently running or not, and if so, whether it meets the specified
        /// <paramref name="ADesiredIsolationLevel" />.
        /// <para>If there is a current Transaction but it has a different <see cref="IsolationLevel" />, the result
        /// depends on the value of the <paramref name="ATryToEnforceIsolationLevel" />
        /// parameter.</para>
        /// <para>If there is no current Transaction, a new Transaction with the specified <see cref="IsolationLevel" />
        /// is started.</para>
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ANewTransaction">True if a new Transaction was started and is returned,
        /// false if an already existing Transaction is returned</param>
        /// <returns>Either an existing or a new Transaction that exactly meets the specified <see cref="IsolationLevel" /></returns>
        /// <exception cref="EDBTransactionIsolationLevelWrongException">Thrown if the
        /// <paramref name="ATryToEnforceIsolationLevel" /> Argument is set to
        /// <see cref="TEnforceIsolationLevel.eilExact" /> and the existing Transactions' <see cref="IsolationLevel" /> does not
        /// exactly match the <see cref="IsolationLevel" /> specified with Argument  <paramref name="ADesiredIsolationLevel" />,
        /// <see cref="EDBTransactionIsolationLevelWrongException" /></exception>
        /// <exception cref="EDBTransactionIsolationLevelTooLowException">Thrown if the
        /// <paramref name="ATryToEnforceIsolationLevel" /> Argument is set to
        /// <see cref="TEnforceIsolationLevel.eilExact" /> and the existing Transactions' <see cref="IsolationLevel" /> does not
        /// exactly match the <see cref="IsolationLevel" /> specified with Argument  <paramref name="ADesiredIsolationLevel" />,
        /// <see cref="EDBTransactionIsolationLevelWrongException" />Thrown if the
        /// <paramref name="ATryToEnforceIsolationLevel" /> Argument is set to
        /// <see cref="TEnforceIsolationLevel.eilMinimum" /> and the existing Transactions' <see cref="IsolationLevel" />
        /// hasn't got at least the <see cref="IsolationLevel" /> specified with Argument
        /// <paramref name="ADesiredIsolationLevel" />.</exception>
        public TDBTransaction GetNewOrExistingTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel,
            out Boolean ANewTransaction)
        {
            TDBTransaction TheTransaction;

            ANewTransaction = false;
            TheTransaction = this.Transaction;

            FDataBaseRDBMS.AdjustIsolationLevel(ref ADesiredIsolationLevel);

            if (TheTransaction != null)
            {
                // Check if the IsolationLevel of the existing Transaction is acceptable
                if ((ATryToEnforceIsolationLevel == TEnforceIsolationLevel.eilExact)
                    && (TheTransaction.IsolationLevel != ADesiredIsolationLevel)
                    || ((ATryToEnforceIsolationLevel == TEnforceIsolationLevel.eilMinimum)
                        && (TheTransaction.IsolationLevel < ADesiredIsolationLevel)))
                {
                    switch (ATryToEnforceIsolationLevel)
                    {
                        case TEnforceIsolationLevel.eilExact:
                            throw new EDBTransactionIsolationLevelWrongException("Expected IsolationLevel: " +
                            ADesiredIsolationLevel.ToString("G"));

                        case TEnforceIsolationLevel.eilMinimum:
                            throw new EDBTransactionIsolationLevelTooLowException(
                            "Expected IsolationLevel: at least " + ADesiredIsolationLevel.ToString("G"));
                    }
                }
            }

            if (TheTransaction == null)
            {
#if DEBUGMODE
                if (FDebugLevel >= 7)
                {
                    Console.WriteLine("GetNewOrExistingTransaction: creating new transaction. IsolationLevel: " + ADesiredIsolationLevel.ToString());
                }
#endif
                TheTransaction = BeginTransaction(ADesiredIsolationLevel);
                ANewTransaction = true;
            }
            else
            {
#if DEBUGMODE
                if (FDebugLevel >= 7)
                {
                    Console.WriteLine(
                        "GetNewOrExistingTransaction: using existing transaction. IsolationLevel: " + TheTransaction.IsolationLevel.ToString());
                }
#endif
            }

            return TheTransaction;
        }

        #endregion

        #region GetNextSequenceValue

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction)
        {
            return FDataBaseRDBMS.GetNextSequenceValue(ASequenceName, ATransaction, this, FSqlConnection);
        }

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction)
        {
            return FDataBaseRDBMS.GetCurrentSequenceValue(ASequenceName, ATransaction, this, FSqlConnection);
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction; the transaction is
        /// automatically committed. Not suitable for parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.
        /// The transaction is automatically committed! Can be null.
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQuery(String ASqlStatement, TDBTransaction ATransaction)
        {
            if (ATransaction != null)
            {
                ExecuteNonQuery(ASqlStatement, ATransaction, true);
            }
            else
            {
                ExecuteNonQuery(ASqlStatement, ATransaction, false);
            }
        }

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction; the transaction is
        /// automatically committed. Suitable for parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.
        /// The transaction is automatically committed!</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQuery(String ASqlStatement, TDBTransaction ATransaction, DbParameter[] AParametersArray)
        {
            ExecuteNonQuery(ASqlStatement, ATransaction, true, AParametersArray);
        }

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction. Not suitable for
        /// parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQuery(String ASqlStatement, TDBTransaction ATransaction, bool ACommitTransaction)
        {
            ExecuteNonQuery(ASqlStatement, ATransaction, ACommitTransaction, new OdbcParameter[0]);
        }

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction. Suitable for
        /// parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQuery(String ASqlStatement, TDBTransaction ATransaction, bool ACommitTransaction, DbParameter[] AParametersArray)
        {
            IDbCommand TransactionCommand = null;

            if ((ATransaction == null) && (ACommitTransaction == true))
            {
                throw new ArgumentNullException("ACommitTransaction", "ACommitTransaction cannot be set to true when ATransaction is null!");
            }

#if DEBUGMODE
            LogSqlStatement(this.GetType().FullName + ".ExecuteNonQuery()", ASqlStatement, AParametersArray);
#endif

            if (ConnectionReady())
            {
                TransactionCommand = Command(ASqlStatement, ATransaction, AParametersArray);

                if (TransactionCommand == null)
                {
                    // should never get here
                    return;
                }

                try
                {
                    TransactionCommand.ExecuteNonQuery();

                    if (ACommitTransaction)
                    {
                        CommitTransaction();
                    }
                }
                catch (Exception exp)
                {
                    LogExceptionAndThrow(exp, ASqlStatement, "Error executing non-query SQL statement.");
                }
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. The transaction's <see cref="IsolationLevel" />
        /// will be <see cref="IsolationLevel.ReadCommitted" />.
        /// Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable)
        {
            TDBTransaction EnclosingTransaction;

            if (ConnectionReady())
            {
                EnclosingTransaction = BeginTransaction(IsolationLevel.ReadCommitted);
                ExecuteNonQueryBatch(AStatementHashTable, EnclosingTransaction);
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. A Transaction with the desired
        /// <see cref="IsolationLevel" /> is automatically created and committed/rolled back.
        /// Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />  of the transaction
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, IsolationLevel AIsolationLevel)
        {
            TDBTransaction EnclosingTransaction;

            if (ConnectionReady())
            {
                EnclosingTransaction = BeginTransaction(AIsolationLevel);
                ExecuteNonQueryBatch(AStatementHashTable, EnclosingTransaction, true);
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. The transaction is automatically
        /// committed if all statements could be executed without error. Suitable for
        /// parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, TDBTransaction ATransaction)
        {
            ExecuteNonQueryBatch(AStatementHashTable, ATransaction, true);
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. Suitable for parameterised SQL
        /// statements.
        ///
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">On successful execution of all statements the
        /// transaction is committed if set to true, otherwise the transaction is not
        /// committed (useful when the caller wants to do further things in the same
        /// transaction).
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, TDBTransaction ATransaction, bool ACommitTransaction)
        {
            int SqlCommandNumber;
            String CurrentBatchEntryKey = "";
            String CurrentBatchEntrySQLStatement = "";
            IDictionaryEnumerator BatchStatementEntryIterator;
            TSQLBatchStatementEntry BatchStatementEntryValue;

            DbParameter[] ParametersArray;

            if (AStatementHashTable == null)
            {
                throw new ArgumentNullException("AStatementHashTable", "This method must be called with an initialized HashTable!!");
            }

            if (AStatementHashTable.Count == 0)
            {
                throw new ArgumentException("AStatementHashTable", "ArrayList containing TSQLBatchStatementEntry objects must not be empty!");
            }

            if (ATransaction == null)
            {
                throw new ArgumentNullException("ATransaction", "This method must be called with an initialized transaction!");
            }

            if (ConnectionReady())
            {
                // TransactionCommand := nil;
                SqlCommandNumber = 0;
                try
                {
                    BatchStatementEntryIterator = AStatementHashTable.GetEnumerator();

                    while (BatchStatementEntryIterator.MoveNext())
                    {
                        BatchStatementEntryValue = (TSQLBatchStatementEntry)BatchStatementEntryIterator.Value;
                        BatchStatementEntryValue.GetWholeParameterArray(out ParametersArray);
                        CurrentBatchEntryKey = BatchStatementEntryIterator.Key.ToString();
                        CurrentBatchEntrySQLStatement = BatchStatementEntryValue.SQLStatement;
                        ExecuteNonQuery(CurrentBatchEntrySQLStatement, ATransaction, false, ParametersArray);
                        SqlCommandNumber = SqlCommandNumber + 1;
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction();
                    }
                }
                catch (Exception exp)
                {
                    RollbackTransaction();

                    LogException(exp, CurrentBatchEntrySQLStatement,
                        "Exception occured while executing AStatementHashTable entry '" +
                        CurrentBatchEntryKey + "' (#" + SqlCommandNumber.ToString() +
                        ")! (The SQL Statement is a non-query SQL statement.)  All SQL statements executed so far were rolled back.");

                    throw new EDBExecuteNonQueryBatchException(
                        "Exception occured while executing AStatementHashTable entry '" + CurrentBatchEntryKey + "' (#" +
                        SqlCommandNumber.ToString() + ")! Non-query SQL statement: [" + CurrentBatchEntrySQLStatement +
                        "]). All SQL statements executed so far were rolled back.",
                        exp);
                }
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction with the desired <see cref="IsolationLevel" /> and
        /// the transaction is automatically committed. Not suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /> of the transaction</param>
        /// <returns>Single result as object
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, IsolationLevel AIsolationLevel)
        {
            return ExecuteScalar(ASqlStatement, AIsolationLevel, new OdbcParameter[0]);
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction with the desired <see cref="IsolationLevel" /> and
        /// the transaction is automatically committed. Suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /> of the transaction</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Single result as object
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, IsolationLevel AIsolationLevel, DbParameter[] AParametersArray)
        {
            object ReturnValue = null;
            TDBTransaction EnclosingTransaction;

            if (ConnectionReady())
            {
                EnclosingTransaction = BeginTransaction(AIsolationLevel);

                try
                {
                    ReturnValue = ExecuteScalar(ASqlStatement, EnclosingTransaction, true, AParametersArray);
                }
                catch (Exception)
                {
                    // Exception logging occurs already  inside ExecuteScalar, so we don't need to do it here!
                    throw;
                }
                finally
                {
                    CommitTransaction();
                }
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction; the transaction is automatically committed. Not suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <returns>Single result as object
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, TDBTransaction ATransaction)
        {
            return ExecuteScalar(ASqlStatement, ATransaction, new OdbcParameter[0]);
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction; the transaction is automatically committed. Suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Single result as object
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, TDBTransaction ATransaction, DbParameter[] AParametersArray)
        {
            return ExecuteScalar(ASqlStatement, ATransaction, true, AParametersArray);
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction. Not suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <returns>Single result as TObject
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, TDBTransaction ATransaction, bool ACommitTransaction)
        {
            return ExecuteScalar(ASqlStatement, ATransaction, ACommitTransaction, new OdbcParameter[0]);
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction. Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Single result as TObject
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, TDBTransaction ATransaction, bool ACommitTransaction, DbParameter[] AParametersArray)
        {
            object ReturnValue = null;
            IDbCommand TransactionCommand = null;

            if ((ATransaction == null) && (ACommitTransaction == true))
            {
                throw new ArgumentNullException("ACommitTransaction", "ACommitTransaction cannot be set to true when ATransaction is null!");
            }

#if DEBUGMODE
            LogSqlStatement(this.GetType().FullName + ".ExecuteScalar()", ASqlStatement, AParametersArray);
#endif

            if (ConnectionReady())
            {
#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".ExecuteScalar: now opening Command(" + ASqlStatement + ")...");
                }
#endif
                TransactionCommand = Command(ASqlStatement, ATransaction, AParametersArray);

                if (TransactionCommand == null)
                {
                    // should never get here
                    return null;
                }

                try
                {
#if DEBUGMODE
                    if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(this.GetType().FullName + ".ExecuteScalar: now calling Command.ExecuteScalar...");
                    }
#endif

                    ReturnValue = TransactionCommand.ExecuteScalar();

                    if (ReturnValue == null)
                    {
                        throw new Exception("Execute Scalar returned no value");
                    }

#if DEBUGMODE
                    if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(this.GetType().FullName + ".ExecuteScalar: finished calling Command.ExecuteScalar");
                    }
#endif

                    if (ACommitTransaction)
                    {
                        CommitTransaction();
                    }
                }
                catch (Exception exp)
                {
                    LogExceptionAndThrow(exp, ASqlStatement, "Error executing scalar SQL statement.");
                }

#if DEBUGMODE
                if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_RESULT)
                {
                    TLogging.Log("Result from ExecuteScalar is " + ReturnValue.ToString() + " " + ReturnValue.GetType().ToString());
                }
#endif
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }

            return ReturnValue;
        }

        #endregion

        /// <summary>
        /// read an sql statement from file and remove the comments
        /// </summary>
        /// <param name="ASqlFilename"></param>
        /// <returns></returns>
        public static string ReadSqlFile(string ASqlFilename)
        {
            ASqlFilename = TAppSettingsManager.GetValueStatic("SqlFiles.Path", ".") +
                           Path.DirectorySeparatorChar +
                           ASqlFilename;

            // Console.WriteLine("reading " + ASqlFilename);
            StreamReader reader = new StreamReader(ASqlFilename);
            string line = null;
            string stmt = "";

            if (reader == null)
            {
                throw new Exception("cannot open file " + ASqlFilename);
            }

            while ((line = reader.ReadLine()) != null)
            {
                if (!line.Trim().StartsWith("--"))
                {
                    stmt += line.Trim() + " ";
                }
            }

            reader.Close();
            return stmt;
        }

        private bool FConnectionReady = false;

        /// <summary>
        /// Tells whether the DB connection is ready to accept commands
        /// or whether it is busy.
        /// </summary>
        /// <returns>True if DB connection can accept commands, false if
        /// it is busy</returns>
        private bool ConnectionReady()
        {
            if (FDbType == TDBType.PostgreSQL)
            {
                // TODO: change when OnStateChangedHandler works for postgresql
                return FSqlConnection != null && FSqlConnection.State == ConnectionState.Open;
            }

            return FConnectionReady;
        }

        /// <summary>
        /// Updates the FConnectionReady variable with the current ConnectionState.
        /// </summary>
        /// <remarks>
        /// <em>WARNING:</em> This doesn't work with NpgsqlConnection because it never raises the
        /// Event. Therefore the FConnectionReady variable must
        /// never be inquired directly, but only through calling ConnectionReady()!
        /// TODO: revise this comment with more recent Npgsql release
        /// </remarks>
        /// <param name="ASender">Sending object</param>
        /// <param name="AArgs">StateChange EventArgs</param>
        private void OnStateChangedHandler(object ASender, StateChangeEventArgs AArgs)
        {
            switch (AArgs.CurrentState)
            {
                case ConnectionState.Open:
                case ConnectionState.Fetching:
                case ConnectionState.Executing:
                    FConnectionReady = true;
                    break;

                case ConnectionState.Closed:
                case ConnectionState.Connecting:
                case ConnectionState.Broken:
                    FConnectionReady = false;
                    break;

                default:
                    FConnectionReady = false;
                    break;
            }
        }

        /// <summary>
        /// for debugging, export data table to xml (which can be saved as xml, yml, csv)
        /// </summary>
        /// <param name="ATable"></param>
        /// <returns></returns>
        public static XmlDocument DataTableToXml(DataTable ATable)
        {
            XmlDocument doc = TYml2Xml.CreateXmlDocument();

            foreach (DataRow row in ATable.Rows)
            {
                XmlElement node = doc.CreateElement(TYml2Xml.XMLELEMENT);

                foreach (DataColumn column in ATable.Columns)
                {
                    node.SetAttribute(column.ColumnName, row[column].ToString());
                }

                doc.DocumentElement.AppendChild(node);
            }

            return doc;
        }

        /// <summary>
        /// For debugging purposes only.
        /// Logs the contents of a DataTable
        /// </summary>
        /// <param name="tab">The DataTable whose contents should be logged
        /// </param>
        /// <returns>void</returns>
        public static void LogTable(DataTable tab)
        {
            String line;

            line = "";

            foreach (DataColumn column in tab.Columns)
            {
                line = line + ' ' + column.ColumnName;
            }

            TLogging.Log(line);

            foreach (DataRow row in tab.Rows)
            {
                line = "";

                foreach (DataColumn column in tab.Columns)
                {
                    line = line + ' ' + row[column].ToString();
                }

                TLogging.Log(line);
            }
        }

        /// <summary>
        /// For debugging purposes.
        /// Formats the sql query so that it is easily readable
        /// (mainly inserting line breaks before AND)
        ///
        /// </summary>
        /// <param name="s">the sql statement that should be formatted</param>
        /// <returns>s the formatted sql statement
        /// </returns>
        public static string FormatSQLStatement(string s)
        {
            string ReturnValue;
            char char13 = (char)13;
            char char10 = (char)10;

            ReturnValue = s;

            ReturnValue = ReturnValue.Replace(char13, ' ').Replace(char10, ' ');
            ReturnValue = ReturnValue.Replace(Environment.NewLine, " ");
            ReturnValue = ReturnValue.Replace(" FROM ", Environment.NewLine + "FROM ");
            ReturnValue = ReturnValue.Replace(" WHERE ", Environment.NewLine + "WHERE ");
            ReturnValue = ReturnValue.Replace(" UNION ", Environment.NewLine + "UNION ");
            ReturnValue = ReturnValue.Replace(" AND ", Environment.NewLine + "AND ");
            ReturnValue = ReturnValue.Replace(" OR ", Environment.NewLine + "OR ");
            ReturnValue = ReturnValue.Replace(" GROUP BY ", Environment.NewLine + "GROUP BY ");
            ReturnValue = ReturnValue.Replace(" ORDER BY ", Environment.NewLine + "ORDER BY ");
            return ReturnValue;
        }

        /// <summary>
        /// This Method checks if the current user has enough access rights to execute the query
        /// passed in in Argument <paramref name="ASQLStatement" />.
        /// <para>This Method needs to be implemented by a derived Class, that knows about the
        /// users' access rights. The implementation here simply returns true...</para>
        /// </summary>
        /// <returns>True if the user has access, false if access is denied.
        /// The implementation here simply returns true, though!
        /// </returns>
        public virtual bool HasAccess(String ASQLStatement)
        {
            return true;
        }

#if DEBUGMODE
        /// <summary>
        /// Logs the SQL statement and the parameters; should only be called in Debugmode;
        /// use DebugLevel to define behaviour.
        /// </summary>
        /// <param name="ASqlStatement">SQL Statement that should be logged.</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement. Can be null.</param>
        /// <returns>void</returns>
        private void LogSqlStatement(String ASqlStatement, DbParameter[] AParametersArray)
        {
            LogSqlStatement("", ASqlStatement, AParametersArray);
        }

        /// <summary>
        /// Logs the SQL statement and the parameters; should only be called in Debugmode;
        /// use DebugLevel to define behaviour.
        /// </summary>
        /// <param name="AContext">Context in which the logging takes place (eg. Method name).</param>
        /// <param name="ASqlStatement">SQL Statement that should be logged.</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement. Can be null.</param>
        /// <returns>void</returns>
        private void LogSqlStatement(String AContext, String ASqlStatement, DbParameter[] AParametersArray)
        {
            String PrintContext = "";

            if (AContext != String.Empty)
            {
                PrintContext = "(Context '" + AContext + "')" + Environment.NewLine;
            }

            if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                TLogging.Log(PrintContext +
                    "The SQL query is: " + Environment.NewLine + FormatSQLStatement(ASqlStatement));
            }

            if ((FDebugLevel >= DBAccess.DB_DEBUGLEVEL_RESULT)
                && (AParametersArray != null))
            {
                Int32 Counter = 1;

                foreach (OdbcParameter Parameter in AParametersArray)
                {
                    if (Parameter.Value == System.DBNull.Value)
                    {
                        TLogging.Log(
                            "Parameter: " + Counter.ToString() + " DBNull" + ' ' + Parameter.Value.GetType().ToString() + ' ' +
                            Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType));
                    }
                    else
                    {
                        TLogging.Log(
                            "Parameter: " + Counter.ToString() + ' ' + Parameter.Value.ToString() + ' ' + Parameter.Value.GetType().ToString() +
                            ' ' +
                            Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString());
                    }

                    Counter = Counter + 1;
                }
            }
        }
#endif



        /// <summary>
        /// Logs an Exception and re-throws it afterwards.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogExceptionAndThrow(Exception AException, string AContext)
        {
            LogException(AException, "", AContext, true);
        }

        /// <summary>
        /// Logs an Exception and re-throws it afterwards.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="ASqlStatement">SQL Statement that caused the Exception (will be logged).</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogExceptionAndThrow(Exception AException, string ASqlStatement, string AContext)
        {
            LogException(AException, ASqlStatement, AContext, true);
        }

        /// <summary>
        /// Logs an Exception.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogException(Exception AException, string AContext)
        {
            LogException(AException, "", AContext, false);
        }

        /// <summary>
        /// Logs an Exception.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="ASqlStatement">SQL Statement that caused the Exception (will be logged).</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogException(Exception AException, string ASqlStatement, string AContext)
        {
            LogException(AException, ASqlStatement, AContext, false);
        }

        /// <summary>
        /// Logs an Exception.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="ASqlStatement">SQL Statement that caused the Exception (will be logged).</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        /// <param name="AThrowExceptionAfterLogging">If set to true, the Exception that is passed in in Argument
        /// <paramref name="AException" /> will be re-thrown.</param>
        /// <exception cref="Exception">Re-throws the Exception that is passed in in Argument
        /// <paramref name="AException" /> if <paramref name="AThrowExceptionAfterLogging" /> is set to true.</exception>
        private void LogException(Exception AException, string ASqlStatement, string AContext, bool AThrowExceptionAfterLogging)
        {
#if DEBUGMODE
            string ErrorMessage = "";
            string FormattedSqlStatement = "";

            if (ASqlStatement != String.Empty)
            {
                ASqlStatement = FDataBaseRDBMS.FormatQueryRDBMSSpecific(ASqlStatement);

                FormattedSqlStatement = "The SQL Statement was: " + Environment.NewLine +
                                        ASqlStatement + Environment.NewLine;
            }

            FDataBaseRDBMS.LogException(AException, ref ErrorMessage);

            TLogging.Log(AContext + Environment.NewLine +
                FormattedSqlStatement +
                "Possible cause: " + AException.ToString() + Environment.NewLine + ErrorMessage);

            TLogging.SafeLogStackTrace(TLoggingType.ToLogfile, FDebugLevel);

            if (AThrowExceptionAfterLogging)
            {
                throw AException;
            }
#endif
        }
    }

    #region TSQLBatchStatementEntry

    /// <summary>
    /// Represents the Value of an entry in a HashTable for use in calls to one of the
    /// <c>ExecuteNonQueryBatch</c> Methods.
    /// </summary>
    /// <remarks>Once instantiated, Batch Statment Entry values can
    /// only be read!</remarks>
    public class TSQLBatchStatementEntry : object
    {
        /// <summary>Holds the SQL Statement for one Batch Statement Entry</summary>
        private string FSQLStatement;

        /// <summary>Holds the Parameters for a Batch Entry (optional)</summary>
        private DbParameter[] FParametersArray;

        /// <summary>
        /// SQL Statement for one Batch Entry
        /// </summary>
        public String SQLStatement
        {
            get
            {
                return FSQLStatement;
            }
        }

        /// <summary>
        /// Parameters for a Batch Entry (optional)
        /// </summary>
        public DbParameter[] Parameters
        {
            get
            {
                return FParametersArray;
            }
        }


        /// <summary>
        /// Initialises the internal variables that hold the Batch Statment Entry
        /// parameters.
        /// </summary>
        /// <param name="ASQLStatement">SQL Statement for one Batch Entry</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement (can be null)</param>
        /// <returns>void</returns>
        public TSQLBatchStatementEntry(String ASQLStatement, DbParameter[] AParametersArray)
        {
            FSQLStatement = ASQLStatement;
            FParametersArray = AParametersArray;
        }

        /// <summary>
        /// Returns the ParameterArray.
        ///
        /// </summary>
        /// <param name="AParametersArray">ParameterArray
        /// </param>
        /// <returns>void</returns>
        public void GetWholeParameterArray(out DbParameter[] AParametersArray)
        {
            AParametersArray = FParametersArray;
        }

        #endregion
    }


    /// <summary>
    /// A generic Class for managing all kinds of ADO.NET Database Transactions -
    /// to be used instead of concrete ADO.NET Transaction objects, eg. <see cref="OdbcTransaction" />
    /// or NpgsqlTransaction.
    /// </summary>
    /// <remarks>
    /// <em>IMPORTANT:</em> This Transaction Class does not have Commit or
    /// Rollback methods! This is so that the programmers are forced to use the
    /// CommitTransaction and RollbackTransaction methods of the <see cref="DB.TDataBase" /> Class.
    /// <para>
    /// The reasons for this:
    /// <list type="bullet">
    /// <item><see cref="DB.TDataBase" /> can know whether a Transaction is
    /// running (unbelievably, there is no way to find this out through ADO.NET!)</item>
    /// <item><see cref="DB.TDataBase" /> can log Commits and Rollbacks. Another benefit of using this
    /// Class instead of a concrete implementation of ADO.NET Transaction Classes
    /// (eg. <see cref="OdbcTransaction" />) is that it is not tied to a specific ADO.NET
    /// provider, therefore making it easier to use a different ADO.NET provider than ODBC.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public class TDBTransaction : object
    {
        /// <summary>Holds the <see cref="IsolationLevel" /> of the Transaction</summary>
        private System.Data.IsolationLevel FIsolationLevel;

        /// <summary>Holds the Database connection to which the Transaction belongs.</summary>
        private IDbConnection FConnection;

        /// <summary>Holds the actual IDbTransaction.</summary>
        private IDbTransaction FWrappedTransaction;

        /// <summary>
        /// Database connection to which the Transaction belongs
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return FConnection;
            }
        }

        /// <summary>
        /// <see cref="IsolationLevel" /> of the Transaction
        /// </summary>
        public System.Data.IsolationLevel IsolationLevel
        {
            get
            {
                return FIsolationLevel;
            }
        }

        /// <summary>
        /// The actual IDbTransaction.
        /// <para><em><b>WARNING:</b> do not do anything
        /// with this Object other than inspecting it; the correct
        /// working of Transactions in the <see cref="DB.TDataBase" />
        /// Object relies on the fact that it manages everything about
        /// a Transaction!!!</em>
        /// </para>
        /// </summary>
        public IDbTransaction WrappedTransaction
        {
            get
            {
                return FWrappedTransaction;
            }
        }

        /// <summary>
        /// Constructor for a <see cref="TDBTransaction" /> Object.
        /// </summary>
        /// <param name="ATransaction">The concrete IDbTransaction Object that <see cref="TDBTransaction" /> should represent</param>
        /// <param name="AConnection"></param>
        public TDBTransaction(IDbTransaction ATransaction, IDbConnection AConnection)
        {
            FWrappedTransaction = ATransaction;

            // somehow, this line does not work for Progress, gives segmentation fault
            //FConnection = ATransaction.Connection;
            FConnection = AConnection;
            FIsolationLevel = ATransaction.IsolationLevel;
        }
    }
}