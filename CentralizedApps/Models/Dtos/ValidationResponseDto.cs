
namespace CentralizedApps.Models.Dtos
{
    public class ValidationResponseDto
    {
        public int CodeStatus { get; set; }
        public bool BooleanStatus { get; set; } = false;
        public string? SentencesError { get; set; } = null;

    }
}