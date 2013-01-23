// Auto generated by nant generateGlue
// From a template at inc\template\src\ClientServerGlue\ServerGlue.cs
//
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using System;
using System.Threading;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using System.ServiceModel;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core.Security;
{#USINGNAMESPACES}

namespace Ict.Petra.Server.app.WebService
{
/// <summary>
/// this publishes the SOAP web services of OpenPetra.org for module {#TOPLEVELMODULE}
/// </summary>
[WebService(Namespace = "http://www.openpetra.org/webservices/M{#TOPLEVELMODULE}")]
[ScriptService]
public class TM{#TOPLEVELMODULE}WebService : System.Web.Services.WebService
{
    private static SortedList<string, object> FUIConnectors = new SortedList<string, object>();

    /// <summary>
    /// constructor, which is called for each http request
    /// </summary>
    public TM{#TOPLEVELMODULE}WebService() : base()
    {
        TOpenPetraOrgSessionManager.Init();
    }

    {#WEBCONNECTORS}

    {#UICONNECTORS}
}
}

{##WEBCONNECTOR}
/// web connector method call
[WebMethod(EnableSession = true)]
public {#RETURNTYPE} {#WEBCONNECTORCLASS}_{#UNIQUEMETHODNAME}({#PARAMETERDEFINITION})
{
    {#CHECKUSERMODULEPERMISSIONS}
    try
    {
        {#LOCALVARIABLES}
        {#LOCALRETURN}{#WEBCONNECTORCLASS}.{#METHODNAME}({#ACTUALPARAMETERS});
        {#RETURN}
    }
    catch (Exception e)
    {
        TLogging.Log(e.ToString());
        throw new Exception("Please check server log file");
    }
}

{##CHECKUSERMODULEPERMISSIONS}
TModuleAccessManager.CheckUserPermissionsForMethod(typeof({#CONNECTORWITHNAMESPACE}), "{#METHODNAME}", "{#PARAMETERTYPES}"{#LEDGERNUMBER});

{##UICONNECTOR}
/// create a new UIConnector
[WebMethod(EnableSession = true)]
public System.String Create_{#CREATEMETHODNAME}({#PARAMETERDEFINITION})
{
    {#CHECKUSERMODULEPERMISSIONS}
    
    System.Guid ObjectID = new Guid();
    FUIConnectors.Add(ObjectID.ToString() + " " + GClientID.ClientID, new {#UICONNECTORCLASS}({#ACTUALPARAMETERS}));
    
    return ObjectID.ToString();
}