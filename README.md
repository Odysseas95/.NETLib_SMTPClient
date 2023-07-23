# SMTPClient Library

The SMTPClient Library is a C# .NET Framework library designed to provide an SMTP client functionality for reading messages from an email server and sending emails to recipients. The library includes two main classes: `SMTPClient` and `SMTPHelper`.

## SMTPClient Class

The `SMTPClient` class provides functionalities for connecting to an email server, retrieving email messages, and sending emails to recipients. It utilizes the MailKit library to interact with the IMAP protocol for reading email messages.

### Class Members:

- `EmailClient`: An instance of the `ImapClient` class from the MailKit library used to connect to the email server.
- `MessageList`: A list of `MimeMessage` objects to store retrieved email messages.
- `EmailListReceived`: A list of `EmailListEntity` objects containing email addresses and their corresponding display names for received messages.
- `AnswerEmailList`: A list of `EmailListEntity` objects containing email addresses and their corresponding display names for sending emails.

### Methods:

- `SMTPClient()`: Constructor for initializing the `SMTPClient` class. It connects to the email server and retrieves email messages.
- `GetMessageList()`: Retrieves email messages from the email server and adds them to the `MessageList`.

## SMTPHelper Class

The `SMTPHelper` class provides helper methods used by the `SMTPClient` class for sending emails. It utilizes the `SmtpClient` class from the System.Net.Mail namespace for sending emails via the SMTP protocol.

### Class Members:

- `BackupMsg`: A backup instance of the `MailMessage` class to handle cases where the primary email sending fails.
- `ClientReceiveUsername`: Username for connecting to the email server to retrieve messages.
- `ClientReceivePassword`: Password for connecting to the email server to retrieve messages.
- `ClientSendUsername`: Username for sending emails via SMTP.
- `ClientSendPassword`: Password for sending emails via SMTP.
- `BackupClientSendUsername`: Backup username for sending emails in case of failure.
- `BackupClientSendPassword`: Backup password for sending emails in case of failure.
- `LogEmailList`: A collection of email addresses used for email logging purposes.
- `GetEmailsHrBack`: The number of hours to retrieve email messages from the past.
- `EmailListEntity`: A helper class representing an email address and its display name.

### Methods:

- `SMTPSend()`: Sends an email using the `SmtpClient` and provides backup email sending functionality in case of failure.
- `SMTPLogSend()`: Sends an email for logging purposes using the `SmtpClient`.
- `ClientConnect()`: Connects to the email server for email retrieval.
- `SendEmail()`: Composes and sends an email with the provided message.

## Usage

The SMTPClient library provides functionalities for reading email messages and sending emails. To use the library, follow the steps below:

1. Instantiate the `SMTPClient` class to connect to the email server and retrieve messages.
2. Use the methods provided by the `SMTPClient` class to read email messages and handle them as needed.
3. To send emails, utilize the methods provided by the `SMTPHelper` class for email composition and sending.

## Contributing

Contributions to this project are welcome. If you find any issues or want to suggest improvements, please feel free to create a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
