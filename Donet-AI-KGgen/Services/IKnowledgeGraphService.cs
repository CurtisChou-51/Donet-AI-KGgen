using Donet_AI_KGgen.Models;

namespace Donet_AI_KGgen.Services
{
    public interface IKnowledgeGraphService
    {
        Task<KnowledgeGraphDto> Generate(GenerateRequestDto dto);
    }
}