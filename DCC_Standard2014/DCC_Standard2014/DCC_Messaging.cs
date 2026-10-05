using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DCC_Standard
{

    /// <summary>
    /// enum for Message Type
    /// </summary>
    public enum DCC_MessageType { Success, Error, Warning, Information, Message }


    /// <summary>
    /// Checks ViewData & TempData for messages then creates the Html for the Message Area on the fly
    /// </summary>
    public static class MessageHtmlHelper
    {

       public static HtmlString MessageArea(this HtmlHelper htmlHelper)
       {
           var messages = String.Empty;
           foreach (var messageType in Enum.GetNames(typeof(DCC_MessageType)))
           {
               var message = htmlHelper.ViewContext.ViewData.ContainsKey(messageType)
                           ? htmlHelper.ViewContext.ViewData[messageType]
                           : htmlHelper.ViewContext.TempData.ContainsKey(messageType)
                           ? htmlHelper.ViewContext.TempData[messageType]
                           : null;
               if (message != null)
               {
                    var newMessageDiv = "<div class='msg-container'><div class='msg-wrapper-" + messageType.ToLowerInvariant() + "'><div class='msg-content-" + messageType.ToLowerInvariant() + "'>" + message.ToString() + "</div></div></div>";
                    messages += newMessageDiv.ToString();

                   //var newMessageDiv = new TagBuilder("div");
                   //newMessageDiv.GenerateId("msg-wrapper");
                   //newMessageDiv.AddCssClass(String.Format("{0}", messageType.ToLowerInvariant()));
                   //newMessageDiv.SetInnerText(message.ToString());
                   //messages += newMessageDiv.ToString();
               }
           }
           return MvcHtmlString.Create(messages);
       }

    }


    /// <summary>
    /// Used to send a message from a controller to ViewData or TempData
    /// </summary>
    public static class ControllerInvoked
    {

        public static void DCC_DisplayMessage(this Controller controller, DCC_MessageType messageType, string message, bool showInAnotherView = false)
        {
            var messageTypeKey = messageType.ToString();
            if (showInAnotherView)
                controller.TempData[messageTypeKey] = message;
            else
                controller.ViewData[messageTypeKey] = message;
        }

    }

}
