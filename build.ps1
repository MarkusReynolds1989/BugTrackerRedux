$directory = pwd

cd src
"`n=============================================="
"Cleaning..."
dotnet clean /tl
"`n==============================================`n"

"`n`n=============================================="
"Building..."
dotnet build /tl
"`n==============================================`n"

"`n`n=============================================="
"Testing..."
dotnet test /tl
"`n==============================================`n"

cd $directory
