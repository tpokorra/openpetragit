//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ict.Tools.OpenPetraWebServer
{
    class CommandLineArgs
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// The command line can be blank - in which case the full UI is used
        /// or it can have parameters as follows - in which case the simplified UI is shown with one instance per port
        ///
        /// "path-to-exe" "fully-qualified-physical-path" /p portNumber [/v virtualPath] [/d defaultPage] [/r] [/q]


        private string _physicalPath = string.Empty;
        private string _virtualPath = string.Empty;
        private int _port = -1;
        private bool _acceptRemoteConnection = false;
        private string _defaultPage = string.Empty;
        private bool _suppressStartUpMessages = false;

        public string PhysicalPath {
            get
            {
                return _physicalPath;
            }
        }
        public string VirtualPath {
            get
            {
                return _virtualPath;
            }
        }
        public int Port {
            get
            {
                return _port;
            }
        }
        public bool AcceptRemoteConnection {
            get
            {
                return _acceptRemoteConnection;
            }
        }
        public string DefaultPage {
            get
            {
                return _defaultPage;
            }
        }
        public bool SuppressStartupMessages {
            get
            {
                return _suppressStartUpMessages;
            }
        }


        public CommandLineArgs(string[] args)
        {
            char nextArg = ' ';

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                int tryInt = -1;

                switch (nextArg)
                {
                    case 'v':
                        _virtualPath = arg;
                        nextArg = ' ';
                        break;

                    case 'd':
                        _defaultPage = arg;
                        nextArg = ' ';
                        break;

                    case 'p':

                        if (Int32.TryParse(arg, out tryInt) && (tryInt >= 80) && (tryInt <= 65535))
                        {
                            _port = tryInt;
                        }

                        nextArg = ' ';
                        break;

                    default:

                        if (arg.StartsWith("/"))
                        {
                            if (arg[1] == 'r')
                            {
                                _acceptRemoteConnection = true;
                            }
                            else if (arg[1] == 'q')
                            {
                                _suppressStartUpMessages = true;
                            }
                            else
                            {
                                nextArg = arg[1];
                            }
                        }
                        else
                        {
                            // this must be the physical path
                            _physicalPath = arg;
                        }

                        break;
                }
            }
        }

        public bool IsValid
        {
            get
            {
                bool isValid = (_physicalPath != string.Empty && _port >= 80);

                if (isValid)
                {
                    if (!Directory.Exists(_physicalPath))
                    {
                        return false;
                    }
                }

                return isValid;
            }
        }
    }
}