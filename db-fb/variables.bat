:: set common variables

@IF EXIST "c:\fb\bin" (
  @SET ISQL="c:\fb\bin\isql.exe"
)

@IF EXIST "c:\fb\isql.exe" (
  @SET ISQL="c:\fb\isql.exe"
)
