namespace TrainingFPT.Helpers
{
    public static class UploadFileHelper
    {
        public static string UploadFile(IFormFile file)
        {
            string uniqueFileName = string.Empty;
            try
            {
                string pathUploadServer = "wwwroot\\uploads\\images";

                string extension = Path.GetExtension(file.FileName);
                string uniqueStr = Guid.NewGuid().ToString();
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string fileNameUpload = uniqueStr + "-" + time + extension;

                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadServer, fileNameUpload);
                var stream = new FileStream(uploadPath, FileMode.Create);
                file.CopyToAsync(stream);
                uniqueFileName = fileNameUpload;
            }
            catch (Exception ex)
            {
                uniqueFileName = ex.Message.ToString();
            }
            return uniqueFileName;
        }
    }
}
