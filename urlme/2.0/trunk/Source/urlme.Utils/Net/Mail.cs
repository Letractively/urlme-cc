// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mail.cs" company="Salem Web Network">
//   2010 Salem Web Network
// </copyright>
// <summary>
//   Provides methods for sending mail (namely encapsulates the server information)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace urlme.Utils.Net
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    /// <summary>
    /// Provides methods for sending mail (namely encapsulates the server information)
    /// </summary>
    public static class Mail
    {
        /// <summary>
        /// Sends an e-mail
        /// </summary>
        /// <param name="fromAddress">the from address</param>
        /// <param name="toAddress">the to address</param>
        /// <param name="subject">the subject</param>
        /// <param name="body">the body of the email</param>
        /// <param name="isBodyHtml">whether the body is Html or plain text</param>
        /// <returns>whether the send was successful</returns>
        public static bool SendMessage(string fromAddress, string toAddress, string subject, string body, bool isBodyHtml)
        {
            return Mail.SendMessage(
                    new MailAddress(fromAddress),
                    new[] { new MailAddress(toAddress) },
                    subject,
                    body,
                    isBodyHtml);
        }

        /// <summary>
        /// Sends an e-mail
        /// </summary>
        /// <param name="fromAddress">the from address</param>
        /// <param name="toAddresses">the to addresses</param>
        /// <param name="subject">the subject</param>
        /// <param name="body">the body of the email</param>
        /// <param name="isBodyHtml">whether the body is Html or plain text</param>
        /// <returns>whether the send was successful</returns>
        public static bool SendMessage(MailAddress fromAddress, IEnumerable<MailAddress> toAddresses, string subject, string body, bool isBodyHtml)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient(Configuration.Mail.SmtpHost, Configuration.Mail.SmtpPort))
                {
                    MailMessage msg = new MailMessage()
                                      {
                                              Body = body,
                                              IsBodyHtml = isBodyHtml,
                                              Subject = subject,
                                              From = fromAddress
                                      };

                    foreach (MailAddress addr in toAddresses)
                    {
                        msg.To.Add(addr);
                    }

                    // send message
                    smtp.Send(msg);

                    // successfully sent
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics.ErrorHandler.LogError(ex);
                return false;
            }
        }
    }
}
