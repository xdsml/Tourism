using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 
namespace menu;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        // Remplacez ces informations par vos propres identifiants Cloudinary.s
        var cloudName = "dvujlwv6u";
        var apiKey = "975766743984623";
        var apiSecret = "Flu_T_anjOxbxOTv07A2qzWpXm0";

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    // Méthode pour télécharger un fichier vers Cloudinary
    public async Task<string> UploadFileAsync(string filePath)
    {
        try
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(filePath),
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult?.SecureUrl != null)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }
            else
            {
                throw new Exception("Upload failed. No secure URL returned.");
            }
        }
        catch (Exception ex)
        {
            // Gérer les erreurs éventuelles lors du téléchargement
            throw new Exception($"Error uploading file: {ex.Message}", ex);
        }
    }

    // Méthode pour récupérer tous les URL des médias stockés sur Cloudinary (limité aux images)
    public async Task<List<string>> GetAllMediaUrlsAsync()
    {
        try
        {
            List<string> mediaUrls = new List<string>();

            // Liste les ressources de type "image"
            var resources = await _cloudinary.ListResourcesAsync(new ListResourcesParams
            {
                MaxResults = 50,
                Type = "upload",
                ResourceType = ResourceType.Image
            });

            if (resources?.Resources != null)
            {
                foreach (var item in resources.Resources)
                {
                    mediaUrls.Add(item.SecureUrl.AbsoluteUri); // Ajoute l'URL de l'image
                }
            }

            return mediaUrls;
        }
        catch (Exception ex)
        {
            // Gérer les erreurs éventuelles lors de la récupération des médias
            throw new Exception($"Error retrieving media URLs: {ex.Message}", ex);
        }
    }
}
