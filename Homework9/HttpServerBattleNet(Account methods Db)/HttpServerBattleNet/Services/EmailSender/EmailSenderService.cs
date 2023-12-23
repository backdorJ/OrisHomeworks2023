using HttpServerBattleNet.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace HttpServerBattleNet.Services.EmailSender;

public class EmailSenderService : IEmailSenderService
{
    private readonly AppSettingsConfig _config = ServerConfiguration.Config;

    public void SendEmail(string emailFromUser, string passwordFromUser, string subject)
    {
        try
        {
            using var emailSender = new MimeMessage();
            var builder = new BodyBuilder();

            emailSender.From.Add(new MailboxAddress(_config.FromName, _config.EmailSender));
            emailSender.To.Add(new MailboxAddress("", emailFromUser));
            emailSender.Subject = subject;

            var attachments = new List<MimeEntity>()
            {
                new MimePart()
                {
                    Content = new MimeContent(File.OpenRead("MyHttpProjectWithEmailSend.zip")),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "MyHttpProjectWithEmailSend.zip"
                },
            };

            foreach (var mimeEntity in attachments)
                builder.Attachments.Add(mimeEntity);

            #region HtmlResponseOnEmail

            builder.HtmlBody =
                $@"
                    <!doctype html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport""
                              content=""width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0"">
                        <meta http-equiv=""X-UA-Compatible"" content=""ie=edge"">
                        <style>
                            *{{
                                padding: 0;
                                margin: 0;
                                box-sizing: border-box;
                            }}

                            .message-card{{
                                background-color: black;
                                max-width:600px;
                                height: 300px;
                                color: white;
                                padding: 10px;
                            }}

                            .message-card h1 {{
                                text-align: center;
                                margin-bottom: 20px;
                            }}

                            .message-card p {{
                                font-size: 19px;
                                line-height: 50px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""message-card"">
                            <h1>Hello from Battle.net</h1>
                            <p>Your email: {emailFromUser}</p>
                            <p>Your password: {passwordFromUser}</p>
                            <footer>
                                <p>With love, battle.net and its creator Damir Nabiullin 11-208</p>
                            </footer>
                        </div>
                    </body>
                    </html>
                ";

            #endregion

            emailSender.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            client.Connect(_config.SmtpServerHost, _config.SmtpServerPort, true);
            client.Authenticate(_config.EmailSender, _config.PasswordSender);
            client.Send(emailSender);
            client.Disconnect(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}