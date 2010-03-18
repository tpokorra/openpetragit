﻿/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Web;

namespace Ict.Common.IO
{
    /// <summary>
    /// a few simple functions to access content from the web
    /// </summary>
    public class THTTPUtils
    {
        /// <summary>
        /// read from a website;
        /// used to check for available patches
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReadWebsite(string url)
        {
            string ReturnValue;

            byte[] buf;
            WebClient client;
            client = new WebClient();
            ReturnValue = null;
            try
            {
                buf = client.DownloadData(url);

                if ((buf != null) && (buf.Length > 0))
                {
                    ReturnValue = Encoding.ASCII.GetString(buf, 0, buf.Length);
                }
            }
            catch (System.Net.WebException e)
            {
                TLogging.Log("Trying to download: " + url + Environment.NewLine +
                    e.Message, TLoggingType.ToLogfile);
            }
            finally
            {
            }
            return ReturnValue;
        }

        /// <summary>
        /// overload: encode all the values for the parameters and retrieve the website
        /// </summary>
        public static string ReadWebsite(string url, SortedList <string, string>AParameters)
        {
            string urlWithParameters = url;

            bool firstParameter = true;

            foreach (string parameterName in AParameters.Keys)
            {
                if (firstParameter)
                {
                    urlWithParameters += "?";
                    firstParameter = false;
                }
                else
                {
                    urlWithParameters += "&";
                }

                urlWithParameters += parameterName + "=" + HttpUtility.UrlEncode(AParameters[parameterName]);
            }

            return ReadWebsite(urlWithParameters);
        }

        /// <summary>
        /// download a patch or other file from a website;
        /// used for patching the program
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Boolean DownloadFile(string url, string filename)
        {
            Boolean ReturnValue;
            WebClient client;

            client = new WebClient();
            ReturnValue = false;
            try
            {
                client.DownloadFile(url, filename);
                ReturnValue = true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message + " url: " + url + " filename: " + filename);
            }
            return ReturnValue;
        }
    }
}