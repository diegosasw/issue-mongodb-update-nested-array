using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDb.UpdateNestedArray
{
    public class DocumentRepository
    {
        private readonly IMongoCollection<Community> _communities;

        public DocumentRepository(
            IMongoCollection<Community> communities)
        {
            _communities = communities;
        }

        public async Task CreateCommunity(Community community)
        {
            var options =
                new FindOneAndReplaceOptions<Community>
                {
                    IsUpsert = true
                };

            await _communities
                .FindOneAndReplaceAsync<Community>(
                    x => x.Id == community.Id,
                    community,
                    options);
        }

        public async Task UpdateDoorNames(Guid id, IEnumerable<Label> labels)
        {
            var labelsGroupedByHouse =
                labels
                    .ToList()
                    .GroupBy(x => new { x.BlockId, x.FloorId, x.DoorId })
                    .ToList();

            var filter =
                Builders<Community>
                    .Filter
                    .Where(x => x.Id == id);

            foreach (var house in labelsGroupedByHouse)
            {
                var houseBlockName = house.Key.BlockId;
                var houseFloorName = house.Key.FloorId;
                var houseDoorName = house.Key.DoorId;
                var names = house.Select(x => x.Name).ToList();

                var update =
                    Builders<Community>
                        .Update
                        .Set($"Blocks.$[{houseBlockName}].Floors.$[{houseFloorName}].Doors.$[{houseDoorName}].LabelNames", names);
                await _communities.UpdateOneAsync(filter, update);
            }
        }
    }
}
