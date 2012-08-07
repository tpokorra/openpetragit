//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using System.Text;
using System.Security.Principal;
using System.Security.Cryptography;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// The TUserManager class provides access to the security-related information
    /// of Users of a Petra DB.
    /// </summary>
    public class TUserManager : IUserManager
    {
        private const int RELOADCACHEDUSERINFORETRIES = 5;
        private static int FReloadCachedUserInfoRetryCount = 0;

        #region Resourcestrings

        private static readonly string StrSystemDisabled1 = Catalog.GetString("OpenPetra is currently disabled due to {0}.");

        private static readonly string StrSystemDisabled2 = Catalog.GetString("It will be available on {0} at {1}.");

        private static readonly string StrSystemDisabled2Admin = Catalog.GetString("Proceed with caution.");

        private static readonly string StrUserIsRetired = Catalog.GetString("User is retired.");

        private static readonly string StrInvalidUserIDPassword = Catalog.GetString("Invalid User ID/Password.");

// TODO        private static readonly string StrUserRecordIsLocked = Catalog.GetString(
// TODO            "Your user record is locked by another process!r\nPlease try to login again later.");

        #endregion

        /// <summary>
        /// load details of user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APetraPrincipal"></param>
        /// <returns></returns>
        public static Ict.Petra.Shared.MSysMan.Data.SUserRow LoadUser(String AUserID,
            out Ict.Petra.Shared.Security.TPetraPrincipal APetraPrincipal)
        {
            SUserRow ReturnValue;

            Ict.Petra.Shared.Security.TPetraIdentity PetraIdentity;
            ReturnValue = LoadUser(AUserID, out PetraIdentity);
            APetraPrincipal = new TPetraPrincipal(PetraIdentity, TGroupManager.LoadUserGroups(
                    AUserID), TTableAccessPermissionManager.LoadTableAccessPermissions(
                    AUserID), TModuleAccessManager.LoadUserModules(AUserID));
#if DEBUGMODE
            if (TLogging.DL >= 8)
            {
                Console.WriteLine("APetraPrincipal.IsTableAccessOK(tapMODIFY, 'p_person'): " +
                    APetraPrincipal.IsTableAccessOK(TTableAccessPermission.tapMODIFY, "p_person").ToString());

                // Console.ReadLine();
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// load the details of the user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APetraIdentity"></param>
        /// <returns></returns>
        public static Ict.Petra.Shared.MSysMan.Data.SUserRow LoadUser(String AUserID,
            out Ict.Petra.Shared.Security.TPetraIdentity APetraIdentity)
        {
            SUserRow ReturnValue;
            TDBTransaction ReadWriteTransaction;
            Boolean NewTransaction;
            SUserTable UserDT;
            SUserRow UserDR;
            Boolean UserExists;
            DateTime LastLoginDateTime;
            DateTime FailedLoginDateTime;

            ReadWriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Check if user exists in s_user DB Table
            try
            {
                UserExists = SUserAccess.Exists(AUserID, ReadWriteTransaction);
            }
            catch
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        Console.WriteLine("TUserManager.LoadUser: committed own transaction.");
                    }
#endif
                }

                throw;
            }

            if (!UserExists)
            {
                throw new EUserNotExistantException(StrInvalidUserIDPassword);
            }
            else
            {
                try
                {
                    // Load User record
                    UserDT = SUserAccess.LoadByPrimaryKey(AUserID, ReadWriteTransaction);
                }
                catch (Exception Exp)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine("TUserManager.LoadUser: committed own transaction.");
                        }
#endif
                    }

#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine("Exception occured while loading a s_user record: " + Exp.ToString());
                    }
#endif
                    throw;
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 7)
                    {
                        Console.WriteLine("TUserManager.LoadUser: committed own transaction.");
                    }
#endif
                }

                UserDR = UserDT[0];

                if (!UserDR.IsFailedLoginDateNull())
                {
                    FailedLoginDateTime = UserDR.FailedLoginDate.Value;
                    FailedLoginDateTime = FailedLoginDateTime.AddSeconds(Convert.ToDouble(UserDR.FailedLoginTime));
                }
                else
                {
                    FailedLoginDateTime = DateTime.MinValue;
                }

                if (!UserDR.IsLastLoginDateNull())
                {
                    LastLoginDateTime = UserDR.LastLoginDate.Value;
                    LastLoginDateTime = LastLoginDateTime.AddSeconds(Convert.ToDouble(UserDR.LastLoginTime));
                }
                else
                {
                    LastLoginDateTime = DateTime.MinValue;
                }

                Int64 PartnerKey;

                if (!UserDR.IsPartnerKeyNull())
                {
                    PartnerKey = UserDR.PartnerKey;
                }
                else
                {
                    // to make it not match PartnerKey 0, which might be stored in the DB or in a variable
                    PartnerKey = -1;
                }

                // Create PetraIdentity
                APetraIdentity = new Ict.Petra.Shared.Security.TPetraIdentity(
                    AUserID.ToUpper(), UserDR.LastName, UserDR.FirstName, UserDR.LanguageCode, UserDR.AcquisitionCode, DateTime.MinValue,
                    LastLoginDateTime, FailedLoginDateTime, UserDR.FailedLogins, PartnerKey, UserDR.DefaultLedgerNumber, UserDR.Retired,
                    UserDR.CanModify);
                ReturnValue = UserDR;
            }

            return ReturnValue;
        }

        /// <summary>
        /// create SHA1 hash of password and the salt.
        /// replacement for FormsAuthentication.HashPasswordForStoringInConfigFile
        /// which is part of System.Web.dll and not available in the client profile of .net v4.0
        /// </summary>
        /// <returns></returns>
        public static string CreateHashOfPassword(string APasswordAndSalt, string AHashType = "SHA1")
        {
            if (AHashType.ToUpper() == "MD5")
            {
                return BitConverter.ToString(
                    MD5.Create().
                    ComputeHash(Encoding.UTF8.GetBytes(APasswordAndSalt))).Replace("-", "");
            }

            // default to SHA1
            return BitConverter.ToString(
                SHA1.Create().
                ComputeHash(Encoding.UTF8.GetBytes(APasswordAndSalt))).Replace("-", "");
        }

        /// <summary>
        /// make sure the user can login with the correct password
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ASystemEnabled"></param>
        /// <returns></returns>
        public IPrincipal PerformUserAuthentication(String AUserID,
            String APassword,
            out Int32 AProcessID,
            out Boolean ASystemEnabled)
        {
            TVerificationResultCollection VerificationResults;
            DateTime LoginDateTime;
            TPetraPrincipal PetraPrincipal = null;

            AProcessID = -1;
            ASystemEnabled = true;

            try
            {
                SUserRow UserDR = LoadUser(AUserID, out PetraPrincipal);

                // Already assign the global variable here, because it is needed for SUserAccess.SubmitChanges later in this function
                UserInfo.GUserInfo = PetraPrincipal;

                // Check if user is retired
                if (PetraPrincipal.PetraIdentity.Retired)
                {
                    throw new EUserRetiredException(StrUserIsRetired);
                }

                // Console.WriteLine('PetraPrincipal.PetraIdentity.FailedLogins: ' + PetraPrincipal.PetraIdentity.FailedLogins.ToString +
                // '; PetraPrincipal.PetraIdentity.Retired: ' + PetraPrincipal.PetraIdentity.Retired.ToString);
                // Check if user should be autoretired
                if ((PetraPrincipal.PetraIdentity.FailedLogins >= 5) && ((!PetraPrincipal.PetraIdentity.Retired)))
                {
                    UserDR.Retired = true;
                    UserDR.FailedLogins = 4;

                    if (!SaveUser(AUserID, (SUserTable)UserDR.Table, out VerificationResults))
                    {
#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine(Messages.BuildMessageFromVerificationResult("Error while trying to auto-retire user: ",
                                    VerificationResults));
                        }
#endif
                    }

                    throw new EAccessDeniedException(StrUserIsRetired);
                }

                // Check SystemLoginStatus (Petra enabled/disabled) in the SystemStatus table (always holds only one record)
                Boolean NewTransaction = false;
                SSystemStatusTable SystemStatusDT;

                try
                {
                    TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);
                    SystemStatusDT = SSystemStatusAccess.LoadAll(ReadTransaction);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine("TUserManager.PerformUserAuthentication: committed own transaction.");
                        }
#endif
                    }
                }

                if (SystemStatusDT[0].SystemLoginStatus)
                {
                    ASystemEnabled = true;
                }
                else
                {
                    ASystemEnabled = false;

                    if (PetraPrincipal.IsInGroup("SYSADMIN"))
                    {
                        // $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine('SystemLoginStatus = false; is in SYSADMIN Group');$ENDIF
                        PetraPrincipal.LoginMessage =
                            String.Format(StrSystemDisabled1,
                                SystemStatusDT[0].SystemDisabledReason) + Environment.NewLine + Environment.NewLine + StrSystemDisabled2Admin;
                    }
                    else
                    {
                        // $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine('SystemLoginStatus = false; is NOT in SYSADMIN Group');$ENDIF

                        if (!TLoginLog.AddLoginLogEntry(AUserID, "System disabled", true, out AProcessID, out VerificationResults))
                        {
                            // $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine(BuildMessageFromVerificationResult('Error while trying to add a login entry: ', VerificationResults) );$ENDIF
                        }

                        throw new ESystemDisabledException(String.Format(StrSystemDisabled1,
                                SystemStatusDT[0].SystemDisabledReason) + Environment.NewLine + Environment.NewLine +
                            String.Format(StrSystemDisabled2, StringHelper.DateToLocalizedString(SystemStatusDT[0].SystemAvailableDate.Value),
                                SystemStatusDT[0].SystemAvailableDate.Value.AddSeconds(SystemStatusDT[0].SystemAvailableTime).ToShortTimeString()));
                    }
                }

                string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

                if (UserAuthenticationMethod == "OpenPetraDBSUser")
                {
                    // TODO 1 oChristianK cSecurity : Perform user authentication by verifying password hash in the DB
                    // see also ICTPetraWiki: Todo_Petra.NET#Implement_Security_.7B2.7D_.5BChristian.5D
                    if (CreateHashOfPassword(String.Concat(APassword,
                                UserDR.PasswordSalt)) != UserDR.PasswordHash)
                    {
                        // increase failed logins
                        UserDR.FailedLogins++;
                        LoginDateTime = DateTime.Now;
                        UserDR.FailedLoginDate = LoginDateTime;
                        UserDR.FailedLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                        SaveUser(AUserID, (SUserTable)UserDR.Table, out VerificationResults);

                        throw new EPasswordWrongException(Catalog.GetString("Invalid User ID/Password."));
                    }
                }
                else
                {
                    // namespace of the class TUserAuthentication, eg. Plugin.AuthenticationPhpBB
                    // the dll has to be in the normal application directory
                    string Namespace = UserAuthenticationMethod;
                    string NameOfDll = Namespace + ".dll";
                    string NameOfClass = Namespace + ".TUserAuthentication";
                    string ErrorMessage;

                    // dynamic loading of dll
                    System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
                    System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

                    IUserAuthentication auth = (IUserAuthentication)Activator.CreateInstance(CustomClass);

                    if (!auth.AuthenticateUser(AUserID, APassword, out ErrorMessage))
                    {
                        UserDR.FailedLogins++;
                        LoginDateTime = DateTime.Now;
                        UserDR.FailedLoginDate = LoginDateTime;
                        UserDR.FailedLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                        SaveUser(AUserID, (SUserTable)UserDR.Table, out VerificationResults);

                        throw new EPasswordWrongException(ErrorMessage);
                    }
                }

                // Save successful login
                LoginDateTime = DateTime.Now;
                UserDR.LastLoginDate = LoginDateTime;
                UserDR.LastLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                UserDR.FailedLogins = 0;

                if (!SaveUser(AUserID, (SUserTable)UserDR.Table, out VerificationResults))
                {
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine(Messages.BuildMessageFromVerificationResult("Error while trying to auto-retire user: ", VerificationResults));
                    }
#endif
                }

                PetraPrincipal.PetraIdentity.CurrentLogin = LoginDateTime;

                // PetraPrincipal.PetraIdentity.FailedLogins := 0;

                if (PetraPrincipal.IsInGroup("SYSADMIN"))
                {
                    if (!TLoginLog.AddLoginLogEntry(AUserID, "Successful  SYSADMIN", out AProcessID, out VerificationResults))
                    {
                        // $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine(BuildMessageFromVerificationResult('Error while trying to add a login entry: ', VerificationResults) );$ENDIF
                    }
                }
                else
                {
                    if (!TLoginLog.AddLoginLogEntry(AUserID, "Successful", out AProcessID, out VerificationResults))
                    {
                        // $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine(BuildMessageFromVerificationResult('Error while trying to add a login entry: ', VerificationResults) );$ENDIF
                    }
                }

                PetraPrincipal.ProcessID = AProcessID;
                AProcessID = 0;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return PetraPrincipal;
        }

        /// <summary>
        /// Causes an immediately reload of the UserInfo that is stored in a global
        /// variable.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static Ict.Petra.Shared.Security.TPetraPrincipal ReloadCachedUserInfo()
        {
            try
            {
                TPetraPrincipal UserDetails;
                LoadUser(UserInfo.GUserInfo.UserID, out UserDetails);
                UserInfo.GUserInfo = UserDetails;
            }
            catch (EDBConnectionNotAvailableException Exp)
            {
                if (Exp.InnerException is OdbcException)
                {
                    TLogging.Log(Exp.ToString() + "\n\r" + ((OdbcException)(Exp.InnerException)).Errors[0].NativeError.ToString());

                    if (((OdbcException)(Exp.InnerException)).Errors[0].NativeError == -210005)  // Progress Error: "Failure getting table lock on table PUB.s_user_table_access_permission"
                    {
                        if (FReloadCachedUserInfoRetryCount < RELOADCACHEDUSERINFORETRIES)
                        {
#if DEBUGMODE
                            {
                                TLogging.Log(String.Format(
                                        "ReloadCachedUserInfo: can't access DB Table because of Exclusive-Lock on it; will retry again soon! (Retry count: {0})",
                                        FReloadCachedUserInfoRetryCount));
                            }
#endif

                            // Wait a bit and then retry again (recursively calling this procedure!)
                            Thread.Sleep(5000);
                            FReloadCachedUserInfoRetryCount++;
                            ReloadCachedUserInfo();
                        }
                        else
                        {
                            // the number of retries was exceeded; re-throw Exception.
                            FReloadCachedUserInfoRetryCount = 0;

                            throw;
                        }
                    }
                }
                else
                {
                    throw;
                }
            }
            catch (Exception Exp)
            {
                TLogging.Log("Exception occured in ReloadCachedUserInfo: " + Exp.ToString());
                throw;
            }

#if DEBUGMODE
            {
                if (FReloadCachedUserInfoRetryCount > 0)
                {
                    TLogging.Log(String.Format("ReloadCachedUserInfo: resolved Exclusive-Lock situation after {0} retries!)",
                            FReloadCachedUserInfoRetryCount));
                }
            }
#endif

            // reset retry counter
            FReloadCachedUserInfoRetryCount = 0;

            return UserInfo.GUserInfo;
        }

        /// <summary>
        /// save user details (last login time, failed logins etc)
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="AUserDataTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean SaveUser(String AUserID,
            Ict.Petra.Shared.MSysMan.Data.SUserTable AUserDataTable,
            out TVerificationResultCollection AVerificationResult)
        {
            Boolean ReturnValue;
            TDBTransaction TheTransaction;
            Boolean SubmissionOK;

            SubmissionOK = false;
            AVerificationResult = null;

            if ((AUserDataTable != null) && (AUserDataTable.Rows.Count > 0))
            {
                TheTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    SubmissionOK = SUserAccess.SubmitChanges(AUserDataTable,
                        TheTransaction,
                        out AVerificationResult);
                }
                finally
                {
                    if (SubmissionOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                ReturnValue = SubmissionOK;
            }
            else
            {
                // nothing to save!
                ReturnValue = false;
            }

            return ReturnValue;
        }
    }
}