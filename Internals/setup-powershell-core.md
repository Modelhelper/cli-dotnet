1) 
```powershell
Install-Module posh-git -Scope CurrentUser
Install-Module oh-my-posh -Scope CurrentUser
```

```powershell
Install-Module -Name PSReadLine -AllowPrerelease -Scope CurrentUser -Force -SkipPublisherCheck
```

To enable the engine edit your PowerShell profile:

```powershell
if (!(Test-Path -Path $PROFILE )) { New-Item -Type File -Path $PROFILE -Force }
notepad $PROFILE
```

```
iex "& { $(irm https://aka.ms/install-powershell.ps1) } -UseMSI -Preview"'
```

Agnoster
Agnoster Theme

Paradox
Paradox Theme

Sorin
Sorin Theme

Darkblood
Darkblood Theme

Avit
Avit Theme

Honukai
Honukai Theme

Fish
Fish Theme

Robbyrussell
Robbyrussell Theme

