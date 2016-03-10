using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Specifications;
using Personal.Domain.Entities;
using Personal.Schema;
using Personal.Service;

namespace Personal.Resource
{
    public class DbResourcesService : IResourcesService
    {
        private readonly IQueryFactory _queryFactory;
        private static readonly ConcurrentDictionary<ResourceCache, IEnumerable> ResourceDictionary;

        static DbResourcesService()
        {
            ResourceDictionary = new ConcurrentDictionary<ResourceCache, IEnumerable>();
        }

        public DbResourcesService(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
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
                .Where(new ExpressionSpecification<Text>(text => text.Culture == resourceCache.Culture.Name
                                                                 && text.Key == resourceCache.Id))
                .FirstOrDefault();

            IEnumerable<string> texts;
            if (single == null)
            {
                texts = new ResourcesService().Get(name)
                    .Cast<string>()
                    .ToArray();
            }
            else
            {
                texts = Json.Decode<IEnumerable<string>>(single.Value)
                    .ToArray();
            }

            ResourceDictionary[resourceCache] = texts;
            return texts;
        }
    }
}
