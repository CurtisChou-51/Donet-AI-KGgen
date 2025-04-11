namespace Donet_AI_KGgen.Models
{
    public class KnowledgeGraphDto
    {
        public List<KnowledgeGraphEntityDto> Entities { get; set; } = [];
        public List<KnowledgeGraphRelationshipDto> Relationships { get; set; } = [];
    }

    public class KnowledgeGraphEntityDto
    {
        public string Id { get; set; } = string.Empty;
        public double Weight { get; set; } = 0.0;
    }

    public class KnowledgeGraphRelationshipDto
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
