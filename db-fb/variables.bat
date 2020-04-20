:: set common variables

@IF EXIST "c:\fb\bin" (
  @SET ISQL="c:\fb\bin\isql.exe"
)
ELSE (
  @SET ISQL="c:\fb\isql.exe"
)
