namespace LawOffice.Models
{
    public class UploadDocumentDto
    {
        public long CaseId { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }
}
