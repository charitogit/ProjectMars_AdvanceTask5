 

public class NotificationData
{
    public UserData UserA { get; set; }   // Requester
    public UserData UserB { get; set; }   // Receiver
    public SkillTradeRequestData Request { get; set; }

    public NotificationInfo Notification { get; set; }  
}

public class UserData
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SkillTradeRequestData
{
    public string Title { get; set; }
    public string Message { get; set; }
    public int Hours { get; set; }
}

public class NotificationInfo
{
    public string Header { get; set; }
    public string Content { get; set; }
    public string PopUpMessage { get; set; }
}

