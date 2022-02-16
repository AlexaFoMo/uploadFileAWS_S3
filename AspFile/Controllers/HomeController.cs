using AspFile.Models;
using AspFile.Models.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AspFile.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _enviroment;
        private string server = "s3-us-east-1.amazonaws.com";
        private string bucket = "awsbucketalexfomo";
        private string user = "";
        private string pass = "";

        public HomeController(IWebHostEnvironment enviroment)
        {
          _enviroment = enviroment;
        }

        public IActionResult Index()
        {
            ViewBag.message = TempData["message"];
            return View();
        }

        public async Task<IActionResult> Upload1(UploadModel upload)
        {
            using (var db = new SampleContext())
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    var file = new File();
                    await upload.MyFile.CopyToAsync(memoryStream);
                    file.Filedb = memoryStream.ToArray();
                    db.Files.Add(file);
                    db.SaveChanges();
                }


            }
            TempData["message"] = "Archivo cargado";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Upload2(UploadModel upload)
        {
            var fileName = System.IO.Path.Combine(_enviroment.ContentRootPath, "uploads", upload.MyFile.FileName);

            await upload.MyFile.CopyToAsync(new System.IO.FileStream(fileName, System.IO.FileMode.Create));

            TempData["message"] = "Archivo cargado";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Upload3(UploadModel upload)
        {
            var fileName = System.IO.Path.Combine(_enviroment.ContentRootPath, 
                "uploads", upload.MyFile.FileName);

            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                await upload.MyFile.CopyToAsync(fs);
            }

            var minioClient = new MinioClient(server, user, pass).WithSSL();

            byte[] bs = await System.IO.File.ReadAllBytesAsync(fileName);
            var ms = new System.IO.MemoryStream(bs);

            await minioClient.PutObjectAsync(bucket, upload.MyFile.FileName,
                ms, ms.Length, "application/octet-stream", null, null);

            System.IO.File.Delete(fileName);

            TempData["message"] = "Archivo cargado";
            return RedirectToAction("Index");
        }




    }
}
