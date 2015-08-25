using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for IEditableTabstrip
/// </summary>
public interface IEditableTabstrip
{
    Dictionary<string, string> Tabs { get; set; }
}
