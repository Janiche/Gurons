using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    #region DELEGATES

    public delegate void NotificationEvent(string intent);
    public static NotificationEvent OnReceiveNotification;
    public void ReceiveNotification(string intent)
    {
        OnReceiveNotification?.Invoke(intent);
    }

    #endregion

    #region VARIABLES

    List<Notification> _notifications = new List<Notification>();
    int _idCount = 0;
    string _channelId = string.Empty;
    private bool _initialized = false;

    private static NotificationManager Instance;
    public static NotificationManager _instance
    {
        get { return Instance; }
    }

    AndroidNotificationChannel _notificationChannel = new AndroidNotificationChannel();

    #endregion


    public bool _IsInitialized()
    {
        return _initialized;
    }

    public void _Init()
    {
        if (!_initialized)
        {
            _initialized = true;

            _CreateChannel("gurons_id", "gurons_Channel", "Channel used for gurons notification");

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            _CancelAllNotifications();
        }
    }

    private void _CreateChannel(string _id = "channel_id", string _name = "defaulf_channel", string _description = "Generic notifications")
    {
        _channelId = _id;

        _notificationChannel = new AndroidNotificationChannel()
        {
            Id = _id,
            Name = _name,
            Importance = Importance.High,
            Description = _description,
            EnableVibration = true,
            CanShowBadge = true,
            LockScreenVisibility = LockScreenVisibility.Public,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(_notificationChannel);
        AndroidNotificationCenter.OnNotificationReceived += _ReceivedNotificationHandler;
    }

    public int _SendNotification(Notification _notification)
    {

        Notification _not = _notifications.Find(_n => _n._identifier == _notification._identifier);

        if (_not != null)
        {
            _UpdateNotification(_notification);
            return 0;
        }

        //Debug.LogError($"{_notification._title}: {_notification._content} -> {_notification._fireTime}");

        //CREATES ANDROID NOTIFICATION
        var _androidNotification = new AndroidNotification();
        _androidNotification.Title = _notification._title;
        _androidNotification.Text = _notification._content;
        _androidNotification.SmallIcon = "small_icon";
        _androidNotification.LargeIcon = "large_icon";

        if (_notification._fireTime != null)
            _androidNotification.FireTime = (DateTime)_notification._fireTime;

        if (_notification._intentData != null)
            _androidNotification.RepeatInterval = _notification._repeatInterval;

        if (!string.IsNullOrEmpty(_notification._intentData))
            _androidNotification.IntentData = _notification._intentData;



        int _identifier = AndroidNotificationCenter.SendNotification(_androidNotification, _channelId);
        //int _identifier = 0;

        //CREATES NOTIFICATION
        _notification._identifier = _identifier;
        _notifications.Add(_notification);
        _idCount++;

        return _identifier;
    }

    public void _SendNotifications(List<Notification> notifications)
    {
        int _count = _notifications.Count;

        for (int i = 0; i < _count; i++)
        {
            _SendNotification(notifications[i]);
        }
    }

    public void _UpdateNotification(Notification _notification)
    {
        //UPDATES NOTIFICATION
        int _id = _notification._identifier;
        int _index = -10;

        _index = _notifications.FindIndex(n => n._identifier == _id);

        if (_index < 0)
            return;

        _notifications[_index] = _notification;

        //UPDATES ANDROID NOTIFICATION
        AndroidNotification _androidNotification = new AndroidNotification();
        _androidNotification.Title = _notification._title;
        _androidNotification.Text = _notification._content;
        _androidNotification.SmallIcon = "small_icon";
        _androidNotification.LargeIcon = "large_icon";

        if (_notification._fireTime != null)
            _androidNotification.FireTime = (DateTime)_notification._fireTime;

        if (_notification._repeatInterval != null)
            _androidNotification.RepeatInterval = (TimeSpan)_notification._repeatInterval;

        if (!string.IsNullOrEmpty(_notification._intentData))
            _androidNotification.IntentData = _notification._intentData;

        AndroidNotificationCenter.UpdateScheduledNotification(_id, _androidNotification, _channelId);
    }

    public void _CancelNotification(int _id)
    {
        AndroidNotificationCenter.CancelNotification(_id);

        int _not = _notifications.FindIndex(n => n._identifier == _id);
        _notifications.RemoveAt(_not);

        _idCount = _notifications.Count;
    }

    public void _CancelNotification(Notification _notification)
    {
        Notification _not = _notifications.Find(_n => _n._title == _notification._title);

        if (_not != null)
        {
            if (_not._identifier != -99)
            {
                _CancelNotification(_not._identifier);
            }
        }
    }

    private void _DeleteNotification(int _id)
    {
        int _not = _notifications.FindIndex(n => n._identifier == _id);
        _notifications.RemoveAt(_not);

        _idCount = _notifications.Count;
    }

    public void _CancelAllNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        _notifications.Clear();
        _idCount = 0;
    }

    public bool _CheckNotificationByTitle(string _title)
    {
        Notification notification = _notifications.Find(n => n._title == _title);

        return (notification != null);
    }

    public bool _CheckNotificationById(int _id)
    {
        Notification notification = _notifications.Find(n => n._identifier == _id);

        if (notification != null)
            return true;

        else
            return false;
    }

    public string _GetIntentDataById(int _id)
    {
        Notification _notification = _notifications.Find(n => n._identifier == _id);

        if (_notification != null)
        {
            return _notification._intentData;
        }

        return string.Empty;

    }

    public string _GetIntentDataByTitle(string _title)
    {
        Notification _notification = _notifications.Find(n => n._title == _title);

        if (_notification != null)
        {
            return _notification._intentData;
        }

        return string.Empty;

    }

    public string _GetLastIntent()
    {
        AndroidNotificationIntentData _not = AndroidNotificationCenter.GetLastNotificationIntent();

        if (_not != null)
        {
            int _id = _not.Id;
            string intent = _not.Notification.IntentData;
            return intent;
        }
        return string.Empty;
    }

    private void _ReceivedNotificationHandler(AndroidNotificationIntentData data)
    {
        if (data != null)
        {
            int _identifier = data.Id;
            ReceiveNotification(data.Notification.IntentData);

            _DeleteNotification(_identifier);
        }
    }

    //    private void OnApplicationPause(bool pause)
    //    {
    //        if (_idCount > 0)
    //        {
    //#if !UNITY_EDITOR


    //                    for (int i = 0; i < _idCount; i++)
    //                    {
    //                        int _identifier = _notifications[i]._identifier;

    //                        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(_identifier) == NotificationStatus.Scheduled)
    //                        {
    //                            _UpdateNotification(_notifications[i]);
    //                            // Replace the currently scheduled notification with a new notification.
    //                            //_UpdateNotification(_identifiers[i], )
    //                            //AndroidNotificationCenter.UpdateScheduledNotification(_identifier, newNotification, _channelId);
    //                        }
    //                        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(_identifier) == NotificationStatus.Delivered)
    //                        {
    //                            //Remove the notification from the status bar
    //                            _CancelNotification(_identifier);
    //                            //AndroidNotificationCenter.CancelNotification(_identifier);
    //                        }
    //                        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(_identifier) == NotificationStatus.Unknown)
    //                        {
    //                            Notification _notification = _notifications[i];
    //                            _notifications.RemoveAt(i);
    //                            _SendNotification(_notification);
    //                            //AndroidNotificationCenter.SendNotification(newNotification, _channelId);
    //                        }
    //                    }
    //#endif
    //        }
    //    }

}


