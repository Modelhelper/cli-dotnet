using System;

namespace ModelHelper
{
    [Obsolete]
    public interface ICommandData
    {
        string Key { get; }
        string Alias { get; }
    }
}