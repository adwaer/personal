using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Specifications;
using Personal.Domain.Entities;
using Personal.Service;

namespace Personal.Resource
{
    public class DbResourcesService : IResourcesService
    {
        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;
        private static readonly ConcurrentDictionary<ResourceCache, IEnumerable> ResourceDictionary;

        static DbResourcesService()
        {
            ResourceDictionary = new ConcurrentDictionary<ResourceCache, IEnumerable>(); ;
        }

        public DbResourcesService(IQueryFactory queryFactory, ICommandFactory commandFactory)
        {
            _queryFactory = queryFactory;
            _commandFactory = commandFactory;
        }

        public IEnumerable Get(string name)
        {
            var resourceCache = ResourceCache.GetInstance(name);
            IEnumerable resourceSet;
            if (ResourceDictionary.TryGetValue(resourceCache, out resourceSet))
            {
                return resourceSet;
            }

            var single = _queryFactory
                .GetQuery<Text>()
                .Where(resourceCache.SearchPattern)
                .FirstOrDefault();

            IEnumerable texts;
            if (single == null)
            {
                texts = new ResourcesService()
                    .Get(name);
            }
            else
            {
                texts = Json.Decode<Dictionary<string, string>>(single.Value)
                    .ToArray();
            }

            var enumerable = texts as object[] ?? texts.Cast<object>().ToArray();

            ResourceDictionary[resourceCache] = enumerable;
            return enumerable;
        }

        public void Update(string key, string value)
        {
            var text = _queryFactory
                .GetQuery<Text>()
                .Where(ResourceCache.GetInstance(key).SearchPattern)
                .Single();

            text.Value = value;

            var command = _commandFactory.GetCommitCommand();
            command.Execute();
        }
    }
}
