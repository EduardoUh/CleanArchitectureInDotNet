namespace CleanArchitecture.Domain.Common
{
    // must be abstract because we don't want to create instances from this class, we just want to heritage its properties
    public abstract class BaseDomainModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string? LastUpdatedby { get; set; }
    }
}
