:: make db and its structure, expects firebird 2.5.x installed in c:\fb
:: https://stackoverflow.com/questions/1601947/how-do-i-create-a-new-firebird-database-from-the-command-line

del d:\mtnc.fdb

SET isql=c:\fb\bin\isql.exe

%isql% -input mtnc.base.sql
%isql% -input mtnc.users.sql
%isql% -input mtnc.objects.sql