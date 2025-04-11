using Donet_AI_KGgen.Models;
using Microsoft.Extensions.AI;

namespace Donet_AI_KGgen.Services
{
    public class KnowledgeGraphService : IKnowledgeGraphService
    {
        private readonly IChatClient _chatClient;
        private readonly ISystemMessageService _systemMessageService;

        public KnowledgeGraphService(IChatClient chatClient, ISystemMessageService systemMessageService)
        {
            _chatClient = chatClient;
            _systemMessageService = systemMessageService;
        }

        public async Task<KnowledgeGraphDto> Generate(GenerateRequestDto dto)
        {
            string systemMessageText = _systemMessageService.GetKnowledgeGraphMessage();
            ChatResponse<KnowledgeGraphDto> resp = await _chatClient.GetResponseAsync<KnowledgeGraphDto>(
            [
                new ChatMessage(ChatRole.System, systemMessageText),
                new ChatMessage(ChatRole.User, dto.Input)
            ]);
            var knowledgeGraphDto = resp.Result;
            AddMissingEntities(knowledgeGraphDto);
            return knowledgeGraphDto;
        }

        private static void AddMissingEntities(KnowledgeGraphDto knowledgeGraphDto)
        {
            HashSet<string> existingEntityIds = knowledgeGraphDto.Entities.Select(e => e.Id).ToHashSet();
            var sourceIds = knowledgeGraphDto.Relationships.Select(x => x.Source);
            var targetIds = knowledgeGraphDto.Relationships.Select(x => x.Target);
            string[] allMissingIds = sourceIds.Concat(targetIds).Where(x => !existingEntityIds.Contains(x)).ToArray();
            var missingEntities = allMissingIds
                .GroupBy(x => x)
                .Select(g => new KnowledgeGraphEntityDto { Id = g.Key, Weight = 0.3 });  // Default weight for missing entities
            knowledgeGraphDto.Entities.AddRange(missingEntities);
        }

    }
}
