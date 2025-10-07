using System;
using System.Collections.Generic;

/// <summary>
/// WordingMaster
/// </summary>
[Serializable]
public class WordingData
{
    public string version;
    public List<WordingEntry> data;

    [Serializable]
    public class WordingEntry
    {
        public string key;
        public string value;
    }
}