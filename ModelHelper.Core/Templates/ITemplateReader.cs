﻿using System.Runtime.InteropServices;

namespace ModelHelper.Core.Templates
{
    public interface ITemplateReader
    {
        ITemplate Read(string path);
    }
}