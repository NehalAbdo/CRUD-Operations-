using Demo.Dal.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Utility
{
	public static class MailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("Nehalabdelrahman7@gmail.com", "osunktpvmvtlwtou");
			client.Send("Nehalabdelrahman7@gmail.com",email.Recipient,email.Subject,email.Body);

		}
	}
}
