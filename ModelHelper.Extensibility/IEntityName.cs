﻿namespace ModelHelper.Extensibility
{
    public interface IEntityName
    {
        string Schema { get; set; }
        string Name { get; set; }

        string Type { get; set; }
    }
}