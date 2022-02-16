using Microsoft.AspNetCore.Http;

namespace AspFile.Models.Dto
{
    public class UploadModel
    {
        public IFormFile MyFile { set; get; }
    }
}
