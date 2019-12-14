1: Console  (opt-[key] value)
2: source.connection.group.options
2: source.connection.options
3: code.options
4: project.options
5: local.options
6: global.options

Using the ExpandoObject

options.FolderName => value

# C# 8

## Absolutt
Nullable Reference Type - viktig... (if noe is null | ikke if noe == null)
Await foreach på generering av template
Default Interface members
Patterns... (ny switch) - ny pattern mathcing
    if (person?.MiddleName is { } middle) return middle.Length;
    if (person?.MiddleName is { Length: var length } ) return length;
    return person?.MiddleName is { Length: var length } ? length : 0;

Indices and Ranges
Null Coalescing Assignment ( numbers ??= new List<int>())

## kanskje
using statement

local static

## sjekk ut
Interpolated verbatim strings
Unmanaged Constraint
