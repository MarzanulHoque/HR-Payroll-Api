namespace HRMS.Application.Common.Models;

public record EmailAttachment(string FileName, byte[] Content, string ContentType);
