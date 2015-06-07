using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigReader
{
    private const string ConfigFileName = @"SuperTrackMayhem.config";
    private static ConfigReader _instance;
    private readonly Dictionary<string, string> fileContent = new Dictionary<string, string>();

    private ConfigReader()
    {
        if (!File.Exists(ConfigFileName))
        {
            Debug.LogWarning("Config file not found, creating default port");
        }

        var lines = 0;

        // We are using Key = Value format in the config file
        foreach (var line in File.ReadAllLines(ConfigFileName))
        {
            lines++;
            if (line.StartsWith("//"))
            {
                continue;
            }

            var parts = line.Split('=');
            if (parts.Length != 2)
            {
                Debug.LogWarning("Config file syntax error on line " + lines + ": \n" + line);
                continue;
            }

            fileContent[parts[0].Trim()] = parts[1].Trim();
        }
    }

    private static ConfigReader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConfigReader();
            }
            return _instance;
        }
    }

    public static bool TryGetValue(string key, out string value)
    {
        return Instance.fileContent.TryGetValue(key, out value);
    }

    public static bool TryGetInt(string key, out int value)
    {
        string val;
        if (!TryGetValue(key, out val))
        {
            value = 0;
            return false;
        }

        return Int32.TryParse(val, out value);
    }
}