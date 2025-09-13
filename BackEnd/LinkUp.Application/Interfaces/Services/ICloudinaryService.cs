namespace LinkUp.Application.Interfaces.Services;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(Stream archive, string imageName, CancellationToken cancellationToken);

}