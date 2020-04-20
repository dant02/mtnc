:: make db and its structure, expects firebird 2.5.x installed in c:\fb
:: https://stackoverflow.com/questions/1601947/how-do-i-create-a-new-firebird-database-from-the-command-line

call .\variables.bat

del d:\fbdata\mtnc.fdb

%isql% -b -e -q -charset UTF8 -u SYSDBA -p masterkey -i mtnc.base.sql

::%isql% -b -e -q -charset UTF8 -u SYSDBA -p masterkey -i mtnc.objects.sql "d:\fbdata\mtnc.fdb"

::%isql% -input mtnc.objects.sql