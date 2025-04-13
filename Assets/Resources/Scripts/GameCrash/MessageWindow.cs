using System;
using WBG;

public static class MessageWindow
{
    public static void ShowDirectXError()
    {
        var caller = new MessageBoxCaller();
        
        string errorText =
            "DirectX function \"adapter-Enum Outputs(0, ppOutput)\" failed with DXGI_ERROR NOT FOUND (\"When calling IDXGIObject::GetPrivateData, the GUID passed in is not recognized as one previously passed to IDXGIObject::SetPrivateData or IDXGIObject::SetPrivateDataInterface.\n" +
            "When calling IDXGIFactory:: EnumAdapters or IDXGIAdapter: EnumOutputs, the enumerated ordinal is out of range.\"). GPU: \"Microsoft Basic Render Driver\",\n" +
            "Driver: 20.19.0015.4454 04-05-2016 05:30:00";
        var answer =  caller.Message_Box(errorText, "DirectX error", WindowOptions.AbortRetryIgnore);

        while (answer == WindowResult.Retry || answer == WindowResult.Ignore)
        { 
            answer =  caller.Message_Box(errorText, "DirectX error", WindowOptions.AbortRetryIgnore);
        }
    }

    public static void ShowRestartMessage()
    {
        MessageWindow.ShowBufferOverrunError();
        
        MessageBoxCaller mb = new MessageBoxCaller();

        mb.Message_Box("You are using old version of the game\nPlease restart it to get latest version", "Error",
            WindowOptions.OK);
    }

    public static void ShowStackOverflowError()
    {
        var caller = new MessageBoxCaller();

        string caption = "Runtime error";
        string errorText = "Error 92\n\nProgram:\n\nInsufficient stack to continue executing the program safely.\n\n";
        var answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);

        while (answer == WindowResult.Retry || answer == WindowResult.Ignore)
        { 
            answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);
        }
    }
    public static void ShowBufferOverrunError()
    {
        var caller = new MessageBoxCaller();

        string caption = "Runtime error";
        string errorText = "Error 73\n\nProgram:\n\nA buffer overrun has been detected which has corrupted the program's internal state" +
                           "The program cannot safely continue execution and must now be terminated";
        var answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);

        while (answer == WindowResult.Retry || answer == WindowResult.Ignore)
        { 
            answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);
        }
    }

    public static void EndOfGame()
    {
    MessageBoxCaller mb = new MessageBoxCaller();

     DataSaver.Data.Statistics stats = LoadManager.Instance.Data.Stats;
     DateTime now = DateTime.Now;
     DateTime start = stats.StartTime;
     TimeSpan elapsed = now - start;
     
     string message = "This is the end of the ACTUAL game.\n" +
                      "Thanks for playing\n" +
                      $"You have died {stats.Deaths} times\n" +
                      $"You have jumped {stats.Jumps} times\n" +
                      $"You have dashed {stats.Dashes} times\n" +
                      $"Real time passed: " +
                      (elapsed.Days > 0 ? $"{elapsed.Days} days, " : "") + 
                      (elapsed.Hours > 0 ? $"{elapsed.Hours} hours, " : "") +
                      (elapsed.Minutes > 0 ? $"{elapsed.Minutes} minutes, " : "") +
                      $"{elapsed.Seconds} seconds, {elapsed.Milliseconds} milliseconds";

     mb.Message_Box(message, "You beat the game!", WindowOptions.OK);
    }

    public static void ShowResetingMessage()
    {
     MessageBoxCaller mb = new MessageBoxCaller();

     string message = "Game is being reset\n" +
                      "Game will be closed now.";
     mb.Message_Box(message,"Reset",
             WindowOptions.OK);
    }

    public static void TextureNotFound()
    {
var caller = new MessageBoxCaller();

        string caption = "Runtime error";
        string errorText = "Error 784\n\nProgram:\n\n" +
                           "Texture Not Found (32)";
        var answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);

        while (answer == WindowResult.Retry || answer == WindowResult.Ignore)
        { 
            answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);
        }
    }

    public static void NoControllerFound()
    {
var caller = new MessageBoxCaller();
        string caption = "Runtime error";
        string errorText = "Error 388\n\nProgram:\n\n" +
                           "Controller Not Found (0)";
        var answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);

        while (answer == WindowResult.Retry || answer == WindowResult.Ignore)
        { 
            answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);
        }
    }

    public static void ShowOrphanError()
    {
        var caller = new MessageBoxCaller();
        string caption = "Runtime error";
        string errorText = "Error 443\n\nProgram:\n\n" +
                           "Object name: \"Orphan\", access property \"Parent\" -> error:\n"+
                           "AccessViolationException";
        var answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);

        while (answer == WindowResult.Retry || answer == WindowResult.Ignore)
        { 
            answer =  caller.Message_Box(errorText, caption, WindowOptions.AbortRetryIgnore);
        }
    }
}