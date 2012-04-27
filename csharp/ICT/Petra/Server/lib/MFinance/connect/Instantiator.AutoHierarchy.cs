// Auto generated with nant generateGlue
// based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.yml
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2012 by OM International
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

//
// Contains a remotable class that instantiates an Object which gives access to
// the MPartner Namespace (from the Client's perspective).
//
// The purpose of the remotable class is to present other classes which are
// Instantiators for sub-namespaces to the Client. The instantiation of the
// sub-namespace objects is completely transparent to the Client!
// The remotable class itself gets instantiated and dynamically remoted by the
// loader class, which in turn gets called when the Client Domain is set up.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core.Security;

using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Interfaces.MFinance.AP;
using Ict.Petra.Shared.Interfaces.MFinance.AR;
using Ict.Petra.Shared.Interfaces.MFinance.Budget;
using Ict.Petra.Shared.Interfaces.MFinance.Cacheable;
using Ict.Petra.Shared.Interfaces.MFinance.ImportExport;
using Ict.Petra.Shared.Interfaces.MFinance.Gift;
using Ict.Petra.Shared.Interfaces.MFinance.GL;
using Ict.Petra.Shared.Interfaces.MFinance.ICH;
using Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd;
using Ict.Petra.Shared.Interfaces.MFinance.Reporting;
using Ict.Petra.Shared.Interfaces.MFinance.Setup;
using Ict.Petra.Shared.Interfaces.MFinance.AP.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.AP.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.AR.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Budget.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Budget.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.ImportExport.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Gift.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Gift.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.GL.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.ICH.WebConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.PeriodEnd.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Reporting.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Setup.UIConnectors;
using Ict.Petra.Shared.Interfaces.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.AP;
using Ict.Petra.Server.MFinance.Instantiator.AR;
using Ict.Petra.Server.MFinance.Instantiator.Budget;
using Ict.Petra.Server.MFinance.Instantiator.Cacheable;
using Ict.Petra.Server.MFinance.Instantiator.ImportExport;
using Ict.Petra.Server.MFinance.Instantiator.Gift;
using Ict.Petra.Server.MFinance.Instantiator.GL;
using Ict.Petra.Server.MFinance.Instantiator.ICH;
using Ict.Petra.Server.MFinance.Instantiator.PeriodEnd;
using Ict.Petra.Server.MFinance.Instantiator.Reporting;
using Ict.Petra.Server.MFinance.Instantiator.Setup;
using Ict.Petra.Server.MFinance.Instantiator.AP.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.AP.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.AR.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Budget.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Budget.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.ImportExport.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Gift.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.GL.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.ICH.WebConnectors;
using Ict.Petra.Server.MFinance.Instantiator.PeriodEnd.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Setup.UIConnectors;
using Ict.Petra.Server.MFinance.Instantiator.Setup.WebConnectors;
//using Ict.Petra.Server.MFinance.AP;
//using Ict.Petra.Server.MFinance.AR;
//using Ict.Petra.Server.MFinance.Budget;
using Ict.Petra.Server.MFinance.Cacheable;
//using Ict.Petra.Server.MFinance.ImportExport;
//using Ict.Petra.Server.MFinance.Gift;
//using Ict.Petra.Server.MFinance.GL;
//using Ict.Petra.Server.MFinance.ICH;
//using Ict.Petra.Server.MFinance.PeriodEnd;
using Ict.Petra.Server.MFinance.Reporting;
//using Ict.Petra.Server.MFinance.Setup;
using Ict.Petra.Server.MFinance.AP.UIConnectors;
using Ict.Petra.Server.MFinance.AP.WebConnectors;
//using Ict.Petra.Server.MFinance.AR.WebConnectors;
//using Ict.Petra.Server.MFinance.Budget.UIConnectors;
using Ict.Petra.Server.MFinance.Budget.WebConnectors;
using Ict.Petra.Server.MFinance.ImportExport.WebConnectors;
//using Ict.Petra.Server.MFinance.Gift.UIConnectors;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
//using Ict.Petra.Server.MFinance.GL.UIConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MFinance.ICH.WebConnectors;
//using Ict.Petra.Server.MFinance.PeriodEnd.UIConnectors;
//using Ict.Petra.Server.MFinance.Reporting.UIConnectors;
//using Ict.Petra.Server.MFinance.Setup.UIConnectors;
#region ManualCode
using System.Xml;
using System.Collections.Specialized;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner.Partner.Data;
#endregion ManualCode
using Ict.Petra.Server.MFinance.Setup.WebConnectors;

namespace Ict.Petra.Server.MFinance.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMFinanceNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>holds reference to the TMFinance object</summary>
        private ObjRef FRemotedObject;

        /// <summary>Constructor</summary>
        public TMFinanceNamespaceLoader()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created in application domain: " + Thread.GetDomain().FriendlyName);
            }

#endif
        }

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMFinance
        /// class to make it callable remotely from the Client.
        ///
        /// @comment This function gets called from TRemoteLoader.LoadPetraModuleAssembly.
        /// This call is done late-bound through .NET Reflection!
        ///
        /// WARNING: If the name of this function or its parameters should change, this
        /// needs to be reflected in the call to this function in
        /// TRemoteLoader.LoadPetraModuleAssembly!!!
        ///
        /// </summary>
        /// <returns>The URL at which the remoted object can be reached.</returns>
        public String GetRemotingURL()
        {
            TMFinance RemotedObject;
            DateTime RemotingTime;
            String RemoteAtURI;
            String RandomString;
            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte rndbytespos;
            Byte[] rndbytes = new Byte[5];

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TMFinanceNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

#endif

            RandomString = "";
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rnd.GetBytes(rndbytes);

            for (rndbytespos = 1; rndbytespos <= 4; rndbytespos += 1)
            {
                RandomString = RandomString + rndbytes[rndbytespos].ToString();
            }

            RemotingTime = DateTime.Now;
            RemotedObject = new TMFinance();
            RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
            FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IMFinanceNamespace));
            FRemotingURL = RemoteAtURI; // FRemotedObject.URI;

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TMFinance.URI: " + FRemotedObject.URI);
            }

#endif

            return FRemotingURL;
        }

    }

    /// <summary>
    /// REMOTEABLE CLASS. MFinance Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMFinance : MarshalByRefObject, IMFinanceNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TAPNamespace FAPSubNamespace;
        private TARNamespace FARSubNamespace;
        private TBudgetNamespace FBudgetSubNamespace;
        private TCacheableNamespace FCacheableSubNamespace;
        private TImportExportNamespace FImportExportSubNamespace;
        private TGiftNamespace FGiftSubNamespace;
        private TGLNamespace FGLSubNamespace;
        private TICHNamespace FICHSubNamespace;
        private TPeriodEndNamespace FPeriodEndSubNamespace;
        private TReportingNamespace FReportingSubNamespace;
        private TSetupNamespace FSetupSubNamespace;

        /// <summary>Constructor</summary>
        public TMFinance()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMFinance()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMFinance object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'AP' subnamespace contains further subnamespaces.</summary>
        public IAPNamespace AP
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.AP' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.AP' sub-namespace
                //

                // accessing TAPNamespace the first time? > instantiate the object
                if (FAPSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TAPNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.AP') should be automatically contructable.
                    FAPSubNamespace = new TAPNamespace();
                }

                return FAPSubNamespace;
            }

        }

        /// <summary>The 'AR' subnamespace contains further subnamespaces.</summary>
        public IARNamespace AR
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.AR' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.AR' sub-namespace
                //

                // accessing TARNamespace the first time? > instantiate the object
                if (FARSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TARNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.AR') should be automatically contructable.
                    FARSubNamespace = new TARNamespace();
                }

                return FARSubNamespace;
            }

        }

        /// <summary>The 'Budget' subnamespace contains further subnamespaces.</summary>
        public IBudgetNamespace Budget
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.Budget' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.Budget' sub-namespace
                //

                // accessing TBudgetNamespace the first time? > instantiate the object
                if (FBudgetSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TBudgetNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.Budget') should be automatically contructable.
                    FBudgetSubNamespace = new TBudgetNamespace();
                }

                return FBudgetSubNamespace;
            }

        }

        /// <summary>The 'Cacheable' subnamespace contains further subnamespaces.</summary>
        public ICacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FCacheableSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.Cacheable') should be automatically contructable.
                    FCacheableSubNamespace = new TCacheableNamespace();
                }

                return FCacheableSubNamespace;
            }

        }

        /// <summary>The 'ImportExport' subnamespace contains further subnamespaces.</summary>
        public IImportExportNamespace ImportExport
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.ImportExport' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.ImportExport' sub-namespace
                //

                // accessing TImportExportNamespace the first time? > instantiate the object
                if (FImportExportSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TImportExportNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.ImportExport') should be automatically contructable.
                    FImportExportSubNamespace = new TImportExportNamespace();
                }

                return FImportExportSubNamespace;
            }

        }

        /// <summary>The 'Gift' subnamespace contains further subnamespaces.</summary>
        public IGiftNamespace Gift
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.Gift' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.Gift' sub-namespace
                //

                // accessing TGiftNamespace the first time? > instantiate the object
                if (FGiftSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TGiftNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.Gift') should be automatically contructable.
                    FGiftSubNamespace = new TGiftNamespace();
                }

                return FGiftSubNamespace;
            }

        }

        /// <summary>The 'GL' subnamespace contains further subnamespaces.</summary>
        public IGLNamespace GL
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.GL' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.GL' sub-namespace
                //

                // accessing TGLNamespace the first time? > instantiate the object
                if (FGLSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TGLNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.GL') should be automatically contructable.
                    FGLSubNamespace = new TGLNamespace();
                }

                return FGLSubNamespace;
            }

        }

        /// <summary>The 'ICH' subnamespace contains further subnamespaces.</summary>
        public IICHNamespace ICH
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.ICH' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.ICH' sub-namespace
                //

                // accessing TICHNamespace the first time? > instantiate the object
                if (FICHSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TICHNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.ICH') should be automatically contructable.
                    FICHSubNamespace = new TICHNamespace();
                }

                return FICHSubNamespace;
            }

        }

        /// <summary>The 'PeriodEnd' subnamespace contains further subnamespaces.</summary>
        public IPeriodEndNamespace PeriodEnd
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.PeriodEnd' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.PeriodEnd' sub-namespace
                //

                // accessing TPeriodEndNamespace the first time? > instantiate the object
                if (FPeriodEndSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPeriodEndNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.PeriodEnd') should be automatically contructable.
                    FPeriodEndSubNamespace = new TPeriodEndNamespace();
                }

                return FPeriodEndSubNamespace;
            }

        }

        /// <summary>The 'Reporting' subnamespace contains further subnamespaces.</summary>
        public IReportingNamespace Reporting
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.Reporting' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.Reporting' sub-namespace
                //

                // accessing TReportingNamespace the first time? > instantiate the object
                if (FReportingSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TReportingNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.Reporting') should be automatically contructable.
                    FReportingSubNamespace = new TReportingNamespace();
                }

                return FReportingSubNamespace;
            }

        }

        /// <summary>The 'Setup' subnamespace contains further subnamespaces.</summary>
        public ISetupNamespace Setup
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MFinance.Setup' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MFinance.Setup' sub-namespace
                //

                // accessing TSetupNamespace the first time? > instantiate the object
                if (FSetupSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSetupNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MFinance.Instantiator.Setup') should be automatically contructable.
                    FSetupSubNamespace = new TSetupNamespace();
                }

                return FSetupSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.AP
{
    /// <summary>auto generated class </summary>
    public class TAPNamespace : MarshalByRefObject, IAPNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TAPUIConnectorsNamespace FAPUIConnectorsSubNamespace;
        private TAPWebConnectorsNamespace FAPWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TAPNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TAPNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TAPNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'APUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IAPUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'AP.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'AP.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FAPUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TAPUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.AP.Instantiator.UIConnectors') should be automatically contructable.
                    FAPUIConnectorsSubNamespace = new TAPUIConnectorsNamespace();
                }

                return FAPUIConnectorsSubNamespace;
            }

        }

        /// <summary>The 'APWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IAPWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'AP.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'AP.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FAPWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TAPWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.AP.Instantiator.WebConnectors') should be automatically contructable.
                    FAPWebConnectorsSubNamespace = new TAPWebConnectorsNamespace();
                }

                return FAPWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.AP.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TAPUIConnectorsNamespace : MarshalByRefObject, IAPUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TAPUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TAPUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TAPUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public IAPUIConnectorsFind Find()
        {
            return new TFindUIConnector();
        }

        /// generated method from interface
        public IAPUIConnectorsSupplierEdit SupplierEdit()
        {
            return new TSupplierEditUIConnector();
        }

        /// generated method from interface
        public IAPUIConnectorsSupplierEdit SupplierEdit(ref AccountsPayableTDS ADataSet,
                                                        Int64 APartnerKey)
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TSupplierEditUIConnector...");
            }

#endif
            TSupplierEditUIConnector ReturnValue = new TSupplierEditUIConnector();

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TSupplierEditUIConnector.GetData...");
            }

#endif
            ADataSet = ReturnValue.GetData(APartnerKey);
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TSupplierEditUIConnector.GetData finished.");
            }

#endif
            return ReturnValue;
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.AP.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TAPWebConnectorsNamespace : MarshalByRefObject, IAPWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TAPWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TAPWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TAPWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public ALedgerTable GetLedgerInfo(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "GetLedgerInfo", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.GetLedgerInfo(ALedgerNumber);
        }

        /// generated method from connector
        public AccountsPayableTDS LoadAApSupplier(Int32 ALedgerNumber,
                                                  Int64 APartnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "LoadAApSupplier", ";INT;LONG;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.LoadAApSupplier(ALedgerNumber, APartnerKey);
        }

        /// generated method from connector
        public AccountsPayableTDS LoadAApDocument(Int32 ALedgerNumber,
                                                  Int32 AApDocumentId)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "LoadAApDocument", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.LoadAApDocument(ALedgerNumber, AApDocumentId);
        }

        /// generated method from connector
        public AccountsPayableTDS CreateAApDocument(Int32 ALedgerNumber,
                                                    Int64 APartnerKey,
                                                    System.Boolean ACreditNoteOrInvoice)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "CreateAApDocument", ";INT;LONG;BOOL;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.CreateAApDocument(ALedgerNumber, APartnerKey, ACreditNoteOrInvoice);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveAApDocument(ref AccountsPayableTDS AInspectDS,
                                                    out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "SaveAApDocument", ";ACCOUNTSPAYABLETDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.SaveAApDocument(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public AccountsPayableTDS CreateAApDocumentDetail(Int32 ALedgerNumber,
                                                          Int32 AApDocumentId,
                                                          System.String AApSupplier_DefaultExpAccount,
                                                          System.String AApSupplier_DefaultCostCentre,
                                                          System.Decimal AAmount,
                                                          Int32 ALastDetailNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "CreateAApDocumentDetail", ";INT;INT;STRING;STRING;DECIMAL;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.CreateAApDocumentDetail(ALedgerNumber, AApDocumentId, AApSupplier_DefaultExpAccount, AApSupplier_DefaultCostCentre, AAmount, ALastDetailNumber);
        }

        /// generated method from connector
        public AccountsPayableTDS FindAApDocument(Int32 ALedgerNumber,
                                                  Int64 ASupplierKey,
                                                  System.String ADocumentStatus,
                                                  System.Boolean IsCreditNoteNotInvoice,
                                                  System.Boolean AHideAgedTransactions)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "FindAApDocument", ";INT;LONG;STRING;BOOL;BOOL;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.FindAApDocument(ALedgerNumber, ASupplierKey, ADocumentStatus, IsCreditNoteNotInvoice, AHideAgedTransactions);
        }

        /// generated method from connector
        public String CheckAccountsAndCostCentres(Int32 ALedgerNumber,
                                                  List<String>AccountCodesCostCentres)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "CheckAccountsAndCostCentres", ";INT;STRING?;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.CheckAccountsAndCostCentres(ALedgerNumber, AccountCodesCostCentres);
        }

        /// generated method from connector
        public System.Boolean DeleteAPDocuments(Int32 ALedgerNumber,
                                                List<Int32>ADeleteTheseDocs,
                                                out TVerificationResultCollection AVerifications)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "DeleteAPDocuments", ";INT;INT?;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.DeleteAPDocuments(ALedgerNumber, ADeleteTheseDocs, out AVerifications);
        }

        /// generated method from connector
        public System.Boolean PostAPDocuments(Int32 ALedgerNumber,
                                              List<Int32>AAPDocumentIds,
                                              DateTime APostingDate,
                                              Boolean Reversal,
                                              out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "PostAPDocuments", ";INT;INT?;DATETIME;BOOL;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.PostAPDocuments(ALedgerNumber, AAPDocumentIds, APostingDate, Reversal, out AVerificationResult);
        }

        /// generated method from connector
        public System.Boolean CreatePaymentTableEntries(ref AccountsPayableTDS ADataset,
                                                        Int32 ALedgerNumber,
                                                        List<Int32>ADocumentsToPay)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "CreatePaymentTableEntries", ";ACCOUNTSPAYABLETDS;INT;INT?;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.CreatePaymentTableEntries(ref ADataset, ALedgerNumber, ADocumentsToPay);
        }

        /// generated method from connector
        public System.Boolean PostAPPayments(ref AccountsPayableTDS MainDS,
                                             DateTime APostingDate,
                                             out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "PostAPPayments", ";ACCOUNTSPAYABLETDS;DATETIME;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.PostAPPayments(ref MainDS, APostingDate, out AVerificationResult);
        }

        /// generated method from connector
        public AccountsPayableTDS LoadAPPayment(Int32 ALedgerNumber,
                                                Int32 APaymentNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "LoadAPPayment", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.LoadAPPayment(ALedgerNumber, APaymentNumber);
        }

        /// generated method from connector
        public System.Boolean ReversePayment(Int32 ALedgerNumber,
                                             Int32 APaymentNumber,
                                             DateTime APostingDate,
                                             out TVerificationResultCollection AVerifications)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector), "ReversePayment", ";INT;INT;DATETIME;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.AP.WebConnectors.TTransactionWebConnector.ReversePayment(ALedgerNumber, APaymentNumber, APostingDate, out AVerifications);
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.AR
{
    /// <summary>auto generated class </summary>
    public class TARNamespace : MarshalByRefObject, IARNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TARWebConnectorsNamespace FARWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TARNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TARNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TARNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'ARWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IARWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'AR.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'AR.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FARWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TARWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.AR.Instantiator.WebConnectors') should be automatically contructable.
                    FARWebConnectorsSubNamespace = new TARWebConnectorsNamespace();
                }

                return FARWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.AR.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TARWebConnectorsNamespace : MarshalByRefObject, IARWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TARWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TARWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TARWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Budget
{
    /// <summary>auto generated class </summary>
    public class TBudgetNamespace : MarshalByRefObject, IBudgetNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TBudgetUIConnectorsNamespace FBudgetUIConnectorsSubNamespace;
        private TBudgetWebConnectorsNamespace FBudgetWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TBudgetNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TBudgetNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TBudgetNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'BudgetUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IBudgetUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Budget.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Budget.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FBudgetUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TBudgetUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Budget.Instantiator.UIConnectors') should be automatically contructable.
                    FBudgetUIConnectorsSubNamespace = new TBudgetUIConnectorsNamespace();
                }

                return FBudgetUIConnectorsSubNamespace;
            }

        }

        /// <summary>The 'BudgetWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IBudgetWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Budget.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Budget.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FBudgetWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TBudgetWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Budget.Instantiator.WebConnectors') should be automatically contructable.
                    FBudgetWebConnectorsSubNamespace = new TBudgetWebConnectorsNamespace();
                }

                return FBudgetWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Budget.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TBudgetUIConnectorsNamespace : MarshalByRefObject, IBudgetUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TBudgetUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TBudgetUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TBudgetUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Budget.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TBudgetWebConnectorsNamespace : MarshalByRefObject, IBudgetWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TBudgetWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TBudgetWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TBudgetWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public BudgetTDS LoadBudgetForAutoGenerate(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetAutoGenerateWebConnector), "LoadBudgetForAutoGenerate", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetAutoGenerateWebConnector.LoadBudgetForAutoGenerate(ALedgerNumber);
        }

        /// generated method from connector
        public System.Boolean GenBudgetForNextYear(System.Int32 ALedgerNumber,
                                                   System.Int32 ABudgetSeq,
                                                   System.String AForecastType)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetAutoGenerateWebConnector), "GenBudgetForNextYear", ";INT;INT;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetAutoGenerateWebConnector.GenBudgetForNextYear(ALedgerNumber, ABudgetSeq, AForecastType);
        }

        /// generated method from connector
        public System.Boolean LoadBudgetForConsolidate(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetConsolidateWebConnector), "LoadBudgetForConsolidate", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetConsolidateWebConnector.LoadBudgetForConsolidate(ALedgerNumber);
        }

        /// generated method from connector
        public System.Boolean ConsolidateBudgets(Int32 ALedgerNumber,
                                                 System.Boolean AConsolidateAll,
                                                 out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetConsolidateWebConnector), "ConsolidateBudgets", ";INT;BOOL;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetConsolidateWebConnector.ConsolidateBudgets(ALedgerNumber, AConsolidateAll, out AVerificationResult);
        }

        /// generated method from connector
        public System.Decimal GetBudgetValue(ref DataTable APeriodDataTable,
                                             System.Int32 AGLMSequence,
                                             System.Int32 APeriodNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetConsolidateWebConnector), "GetBudgetValue", ";DATATABLE;INT;INT;");
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetConsolidateWebConnector.GetBudgetValue(ref APeriodDataTable, AGLMSequence, APeriodNumber);
        }

        /// generated method from connector
        public BudgetTDS LoadBudget(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector), "LoadBudget", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector.LoadBudget(ALedgerNumber);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveBudget(ref BudgetTDS AInspectDS,
                                               out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector), "SaveBudget", ";BUDGETTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector.SaveBudget(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public System.Int32 ImportBudgets(Int32 ALedgerNumber,
                                          Int32 ACurrentBudgetYear,
                                          System.String ACSVFileName,
                                          System.String[] AFdlgSeparator,
                                          ref BudgetTDS AImportDS,
                                          out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector), "ImportBudgets", ";INT;INT;STRING;STRING.ARRAY;BUDGETTDS;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector.ImportBudgets(ALedgerNumber, ACurrentBudgetYear, ACSVFileName, AFdlgSeparator, ref AImportDS, out AVerificationResult);
        }

        /// generated method from connector
        public System.Int32 GetGLMSequenceForBudget(System.Int32 ALedgerNumber,
                                                    System.String AAccountCode,
                                                    System.String ACostCentreCode,
                                                    System.Int32 AYear)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector), "GetGLMSequenceForBudget", ";INT;STRING;STRING;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector.GetGLMSequenceForBudget(ALedgerNumber, AAccountCode, ACostCentreCode, AYear);
        }

        /// generated method from connector
        public System.Decimal GetActual(System.Int32 ALedgerNumber,
                                        System.Int32 AGLMSeqThisYear,
                                        System.Int32 AGLMSeqNextYear,
                                        System.Int32 APeriodNumber,
                                        System.Int32 ANumberAccountingPeriods,
                                        System.Int32 ACurrentFinancialYear,
                                        System.Int32 AThisYear,
                                        System.Boolean AYTD,
                                        System.String ACurrencySelect)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector), "GetActual", ";INT;INT;INT;INT;INT;INT;INT;BOOL;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector.GetActual(ALedgerNumber, AGLMSeqThisYear, AGLMSeqNextYear, APeriodNumber, ANumberAccountingPeriods, ACurrentFinancialYear, AThisYear, AYTD, ACurrencySelect);
        }

        /// generated method from connector
        public System.Decimal GetBudget(System.Int32 AGLMSeqThisYear,
                                        System.Int32 AGLMSeqNextYear,
                                        System.Int32 APeriodNumber,
                                        System.Int32 ANumberAccountingPeriods,
                                        System.Boolean AYTD,
                                        System.String ACurrencySelect)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector), "GetBudget", ";INT;INT;INT;INT;BOOL;STRING;");
            return Ict.Petra.Server.MFinance.Budget.WebConnectors.TBudgetMaintainWebConnector.GetBudget(AGLMSeqThisYear, AGLMSeqNextYear, APeriodNumber, ANumberAccountingPeriods, AYTD, ACurrencySelect);
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Cacheable
{
    /// <summary>auto generated class </summary>
    public class TCacheableNamespace : MarshalByRefObject, ICacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MFinance.Cacheable.TCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TCacheableNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MFinance.Cacheable.TCacheable();
            #endregion ManualCode
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TCacheableNamespace object exists until this AppDomain is unloaded!
        }

        #region ManualCode

        /// <summary>
        /// Returns the desired cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTable">Used to select the desired DataTable</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable The desired DataTable
        /// </returns>
        private DataTable GetCacheableTableInternal(TCacheableFinanceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the desired cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTable">Used to select the desired DataTable</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="ALedgerNumber">The LedgerNumber that the rows that should be stored in
        /// the Cache need to match.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable The desired DataTable
        /// </returns>
        private DataTable GetCacheableTableInternal(TCacheableFinanceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            System.Int32 ALedgerNumber,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, ALedgerNumber, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       System.Int32 ALedgerNumber,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, ALedgerNumber, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableFinanceTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                          System.Int32 ALedgerNumber)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, ALedgerNumber, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                          System.Int32 ALedgerNumber,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, ALedgerNumber, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      System.Int32 ALedgerNumber,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, ALedgerNumber, out AVerificationResult);
            #endregion ManualCode                                    
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode                                    
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.ImportExport
{
    /// <summary>auto generated class </summary>
    public class TImportExportNamespace : MarshalByRefObject, IImportExportNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TImportExportWebConnectorsNamespace FImportExportWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TImportExportNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TImportExportNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'ImportExportWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IImportExportWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'ImportExport.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'ImportExport.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FImportExportWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TImportExportWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.ImportExport.Instantiator.WebConnectors') should be automatically contructable.
                    FImportExportWebConnectorsSubNamespace = new TImportExportWebConnectorsNamespace();
                }

                return FImportExportWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.ImportExport.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TImportExportWebConnectorsNamespace : MarshalByRefObject, IImportExportWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TImportExportWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TImportExportWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public TSubmitChangesResult StoreNewBankStatement(ref AEpStatementTable AStmtTable,
                                                          AEpTransactionTable ATransTable,
                                                          out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "StoreNewBankStatement", ";AEPSTATEMENTTABLE;AEPTRANSACTIONTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.StoreNewBankStatement(ref AStmtTable, ATransTable, out AVerificationResult);
        }

        /// generated method from connector
        public AEpStatementTable GetImportedBankStatements(DateTime AStartDate)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "GetImportedBankStatements", ";DATETIME;");
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.GetImportedBankStatements(AStartDate);
        }

        /// generated method from connector
        public System.Boolean DropBankStatement(Int32 AEpStatementKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "DropBankStatement", ";INT;");
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.DropBankStatement(AEpStatementKey);
        }

        /// generated method from connector
        public BankImportTDS GetBankStatementTransactionsAndMatches(Int32 AStatementKey,
                                                                    Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "GetBankStatementTransactionsAndMatches", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber);
        }

        /// generated method from connector
        public System.Boolean CommitMatches(BankImportTDS AMainDS)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "CommitMatches", ";BANKIMPORTTDS;");
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.CommitMatches(AMainDS);
        }

        /// generated method from connector
        public Int32 CreateGiftBatch(BankImportTDS AMainDS,
                                     Int32 ALedgerNumber,
                                     Int32 AStatementKey,
                                     Int32 AGiftBatchNumber,
                                     out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "CreateGiftBatch", ";BANKIMPORTTDS;INT;INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.CreateGiftBatch(AMainDS, ALedgerNumber, AStatementKey, AGiftBatchNumber, out AVerificationResult);
        }

        /// generated method from connector
        public Int32 CreateGLBatch(BankImportTDS AMainDS,
                                   Int32 ALedgerNumber,
                                   Int32 AStatementKey,
                                   Int32 AGLBatchNumber,
                                   out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector), "CreateGLBatch", ";BANKIMPORTTDS;INT;INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.ImportExport.WebConnectors.TBankImportWebConnector.CreateGLBatch(AMainDS, ALedgerNumber, AStatementKey, AGLBatchNumber, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Gift
{
    /// <summary>auto generated class </summary>
    public class TGiftNamespace : MarshalByRefObject, IGiftNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TGiftUIConnectorsNamespace FGiftUIConnectorsSubNamespace;
        private TGiftWebConnectorsNamespace FGiftWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TGiftNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TGiftNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TGiftNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'GiftUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IGiftUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Gift.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Gift.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FGiftUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TGiftUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Gift.Instantiator.UIConnectors') should be automatically contructable.
                    FGiftUIConnectorsSubNamespace = new TGiftUIConnectorsNamespace();
                }

                return FGiftUIConnectorsSubNamespace;
            }

        }

        /// <summary>The 'GiftWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IGiftWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Gift.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Gift.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FGiftWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TGiftWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Gift.Instantiator.WebConnectors') should be automatically contructable.
                    FGiftWebConnectorsSubNamespace = new TGiftWebConnectorsNamespace();
                }

                return FGiftWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Gift.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TGiftUIConnectorsNamespace : MarshalByRefObject, IGiftUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TGiftUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TGiftUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TGiftUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Gift.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TGiftWebConnectorsNamespace : MarshalByRefObject, IGiftWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TGiftWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TGiftWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TGiftWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public Int32 FieldChangeAdjustment(Int32 ALedgerNumber,
                                           Int64 ARecipientKey,
                                           DateTime AStartDate,
                                           DateTime AEndDate,
                                           Int64 AOldField,
                                           DateTime ADateCorrection,
                                           System.Boolean AWithReceipt)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TAdjustmentWebConnector), "FieldChangeAdjustment", ";INT;LONG;DATETIME;DATETIME;LONG;DATETIME;BOOL;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TAdjustmentWebConnector.FieldChangeAdjustment(ALedgerNumber, ARecipientKey, AStartDate, AEndDate, AOldField, ADateCorrection, AWithReceipt);
        }

        /// generated method from connector
        public System.Boolean GiftRevertAdjust(Hashtable requestParams,
                                               out TVerificationResultCollection AMessages)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TAdjustmentWebConnector), "GiftRevertAdjust", ";HASHTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TAdjustmentWebConnector.GiftRevertAdjust(requestParams, out AMessages);
        }

        /// generated method from connector
        public NewDonorTDS GetDonorsOfWorker(Int64 AWorkerPartnerKey,
                                             Int32 ALedgerNumber,
                                             System.Boolean ADropForeignAddresses,
                                             System.Boolean ADropPartnersWithNoMailing)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TDonorsOfWorkerWebConnector), "GetDonorsOfWorker", ";LONG;INT;BOOL;BOOL;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TDonorsOfWorkerWebConnector.GetDonorsOfWorker(AWorkerPartnerKey, ALedgerNumber, ADropForeignAddresses, ADropPartnersWithNoMailing);
        }

        /// generated method from connector
        public Boolean GetMotivationGroupAndDetail(Int64 partnerKey,
                                                   ref String motivationGroup,
                                                   ref String motivationDetail)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TGuiTools), "GetMotivationGroupAndDetail", ";LONG;STRING;STRING;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TGuiTools.GetMotivationGroupAndDetail(partnerKey, ref motivationGroup, ref motivationDetail);
        }

        /// generated method from connector
        public NewDonorTDS GetNewDonorSubscriptions(System.String APublicationCode,
                                                    DateTime ASubscriptionStartFrom,
                                                    DateTime ASubscriptionStartUntil,
                                                    System.String AExtractName,
                                                    System.Boolean ADropForeignAddresses)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TNewDonorSubscriptionsWebConnector), "GetNewDonorSubscriptions", ";STRING;DATETIME;DATETIME;STRING;BOOL;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TNewDonorSubscriptionsWebConnector.GetNewDonorSubscriptions(APublicationCode, ASubscriptionStartFrom, ASubscriptionStartUntil, AExtractName, ADropForeignAddresses);
        }

        /// generated method from connector
        public StringCollection PrepareNewDonorLetters(ref NewDonorTDS AMainDS,
                                                       System.String AHTMLTemplate)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TNewDonorSubscriptionsWebConnector), "PrepareNewDonorLetters", ";NEWDONORTDS;STRING;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TNewDonorSubscriptionsWebConnector.PrepareNewDonorLetters(ref AMainDS, AHTMLTemplate);
        }

        /// generated method from connector
        public System.String CreateAnnualGiftReceipts(Int32 ALedgerNumber,
                                                      DateTime AStartDate,
                                                      DateTime AEndDate,
                                                      System.String AHTMLTemplate)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TReceiptingWebConnector), "CreateAnnualGiftReceipts", ";INT;DATETIME;DATETIME;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TReceiptingWebConnector.CreateAnnualGiftReceipts(ALedgerNumber, AStartDate, AEndDate, AHTMLTemplate);
        }

        /// generated method from connector
        public GiftBatchTDS LoadMotivationDetails(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TGiftSetupWebConnector), "LoadMotivationDetails", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TGiftSetupWebConnector.LoadMotivationDetails(ALedgerNumber);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveMotivationDetails(ref GiftBatchTDS AInspectDS,
                                                          out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TGiftSetupWebConnector), "SaveMotivationDetails", ";GIFTBATCHTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TGiftSetupWebConnector.SaveMotivationDetails(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber,
                                             DateTime ADateEffective)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "CreateAGiftBatch", ";INT;DATETIME;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.CreateAGiftBatch(ALedgerNumber, ADateEffective);
        }

        /// generated method from connector
        public GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "CreateAGiftBatch", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.CreateAGiftBatch(ALedgerNumber);
        }

        /// generated method from connector
        public RecurringGiftBatchTDS CreateARecurringGiftBatch(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "CreateARecurringGiftBatch", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.CreateARecurringGiftBatch(ALedgerNumber);
        }

        /// generated method from connector
        public Boolean SubmitRecurringGiftBatch(Hashtable requestParams,
                                                out TVerificationResultCollection AMessages)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "SubmitRecurringGiftBatch", ";HASHTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.SubmitRecurringGiftBatch(requestParams, out AMessages);
        }

        /// generated method from connector
        public DataTable GetAvailableGiftYears(Int32 ALedgerNumber,
                                               out String ADisplayMember,
                                               out String AValueMember)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "GetAvailableGiftYears", ";INT;STRING;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.GetAvailableGiftYears(ALedgerNumber, out ADisplayMember, out AValueMember);
        }

        /// generated method from connector
        public GiftBatchTDS LoadAGiftBatch(Int32 ALedgerNumber,
                                           System.String ABatchStatus,
                                           Int32 AYear,
                                           Int32 APeriod)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadAGiftBatch", ";INT;STRING;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadAGiftBatch(ALedgerNumber, ABatchStatus, AYear, APeriod);
        }

        /// generated method from connector
        public RecurringGiftBatchTDS LoadARecurringGiftBatch(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadARecurringGiftBatch", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadARecurringGiftBatch(ALedgerNumber);
        }

        /// generated method from connector
        public GiftBatchTDS LoadTransactions(Int32 ALedgerNumber,
                                             Int32 ABatchNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadTransactions", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadTransactions(ALedgerNumber, ABatchNumber);
        }

        /// generated method from connector
        public GiftBatchTDS LoadDonorRecipientHistory(Hashtable requestParams,
                                                      out TVerificationResultCollection AMessages)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadDonorRecipientHistory", ";HASHTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadDonorRecipientHistory(requestParams, out AMessages);
        }

        /// generated method from connector
        public RecurringGiftBatchTDS LoadRecurringTransactions(Int32 ALedgerNumber,
                                                               Int32 ABatchNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadRecurringTransactions", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadRecurringTransactions(ALedgerNumber, ABatchNumber);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveGiftBatchTDS(ref GiftBatchTDS AInspectDS,
                                                     out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "SaveGiftBatchTDS", ";GIFTBATCHTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.SaveGiftBatchTDS(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveRecurringGiftBatchTDS(ref RecurringGiftBatchTDS AInspectDS,
                                                              out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "SaveRecurringGiftBatchTDS", ";RECURRINGGIFTBATCHTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.SaveRecurringGiftBatchTDS(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public System.Decimal CalculateAdminFee(GiftBatchTDS MainDS,
                                                Int32 ALedgerNumber,
                                                System.String AFeeCode,
                                                System.Decimal AGiftAmount,
                                                out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "CalculateAdminFee", ";GIFTBATCHTDS;INT;STRING;DECIMAL;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.CalculateAdminFee(MainDS, ALedgerNumber, AFeeCode, AGiftAmount, out AVerificationResult);
        }

        /// generated method from connector
        public System.Boolean PostGiftBatch(Int32 ALedgerNumber,
                                            Int32 ABatchNumber,
                                            out TVerificationResultCollection AVerifications)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "PostGiftBatch", ";INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.PostGiftBatch(ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// generated method from connector
        public Int32 ExportAllGiftBatchData(Hashtable requestParams,
                                            out String exportString,
                                            out TVerificationResultCollection AMessages)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "ExportAllGiftBatchData", ";HASHTABLE;STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.ExportAllGiftBatchData(requestParams, out exportString, out AMessages);
        }

        /// generated method from connector
        public System.Boolean ImportGiftBatches(Hashtable requestParams,
                                                String importString,
                                                out TVerificationResultCollection AMessages)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "ImportGiftBatches", ";HASHTABLE;STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.ImportGiftBatches(requestParams, importString, out AMessages);
        }

        /// generated method from connector
        public PPartnerTable LoadPartnerData(System.Int64 PartnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadPartnerData", ";LONG;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadPartnerData(PartnerKey);
        }

        /// generated method from connector
        public System.String IdentifyPartnerCostCentre(Int32 ledgerNumber,
                                                       Int64 fieldNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "IdentifyPartnerCostCentre", ";INT;LONG;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.IdentifyPartnerCostCentre(ledgerNumber, fieldNumber);
        }

        /// generated method from connector
        public Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable LoadKeyMinistry(Int64 partnerKey,
                                                                                 out Int64 fieldNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "LoadKeyMinistry", ";LONG;LONG;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.LoadKeyMinistry(partnerKey, out fieldNumber);
        }

        /// generated method from connector
        public Int64 SearchRecipientLedgerKey(Int64 partnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector), "SearchRecipientLedgerKey", ";LONG;");
            return Ict.Petra.Server.MFinance.Gift.WebConnectors.TTransactionWebConnector.SearchRecipientLedgerKey(partnerKey);
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.GL
{
    /// <summary>auto generated class </summary>
    public class TGLNamespace : MarshalByRefObject, IGLNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TGLUIConnectorsNamespace FGLUIConnectorsSubNamespace;
        private TGLWebConnectorsNamespace FGLWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TGLNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TGLNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TGLNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'GLUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IGLUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'GL.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'GL.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FGLUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TGLUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.GL.Instantiator.UIConnectors') should be automatically contructable.
                    FGLUIConnectorsSubNamespace = new TGLUIConnectorsNamespace();
                }

                return FGLUIConnectorsSubNamespace;
            }

        }

        /// <summary>The 'GLWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IGLWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'GL.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'GL.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FGLWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TGLWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.GL.Instantiator.WebConnectors') should be automatically contructable.
                    FGLWebConnectorsSubNamespace = new TGLWebConnectorsNamespace();
                }

                return FGLWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.GL.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TGLUIConnectorsNamespace : MarshalByRefObject, IGLUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TGLUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TGLUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TGLUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.GL.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TGLWebConnectorsNamespace : MarshalByRefObject, IGLWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TGLWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TGLWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TGLWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public System.Boolean GetCurrentPeriodDates(Int32 ALedgerNumber,
                                                    out DateTime AStartDate,
                                                    out DateTime AEndDate)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetCurrentPeriodDates", ";INT;DATETIME;DATETIME;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetCurrentPeriodDates(ALedgerNumber, out AStartDate, out AEndDate);
        }

        /// generated method from connector
        public System.Boolean GetCurrentPostingRangeDates(Int32 ALedgerNumber,
                                                          out DateTime AStartDateCurrentPeriod,
                                                          out DateTime AEndDateLastForwardingPeriod)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetCurrentPostingRangeDates", ";INT;DATETIME;DATETIME;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetCurrentPostingRangeDates(ALedgerNumber, out AStartDateCurrentPeriod, out AEndDateLastForwardingPeriod);
        }

        /// generated method from connector
        public System.Boolean GetRealPeriod(System.Int32 ALedgerNumber,
                                            System.Int32 ADiffPeriod,
                                            System.Int32 AYear,
                                            System.Int32 APeriod,
                                            out System.Int32 ARealPeriod,
                                            out System.Int32 ARealYear)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetRealPeriod", ";INT;INT;INT;INT;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetRealPeriod(ALedgerNumber, ADiffPeriod, AYear, APeriod, out ARealPeriod, out ARealYear);
        }

        /// generated method from connector
        public System.DateTime GetPeriodStartDate(System.Int32 ALedgerNumber,
                                                  System.Int32 AYear,
                                                  System.Int32 ADiffPeriod,
                                                  System.Int32 APeriod)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetPeriodStartDate", ";INT;INT;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetPeriodStartDate(ALedgerNumber, AYear, ADiffPeriod, APeriod);
        }

        /// generated method from connector
        public System.DateTime GetPeriodEndDate(Int32 ALedgerNumber,
                                                System.Int32 AYear,
                                                System.Int32 ADiffPeriod,
                                                System.Int32 APeriod)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetPeriodEndDate", ";INT;INT;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetPeriodEndDate(ALedgerNumber, AYear, ADiffPeriod, APeriod);
        }

        /// generated method from connector
        public System.Boolean GetPeriodDates(Int32 ALedgerNumber,
                                             Int32 AYearNumber,
                                             Int32 ADiffPeriod,
                                             Int32 APeriodNumber,
                                             out DateTime AStartDatePeriod,
                                             out DateTime AEndDatePeriod)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetPeriodDates", ";INT;INT;INT;INT;DATETIME;DATETIME;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetPeriodDates(ALedgerNumber, AYearNumber, ADiffPeriod, APeriodNumber, out AStartDatePeriod, out AEndDatePeriod);
        }

        /// generated method from connector
        public DataTable GetAvailableGLYears(Int32 ALedgerNumber,
                                             System.Int32 ADiffPeriod,
                                             System.Boolean AIncludeNextYear,
                                             out String ADisplayMember,
                                             out String AValueMember)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector), "GetAvailableGLYears", ";INT;INT;BOOL;STRING;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TAccountingPeriodsWebConnector.GetAvailableGLYears(ALedgerNumber, ADiffPeriod, AIncludeNextYear, out ADisplayMember, out AValueMember);
        }

        /// generated method from connector
        public System.Boolean TPeriodMonthEnd(System.Int32 ALedgerNum,
                                              System.Boolean AIsInInfoMode,
                                              out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TPeriodIntervallConnector), "TPeriodMonthEnd", ";INT;BOOL;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TPeriodIntervallConnector.TPeriodMonthEnd(ALedgerNum, AIsInInfoMode, out AVerificationResult);
        }

        /// generated method from connector
        public System.Boolean TPeriodYearEnd(System.Int32 ALedgerNum,
                                             System.Boolean AIsInInfoMode,
                                             out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TPeriodIntervallConnector), "TPeriodYearEnd", ";INT;BOOL;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TPeriodIntervallConnector.TPeriodYearEnd(ALedgerNum, AIsInInfoMode, out AVerificationResult);
        }

        /// generated method from connector
        public System.Boolean Revaluate(System.Int32 ALedgerNum,
                                        System.Int32 AAccoutingPeriod,
                                        System.String[] AForeignCurrency,
                                        System.Decimal[] ANewExchangeRate,
                                        out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TRevaluationWebConnector), "Revaluate", ";INT;INT;STRING.ARRAY;DECIMAL.ARRAY;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TRevaluationWebConnector.Revaluate(ALedgerNum, AAccoutingPeriod, AForeignCurrency, ANewExchangeRate, out AVerificationResult);
        }

        /// generated method from connector
        public GLBatchTDS CreateABatch(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "CreateABatch", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.CreateABatch(ALedgerNumber);
        }

        /// generated method from connector
        public GLBatchTDS LoadABatch(Int32 ALedgerNumber,
                                     TFinanceBatchFilterEnum AFilterBatchStatus,
                                     System.Int32 AYear,
                                     System.Int32 APeriod)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "LoadABatch", ";INT;TFINANCEBATCHFILTERENUM;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.LoadABatch(ALedgerNumber, AFilterBatchStatus, AYear, APeriod);
        }

        /// generated method from connector
        public GLBatchTDS LoadABatchAndContent(Int32 ALedgerNumber,
                                               Int32 ABatchNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "LoadABatchAndContent", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.LoadABatchAndContent(ALedgerNumber, ABatchNumber);
        }

        /// generated method from connector
        public GLBatchTDS LoadAJournal(Int32 ALedgerNumber,
                                       Int32 ABatchNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "LoadAJournal", ";INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.LoadAJournal(ALedgerNumber, ABatchNumber);
        }

        /// generated method from connector
        public GLBatchTDS LoadATransaction(Int32 ALedgerNumber,
                                           Int32 ABatchNumber,
                                           Int32 AJournalNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "LoadATransaction", ";INT;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.LoadATransaction(ALedgerNumber, ABatchNumber, AJournalNumber);
        }

        /// generated method from connector
        public GLBatchTDS LoadATransAnalAttrib(Int32 ALedgerNumber,
                                               Int32 ABatchNumber,
                                               Int32 AJournalNumber,
                                               Int32 ATransactionNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "LoadATransAnalAttrib", ";INT;INT;INT;INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.LoadATransAnalAttrib(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber);
        }

        /// generated method from connector
        public GLSetupTDS LoadAAnalysisAttributes(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "LoadAAnalysisAttributes", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.LoadAAnalysisAttributes(ALedgerNumber);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveGLBatchTDS(ref GLBatchTDS AInspectDS,
                                                   out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "SaveGLBatchTDS", ";GLBATCHTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.SaveGLBatchTDS(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public System.Boolean PostGLBatch(Int32 ALedgerNumber,
                                          Int32 ABatchNumber,
                                          out TVerificationResultCollection AVerifications)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "PostGLBatch", ";INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.PostGLBatch(ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// generated method from connector
        public List<TVariant> TestPostGLBatch(Int32 ALedgerNumber,
                                              Int32 ABatchNumber,
                                              out TVerificationResultCollection AVerifications)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "TestPostGLBatch", ";INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.TestPostGLBatch(ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// generated method from connector
        public System.String GetStandardCostCentre(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "GetStandardCostCentre", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(ALedgerNumber);
        }

        /// generated method from connector
        public System.Decimal GetDailyExchangeRate(System.String ACurrencyFrom,
                                                   System.String ACurrencyTo,
                                                   DateTime ADateEffective)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "GetDailyExchangeRate", ";STRING;STRING;DATETIME;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetDailyExchangeRate(ACurrencyFrom, ACurrencyTo, ADateEffective);
        }

        /// generated method from connector
        public System.Decimal GetCorporateExchangeRate(System.String ACurrencyFrom,
                                                       System.String ACurrencyTo,
                                                       DateTime AStartDate,
                                                       DateTime AEndDate)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "GetCorporateExchangeRate", ";STRING;STRING;DATETIME;DATETIME;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetCorporateExchangeRate(ACurrencyFrom, ACurrencyTo, AStartDate, AEndDate);
        }

        /// generated method from connector
        public System.Boolean CancelGLBatch(out GLBatchTDS AMainDS,
                                            Int32 ALedgerNumber,
                                            Int32 ABatchNumber,
                                            out TVerificationResultCollection AVerifications)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "CancelGLBatch", ";GLBATCHTDS;INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.CancelGLBatch(out AMainDS, ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// generated method from connector
        public System.Boolean ExportAllGLBatchData(ref ArrayList batches,
                                                   Hashtable requestParams,
                                                   out String exportString)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "ExportAllGLBatchData", ";ARRAYLIST;HASHTABLE;STRING;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.ExportAllGLBatchData(ref batches, requestParams, out exportString);
        }

        /// generated method from connector
        public System.Boolean ImportGLBatches(Hashtable requestParams,
                                              String importString,
                                              out TVerificationResultCollection AMessages)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector), "ImportGLBatches", ";HASHTABLE;STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.ImportGLBatches(requestParams, importString, out AMessages);
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.ICH
{
    /// <summary>auto generated class </summary>
    public class TICHNamespace : MarshalByRefObject, IICHNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TICHWebConnectorsNamespace FICHWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TICHNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TICHNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TICHNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'ICHWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IICHWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'ICH.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'ICH.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FICHWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TICHWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.ICH.Instantiator.WebConnectors') should be automatically contructable.
                    FICHWebConnectorsSubNamespace = new TICHWebConnectorsNamespace();
                }

                return FICHWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.ICH.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TICHWebConnectorsNamespace : MarshalByRefObject, IICHWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TICHWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TICHWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TICHWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public System.Boolean PerformStewardshipCalculation(System.Int32 ALedgerNumber,
                                                            System.Int32 APeriodNumber,
                                                            out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.ICH.WebConnectors.TStewardshipCalculationWebConnector), "PerformStewardshipCalculation", ";INT;INT;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.ICH.WebConnectors.TStewardshipCalculationWebConnector.PerformStewardshipCalculation(ALedgerNumber, APeriodNumber, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.PeriodEnd
{
    /// <summary>auto generated class </summary>
    public class TPeriodEndNamespace : MarshalByRefObject, IPeriodEndNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPeriodEndUIConnectorsNamespace FPeriodEndUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPeriodEndNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPeriodEndNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPeriodEndNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'PeriodEndUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPeriodEndUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PeriodEnd.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PeriodEnd.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPeriodEndUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPeriodEndUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PeriodEnd.Instantiator.UIConnectors') should be automatically contructable.
                    FPeriodEndUIConnectorsSubNamespace = new TPeriodEndUIConnectorsNamespace();
                }

                return FPeriodEndUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.PeriodEnd.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TPeriodEndUIConnectorsNamespace : MarshalByRefObject, IPeriodEndUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPeriodEndUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPeriodEndUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPeriodEndUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Reporting
{
    /// <summary>auto generated class </summary>
    public class TReportingNamespace : MarshalByRefObject, IReportingNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TReportingUIConnectorsNamespace FReportingUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TReportingNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TReportingNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TReportingNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'ReportingUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IReportingUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Reporting.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Reporting.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FReportingUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TReportingUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Reporting.Instantiator.UIConnectors') should be automatically contructable.
                    FReportingUIConnectorsSubNamespace = new TReportingUIConnectorsNamespace();
                }

                return FReportingUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Reporting.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TReportingUIConnectorsNamespace : MarshalByRefObject, IReportingUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        #region ManualCode

        /// <summary>holds reference to the Reporting UIConnector object (only once instantiated)</summary>
        private TFinanceReportingUIConnector FFinanceReportingUIConnector;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TReportingUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
            #region ManualCode
            FFinanceReportingUIConnector = new TFinanceReportingUIConnector();
            #endregion ManualCode
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TReportingUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TReportingUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public void SelectLedger(System.Int32 ALedgerNr)
        {
            #region ManualCode
            FFinanceReportingUIConnector.SelectLedger(ALedgerNr);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Data.DataTable GetReceivingFields(out System.String ADisplayMember,
                                                        out System.String AValueMember)
        {
            #region ManualCode
            return FFinanceReportingUIConnector.GetReceivingFields(out ADisplayMember, out AValueMember);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.String GetReportingCostCentres(String ASummaryCostCentreCode)
        {
            #region ManualCode
            return FFinanceReportingUIConnector.GetReportingCostCentres(ASummaryCostCentreCode);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Setup
{
    /// <summary>auto generated class </summary>
    public class TSetupNamespace : MarshalByRefObject, ISetupNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TSetupUIConnectorsNamespace FSetupUIConnectorsSubNamespace;
        private TSetupWebConnectorsNamespace FSetupWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TSetupNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TSetupNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSetupNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

        /// <summary>The 'SetupUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ISetupUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Setup.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Setup.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FSetupUIConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSetupUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Setup.Instantiator.UIConnectors') should be automatically contructable.
                    FSetupUIConnectorsSubNamespace = new TSetupUIConnectorsNamespace();
                }

                return FSetupUIConnectorsSubNamespace;
            }

        }

        /// <summary>The 'SetupWebConnectors' subnamespace contains further subnamespaces.</summary>
        public ISetupWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Setup.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Setup.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FSetupWebConnectorsSubNamespace == null)
                {
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSetupWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Setup.Instantiator.WebConnectors') should be automatically contructable.
                    FSetupWebConnectorsSubNamespace = new TSetupWebConnectorsNamespace();
                }

                return FSetupWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Setup.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TSetupUIConnectorsNamespace : MarshalByRefObject, ISetupUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TSetupUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TSetupUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSetupUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MFinance.Instantiator.Setup.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TSetupWebConnectorsNamespace : MarshalByRefObject, ISetupWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TSetupWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TSetupWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSetupWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public GLSetupTDS LoadAccountHierarchies(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "LoadAccountHierarchies", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.LoadAccountHierarchies(ALedgerNumber);
        }

        /// generated method from connector
        public GLSetupTDS LoadCostCentreHierarchy(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "LoadCostCentreHierarchy", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.LoadCostCentreHierarchy(ALedgerNumber);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveGLSetupTDS(Int32 ALedgerNumber,
                                                   ref GLSetupTDS AInspectDS,
                                                   out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "SaveGLSetupTDS", ";INT;GLSETUPTDS;TVERIFICATIONRESULTCOLLECTION;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.SaveGLSetupTDS(ALedgerNumber, ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public System.String ExportAccountHierarchy(Int32 ALedgerNumber,
                                                    System.String AAccountHierarchyName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "ExportAccountHierarchy", ";INT;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.ExportAccountHierarchy(ALedgerNumber, AAccountHierarchyName);
        }

        /// generated method from connector
        public System.String ExportCostCentreHierarchy(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "ExportCostCentreHierarchy", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.ExportCostCentreHierarchy(ALedgerNumber);
        }

        /// generated method from connector
        public System.Boolean ImportAccountHierarchy(Int32 ALedgerNumber,
                                                     System.String AHierarchyName,
                                                     System.String AXmlAccountHierarchy)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "ImportAccountHierarchy", ";INT;STRING;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.ImportAccountHierarchy(ALedgerNumber, AHierarchyName, AXmlAccountHierarchy);
        }

        /// generated method from connector
        public System.Boolean ImportCostCentreHierarchy(Int32 ALedgerNumber,
                                                        System.String AXmlHierarchy)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "ImportCostCentreHierarchy", ";INT;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.ImportCostCentreHierarchy(ALedgerNumber, AXmlHierarchy);
        }

        /// generated method from connector
        public System.Boolean ImportNewLedger(Int32 ALedgerNumber,
                                              System.String AXmlLedgerDetails,
                                              System.String AXmlAccountHierarchy,
                                              System.String AXmlCostCentreHierarchy,
                                              System.String AXmlMotivationDetails)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "ImportNewLedger", ";INT;STRING;STRING;STRING;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.ImportNewLedger(ALedgerNumber, AXmlLedgerDetails, AXmlAccountHierarchy, AXmlCostCentreHierarchy, AXmlMotivationDetails);
        }

        /// generated method from connector
        public System.Boolean CanDeleteAccount(Int32 ALedgerNumber,
                                               System.String AAccountCode)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "CanDeleteAccount", ";INT;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.CanDeleteAccount(ALedgerNumber, AAccountCode);
        }

        /// generated method from connector
        public System.Boolean CreateNewLedger(Int32 ANewLedgerNumber,
                                              String ALedgerName,
                                              String ACountryCode,
                                              String ABaseCurrency,
                                              String AIntlCurrency,
                                              DateTime ACalendarStartDate,
                                              Int32 ANumberOfPeriods,
                                              Int32 ACurrentPeriod,
                                              Int32 ANumberOfFwdPostingPeriods,
                                              out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "CreateNewLedger", ";INT;STRING;STRING;STRING;STRING;DATETIME;INT;INT;INT;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.CreateNewLedger(ANewLedgerNumber, ALedgerName, ACountryCode, ABaseCurrency, AIntlCurrency, ACalendarStartDate, ANumberOfPeriods, ACurrentPeriod, ANumberOfFwdPostingPeriods, out AVerificationResult);
        }

        /// generated method from connector
        public ALedgerTable GetAvailableLedgers()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "GetAvailableLedgers", ";");
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.GetAvailableLedgers();
        }

        /// generated method from connector
        public AFreeformAnalysisTable LoadAFreeformAnalysis(Int32 ALedgerNumber)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "LoadAFreeformAnalysis", ";INT;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.LoadAFreeformAnalysis(ALedgerNumber);
        }

        /// generated method from connector
        public System.Int32 CheckDeleteAFreeformAnalysis(Int32 ALedgerNumber,
                                                         String ATypeCode,
                                                         String AAnalysisValue)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "CheckDeleteAFreeformAnalysis", ";INT;STRING;STRING;", ALedgerNumber);
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.CheckDeleteAFreeformAnalysis(ALedgerNumber, ATypeCode, AAnalysisValue);
        }

        /// generated method from connector
        public System.Int32 CheckDeleteAAnalysisType(String ATypeCode)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector), "CheckDeleteAAnalysisType", ";STRING;");
            return Ict.Petra.Server.MFinance.Setup.WebConnectors.TGLSetupWebConnector.CheckDeleteAAnalysisType(ATypeCode);
        }
    }
}

