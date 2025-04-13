using System;
using System.Runtime.InteropServices;
 
namespace WBG
{
    /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-messagebox</see>
    public class MessageBoxCaller
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr GetActiveWindow();
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern int MessageBox(IntPtr hwnd, String lpText, String lpCaption, uint uType);
 
        public static System.IntPtr GetWindowHandle()
        {
            return GetActiveWindow();
        }
 
        /// <summary>
        /// Shows Message Box with button type.
        /// </summary>
        /// <param name="text">Main alert text / content.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of message / icon to use - </param>
        /// <remarks>types: AbortRetryIgnore, CancelTryContinue, Help, OK, OkCancel, RetryCancel, YesNo, YesNoCancel</remarks>
        /// <example>Message_Box("My Text Message", "My Title", "OK");</example>
        /// <returns>OK,CANCEL,ABORT,RETRY, IGNORE, YES, NO, TRY AGAIN</returns>
        public WindowResult Message_Box(string text, string caption, WindowOptions type)
        {
            try
            {
                string DialogResult = string.Empty;
                uint MB_ABORTRETRYIGNORE = (uint)(0x00000002L | 0x00000010L);
                uint MB_CANCELTRYCONTINUE = (uint)(0x00000006L | 0x00000030L);
                uint MB_HELP = (uint)(0x00004000L | 0x00000040L);
                uint MB_OK = (uint)(0x00000000L | 0x00000040L);
                uint MB_OKCANCEL = (uint)(0x00000001L | 0x00000040L);
                uint MB_RETRYCANCEL = (uint)0x00000005L;
                uint MB_YESNO = (uint)(0x00000004L | 0x00000040L);
                uint MB_YESNOCANCEL = (uint)(0x00000003L | 0x00000040L);
                int intresult = -1;
                WindowResult result;
 
                switch (type)
                {
                    case WindowOptions.AbortRetryIgnore: 
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_ABORTRETRYIGNORE);
                        break;
                    case WindowOptions.CancelTryContinue:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_CANCELTRYCONTINUE);
                        break;
                    case WindowOptions.Help:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_HELP);
                        break;
                    case WindowOptions.OK:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_OK);
                        break;
                    case WindowOptions.OkCancel:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_OKCANCEL);
                        break;
                    case WindowOptions.RetryCancel:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_RETRYCANCEL);
                        break;
                    case WindowOptions.YesNo:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_YESNO);
                        break;
                    case WindowOptions.YesNoCancel:
                        intresult = MessageBox(GetWindowHandle(), text, caption, MB_YESNOCANCEL);
                        break;
                    default:
                        intresult = MessageBox(GetWindowHandle(), text, caption, (uint)(0x00000000L | 0x00000010L));
                        break;
                }
 
                switch(intresult)
                {
                    case 1:
                        result = WindowResult.OK;
                        break;
                    case 2:
                        result = WindowResult.Cancel;
                        break;
                    case 3:
                        result = WindowResult.Abort;
                        break;
                    case 4:
                        result = WindowResult.Retry;
                        break;
                    case 5:
                        result = WindowResult.Ignore;
                        break;
                    case 6:
                        result = WindowResult.Yes ;
                        break;
                    case 7:
                        result = WindowResult.No ;
                        break;
                    case 10:
                        result = WindowResult.TryAgain;
                        break;
                    default:
                        result = WindowResult.OK;
                        break;
 
                }
 
                return result;
            }
            catch (Exception)
            {
                return WindowResult.None;
            }
        }
    }

    public enum WindowOptions
    {
        AbortRetryIgnore,
        CancelTryContinue,
        Help,
        OK,
        OkCancel,
        RetryCancel,       
        YesNo,
        YesNoCancel
    }

    public enum WindowResult
    {
        None,
        OK,
        Abort,
        Cancel,
        Retry,
        Ignore,
        Yes,
        No,
        TryAgain
    }
}
