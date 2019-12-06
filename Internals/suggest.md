Install:

```cmd
dotnet tool install --global dotnet-suggest --version 1.1.19174.3
```

in .bash_profile add

```bash
# dotnet suggest shell complete script start
_dotnet_bash_complete()
{
    local fullpath=`type -p ${COMP_WORDS[0]}`
    local escaped_comp_line=$(echo "$COMP_LINE" | sed s/\"/'\\\"'/g)
    local completions=`dotnet-suggest get --executable "${fullpath}" --position ${COMP_POINT} -- "${escaped_comp_line}"`

    if [ "${#COMP_WORDS[@]}" != "2" ]; then
        return
    fi

    local IFS=$'\n'
    local suggestions=($(compgen -W "$completions"))

    if [ "${#suggestions[@]}" == "1" ]; then
        local number="${suggestions[0]/%\ */}"
        COMPREPLY=("$number")
    else
        for i in "${!suggestions[@]}"; do
            suggestions[$i]="$(printf '%*s' "-$COLUMNS"  "${suggestions[$i]}")"
        done

        COMPREPLY=("${suggestions[@]}")
    fi
}

complete -F _dotnet_bash_complete `dotnet-suggest list`
export DOTNET_SUGGEST_SCRIPT_VERSION="1.0.0"
# dotnet suggest shell complete script end
```

in powershell:
```powershell
  
# dotnet suggest shell start
$availableToComplete = (dotnet-suggest list) | Out-String
$availableToCompleteArray = $availableToComplete.Split([Environment]::NewLine, [System.StringSplitOptions]::RemoveEmptyEntries) 


    Register-ArgumentCompleter -Native -CommandName $availableToCompleteArray -ScriptBlock {
        param($commandName, $wordToComplete, $cursorPosition)
        $fullpath = (Get-Command $wordToComplete.CommandElements[0]).Source

        $arguments = $wordToComplete.Extent.ToString().Replace('"', '\"')
        dotnet-suggest get -e $fullpath --position $cursorPosition -- "$arguments" | ForEach-Object {
            [System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
        }
    }
$env:DOTNET_SUGGEST_SCRIPT_VERSION = "1.0.0"
# dotnet suggest script end
```