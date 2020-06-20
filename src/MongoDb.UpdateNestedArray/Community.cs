using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDb.UpdateNestedArray
{
    public class Community
    {
        public Guid Id { get; set; }
        public IEnumerable<Block> Blocks { get; set; } = Enumerable.Empty<Block>();
    }

    public class Block
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Floor> Floors { get; set; } = Enumerable.Empty<Floor>();
    }

    public class Floor
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Door> Doors { get; set; } = Enumerable.Empty<Door>();
    }

    public class Door
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<string> LabelNames = Enumerable.Empty<string>();
    }
}
