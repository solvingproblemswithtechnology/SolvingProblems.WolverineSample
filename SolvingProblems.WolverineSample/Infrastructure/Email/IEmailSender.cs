namespace SolvingProblems.WolverineSample.Infrastructure.Email;

public interface IEmailSender
{
    Task<bool> SendEmail(string to, string templateId, object data, CancellationToken cancellationToken = default);

    Task<bool> SendEmailWithAttachment(string to, string templateId, object objectData, string attachmentName, string attachmentBase64Content, string attachmentMimeType, string attachmentContentId, CancellationToken cancellationToken = default);
}
