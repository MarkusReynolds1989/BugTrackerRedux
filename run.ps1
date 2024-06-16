$backend = "./src/BugTrackerRedux"
$frontend = "./src/Frontend"

$start_backend = "cd $backend; dotnet watch run"
$start_frontend = "cd $frontend; dotnet watch run"

Start-Process powershell -ArgumentList "-NoExit -Command $start_backend"
Start-Process powershell -ArgumentList "-NoExit -Command $start_frontend"
