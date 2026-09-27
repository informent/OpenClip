$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$out = Join-Path $root 'test-output'
dotnet publish (Join-Path $root 'OpenClip.csproj') -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o $out --nologo | Out-Host
$exe = Join-Path $out 'OpenClip.exe'; if (!(Test-Path -LiteralPath $exe)) { throw 'OpenClip executable missing' }
$p = Start-Process -FilePath $exe -PassThru; Start-Sleep -Milliseconds 1200; if ($p.HasExited) { throw "OpenClip exited with code $($p.ExitCode)" }; Stop-Process -Id $p.Id -Force; Write-Output 'PASS: build, publish, and launch (OpenClip)'
