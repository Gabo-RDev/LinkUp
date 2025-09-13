using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LinkUp.Application.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace LinkUp.Infrastructure.Shared.Services;

public sealed class CloudinaryService(IOptions<Domain.Configuration.CloudinaryConfiguration> cloudinaryOptions): ICloudinaryService
{
    private Domain.Configuration.CloudinaryConfiguration _cloudinaryConfiguration { get; } = cloudinaryOptions.Value;
    

    public async Task<string> UploadImageAsync(Stream archive, string imageName, CancellationToken cancellationToken)
    {
        var cloudinary = new Cloudinary(_cloudinaryConfiguration.CloudinaryUrl);
        ImageUploadParams uploadImage = new()
        {
            File = new FileDescription(imageName, archive),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };
        ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadImage, cancellationToken);
        return uploadResult.SecureUrl.ToString();
    }
}