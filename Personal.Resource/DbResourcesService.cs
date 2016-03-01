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
        private readonly IScope<IQueryFactory> _queryFactory;
        private readonly ConcurrentDictionary<ResourceCache, IEnumerable> _resourceDictionary;

        public DbResourcesService(IScope<IQueryFactory> queryFactory)
        {
            _queryFactory = queryFactory;
            _resourceDictionary = new ConcurrentDictionary<ResourceCache, IEnumerable>();
        }

        public IEnumerable Get(string name)
        {
            var resourceCache = ResourceCache.GetInstance(name);
            IEnumerable resourceSet;
            if (_resourceDictionary.TryGetValue(resourceCache, out resourceSet))
            {
                return resourceSet;
            }

            var single = _queryFactory
                .Instance
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

            _resourceDictionary[resourceCache] = texts;
            return texts;
        }
    }
}
