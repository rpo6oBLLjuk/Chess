using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DeskSaverService : BaseSaver<SerializableArray>
{
    public List<string> GetAllSaves()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _baseDirectory);
        if (!Directory.Exists(fullPath))
            return null;

        return Directory.GetFiles(fullPath, "*.json")
                   .Select(Path.GetFileNameWithoutExtension)
                   .ToList();
    }
}

[Serializable]
public struct SerializableArray
{
    public byte[] array;

    public SerializableArray(byte[] array) => this.array = array;
}
