sqlite3.dll has been downloaded from sqlite.org

Mono.Data.Sqlite is from C:\Program Files (x86)\Mono-2.10.6\lib\mono\2.0
Version for .net 4.0 would not work on Windows. see also http://bugzilla.xamarin.com/show_bug.cgi?id=2148
Unhandled Exception: System.TypeLoadException: Inheritance security rules
violated by type: 'Mono.Data.Sqlite.SqliteConnectionHandle'. Derived types must
either match the security accessibility of the base type or be less accessible.

other problem: cannot open on Windows?
http://bugzilla.xamarin.com/show_bug.cgi?id=152

see also http://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki
The problem is that those dlls do not seem to work on Linux, I can't get them to work (November 2011).