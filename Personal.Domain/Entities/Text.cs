using CostEffectiveCode.Domain.Ddd.Entities;

namespace Personal.Domain.Entities
{
    public class Text : EntityBase<int>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Culture { get; set; }
    }
}
