using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;

namespace DCC_Standard
{

	/// <summary>
	/// Sends an email or SMS text from the application
	/// </summary>
	public class DCC_Email
    {


        /// <summary>
        /// Send an email (just Subject and Body to single recipient)
        /// </summary>
        /// <param name="sender">Email address of sender. If empty or null will default to the appSettings["SoftwareIdentityCode"]+"_Notifier@durham.gov.uk"</param>
        /// <param name="subject">Subject line. Cannot be empty or null</param>
        /// <param name="body">Message body. Cannot be empty or null</param>
        /// <param name="to">Email address of recipient (single address only, cannot be empty or null)</param>
        /// <returns>"OK" or an error message</returns>
        /// <param name="alternateView">AlternateView version of your body string if you want to include an image etc.</param>
        public static string Send(string sender, string subject, string body, string to, System.Net.Mail.AlternateView alternateView = null)
        {
			if (string.IsNullOrEmpty(to)) return "TO cannot be null or empty";
            List<string> toList = new List<string>();
            toList.Add(to);
            return Send(sender, subject, body, toList, null, null, null, alternateView: alternateView);
        }


        /// <summary>
        /// Send an SMS to a single recipient
        /// </summary>
        /// <param name="mobileNumber">mobile number (i.e. 11 digits)</param>
        /// <param name="message">message to send.  Usually 140 chars or less"</param>
        /// <returns>"OK" or an error message</returns>
        public static string Send_SMS(string mobileNumber, string message)
        {
            List<string> toList = new List<string>();
            toList.Add(mobileNumber+"@durhamtms.com");
            return Send("", message, "-", toList, null, null, null);
        }


		/// <summary>
		/// Send an email (Subject / Body / CC / BCC / Attachments to list of recipients)
		/// </summary>
		/// <param name="sender">Email address of sender. If empty or null will default to the appSettings["SoftwareIdentityCode"]+"_Notifier@durham.gov.uk"</param>
		/// <param name="subject">Subject line. Cannot be empty or null</param>
		/// <param name="body">Message body. Cannot be empty or null</param>
		/// <param name="to">Email addresses of recipients (can be null)</param>
		/// <param name="cc_List">Email addresses of copy recipients (can be null)</param>
		/// <param name="bcc_List">Email addresses of blind copy recipients (can be null)</param>
		/// <param name="attachmentList">full paths of files to attach (can be null)</param>
		/// <param name="memoryStreamAttachment">optional MemoryStream attachment</param>
		/// <param name="memoryStreamAttachmentFileName">optional FileName for MemmoryStream attachment)</param>
        /// <param name="alternateView">AlternateView version of your body string if you want to include an image etc.</param>
		/// <returns>"OK" or an error message</returns>
		public static string Send(string sender, string subject, string body, List<string> to, List<string> cc_List, List<string> bcc_List, List<string> attachmentList, byte[] memoryStreamAttachment = null, string memoryStreamAttachmentFileName = "Attachment.pdf", System.Net.Mail.AlternateView alternateView = null)
        {
            string reply = "OK";
			if (string.IsNullOrEmpty(body) && (alternateView == null)) return "You must send either a BODY or an ALTERNATEVIEW";
			if (string.IsNullOrEmpty(subject)) return "SUBJECT cannot be null or empty";
			if (string.IsNullOrEmpty(sender))
            {
                string senderStub = (string)(ConfigurationManager.AppSettings["SoftwareIdentityCode"] ?? "");
                if (senderStub.Length == 0) return "SoftwareIdentityCode is missing from AppSettings";
                sender = senderStub + "_Notifier@durham.gov.uk";
            }

            MailMessage letter = new MailMessage();
            try
            {
                letter.From = new MailAddress(sender);
                letter.Subject = subject;
                if (alternateView == null)
                {
                    letter.Body = body;
                    if ( (body.Length > 4) && (body.Substring(0, 5).ToUpper() == "<HTML") ) letter.IsBodyHtml = true;
                }
                else
                {
                    letter.AlternateViews.Add(alternateView);
                    letter.IsBodyHtml = true;
                }

                if (to != null) foreach (string toWhom in to) letter.To.Add(toWhom);
                if (cc_List != null) foreach (string cc in cc_List) letter.CC.Add(cc);
                if (bcc_List != null) foreach (string bcc in bcc_List) letter.Bcc.Add(bcc);
                if (attachmentList != null) foreach (string attachment in attachmentList) letter.Attachments.Add(new Attachment(attachment));
				if (memoryStreamAttachment != null)	letter.Attachments.Add(new Attachment(new MemoryStream(memoryStreamAttachment), memoryStreamAttachmentFileName));
                if (letter.To.Count + letter.CC.Count + letter.Bcc.Count == 0) return "no recipients in TO, CC or BCC";

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Port = 25;

                object host = ConfigurationManager.AppSettings["SMTP_Host"];
                if (host != null && host is string)
                {
                    client.Host = (string)host;
                }
                else
                {
                    SmtpSection DefaultSMTP = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                    if (DefaultSMTP.Network.Host != null)   // use if host is supplied
                    {
                        client.DeliveryMethod = DefaultSMTP.DeliveryMethod;
                        client.Port           = DefaultSMTP.Network.Port;
                        client.Host           = DefaultSMTP.Network.Host;
                    }
                    else  // if nothing provided then use port 25 on host name that was ok in December 2011
                    {
                        client.Host = "smtp.durham.gov.uk";
                    }
                }
                client.Send(letter);
				client.Dispose();
            }

			catch (Exception e)
            {
                reply = e.Message;
                Exception eHere = e;
                while (eHere.InnerException != null)
                {
                    reply += " ... " + eHere.InnerException.Message;
                    eHere = eHere.InnerException;
                    reply = (reply.Length < 4000 ? reply : reply.Substring(0, 3998));
                }
            }

			finally
            {
                letter.Dispose();
            }

			return reply;
        }

    }

}
