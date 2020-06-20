using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;

namespace MongoDb.UpdateNestedArray.IntegrationTests.DocumentRepositoryTests
{
    public static class UpdateDoorNames
    {
        public class Given_A_Document_Id_And_A_Collection_Of_Names_When_Updating_Door_Names
        {
            private readonly DocumentRepository _sut;
            private readonly Guid _id;
            private readonly IEnumerable<Label> _labels;

            public Given_A_Document_Id_And_A_Collection_Of_Names_When_Updating_Door_Names()
            {
                //Database
                const string connectionString = "mongodb://root:example@localhost:27017/admin?ssl=false";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("sasw");
                var communities = database.GetCollection<Community>("communities");
                _sut = new DocumentRepository(communities);

                //Given
                _id = Guid.NewGuid();
                var blocks = GetSampleBlocks();
                var community =
                    new Community
                    {
                        Id = _id,
                        Blocks = blocks
                    };
                _sut.CreateCommunity(community).GetAwaiter().GetResult();
                _labels = GetSampleLabels();
            }


            [Fact]
            public async Task When_Updating_Labels_It_Should_Not_Throw_Exception()
            {
                Exception exception = null!;
                try
                {
                    await _sut.UpdateDoorNames(_id, _labels);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                exception.Should().BeNull();
            }

            private IEnumerable<Block> GetSampleBlocks()
            {
                var blocks =
                    new List<Block>
                    {
                        new Block
                        {
                            Name = "Block 1",
                            Floors = new List<Floor> {
                                new Floor
                                {
                                    Name = "Ground Floor",
                                    Doors =
                                        new List<Door>
                                        {
                                            new Door {Name = "A"},
                                            new Door {Name = "B"}
                                        }
                                },
                                new Floor
                                {
                                    Name = "First Floor",
                                    Doors =
                                        new List<Door>
                                        {
                                            new Door {Name = "A"},
                                            new Door {Name = "B"},
                                            new Door {Name = "C"},
                                            new Door {Name = "D"}
                                        }
                                }
                            }
                        },
                        new Block
                        {
                            Name = "Block 2",
                            Floors =
                                new List<Floor>
                                {
                                    new Floor
                                    {
                                        Name = "Ground Floor",
                                        Doors =
                                            new List<Door>
                                            {
                                                new Door {Name = "A"},
                                                new Door {Name = "B"}
                                            }
                                    },
                                    new Floor
                                    {
                                        Name = "First Floor",
                                        Doors =
                                            new List<Door>
                                            {
                                                new Door {Name = "A"},
                                                new Door {Name = "B"},
                                                new Door {Name = "C"},
                                                new Door {Name = "D"}
                                            }
                                    }
                                }
                        }
                    };

                return blocks;
            }

            private IEnumerable<Label> GetSampleLabels()
            {
                var labels =
                    new List<Label>
                    {
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "A", Name = "Joe"},
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "A", Name = "Jane"},
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "A", Name = "Jake"},
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "A", Name = "Jeff"},

                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "B", Name = "Bob"},
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "B", Name = "Bill"},
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "B", Name = "Bonnie"},
                        new Label {BlockId = "Block 1", FloorId = "Ground Floor", DoorId = "B", Name = "Buck"},

                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "A", Name = "Fred"},
                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "A", Name = "Felix"},

                        new Label {BlockId = "Block 1", FloorId = "First Floor", DoorId = "A", Name = "Emma"},
                        new Label {BlockId = "Block 1", FloorId = "First Floor", DoorId = "A", Name = "Elan"},

                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "B", Name = "Axel"},
                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "B", Name = "Albin"},
                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "B", Name = "Antonio"},
                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "B", Name = "Alex"},
                        new Label {BlockId = "Block 2", FloorId = "Ground Floor", DoorId = "B", Name = "Assia"}
                    };

                return labels;
            }
        }
    }
}