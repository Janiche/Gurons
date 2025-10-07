using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class Notification
{
    public int _identifier = -99;
    public string _title = string.Empty;
    public string _content = string.Empty;
    public DateTime? _fireTime = null;
    public string _intentData = string.Empty;
    public TimeSpan? _repeatInterval = null;

    public Notification() { }

    public Notification(string title, string content)
    {
        _title = title;
        _content = content;
    }

    public Notification(string title, string content, DateTime fireTime)
    {
        _title = title;
        _content = content;
        _fireTime = fireTime;
    }

    public Notification(string title, string content, DateTime fireTime, string intentData)
    {
        _title = title;
        _content = content;
        _fireTime = fireTime;
        _intentData = intentData;
    }

    public Notification(string title, string content, DateTime fireTime, TimeSpan repeatInterval)
    {
        _title = title;
        _content = content;
        _fireTime = fireTime;
        _repeatInterval = repeatInterval;
    }

    public Notification(string title, string content, DateTime fireTime, string intentData, TimeSpan repeatInterval)
    {
        _title = title;
        _content = content;
        _fireTime = fireTime;
        _intentData = intentData;
        _repeatInterval = repeatInterval;
    }

    public Notification(int id, string title, string content, DateTime fireTime)
    {
        _identifier = id;
        _title = title;
        _content = content;
        _fireTime = fireTime;
    }

    public Notification(int id, string title, string content, DateTime fireTime, string intentData)
    {
        _identifier = id;
        _title = title;
        _content = content;
        _fireTime = fireTime;
        _intentData = intentData;
    }

    public Notification(int id, string title, string content, DateTime fireTime, TimeSpan repeatInterval)
    {
        _identifier = id;
        _title = title;
        _content = content;
        _fireTime = fireTime;
        _repeatInterval = repeatInterval;
    }

    public Notification(int id, string title, string content, DateTime fireTime, string intentData, TimeSpan repeatInterval)
    {
        _identifier = id;
        _title = title;
        _content = content;
        _fireTime = fireTime;
        _intentData = intentData;
        _repeatInterval = repeatInterval;
    }
}
