using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using OpenPop.Mime.Header;
namespace AbbouClima.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarPresupuestoAsync(string destinatario, string asunto, string mensaje, byte[] adjunto = null, string nombreAdjunto = "Presupuesto.pdf")
        {

            var emailSettings = _configuration.GetSection("EmailSettings");

            // Crear el mensaje de correo
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Abbou Clima", emailSettings["SenderEmail"]));
            message.To.Add(new MailboxAddress("", destinatario));
            message.Subject = asunto;

            // Cuerpo del correo (puede ser texto o HTML)
            var body = new TextPart("HTML")
            {
                Text = mensaje
            };
            message.Body = body;

            // Crear el adjunto (asegurándote de que el archivo existe en la ruta proporcionada)
            var attachment = new MimePart("application", "octet-stream")
            {
                Content = new MimeContent(new System.IO.MemoryStream(adjunto)), // Usar los bytes directamente
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = (ContentEncoding)ContentTransferEncoding.Base64,
                FileName = nombreAdjunto // El nombre del archivo que deseas que se adjunte
            };

            // Crear un Multipart que contenga tanto el cuerpo como el adjunto
            var multipart = new Multipart("mixed")
            {
                body, // Cuerpo del correo
                attachment // El adjunto
            };

            message.Body = multipart;

            // Usar MailKit para enviar el correo
            using (var smtp = new SmtpClient())
            {
                // Deshabilitar la validación del certificado SSL
                smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Conectar al servidor SMTP con autenticación
                await smtp.ConnectAsync(emailSettings["SmtpServer"], 587, SecureSocketOptions.StartTls);

                // Autenticarse en el servidor
                await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);

                // Enviar el correo
                await smtp.SendAsync(message);

                // Desconectar
                await smtp.DisconnectAsync(true);
 
            }

        }

    }
}
