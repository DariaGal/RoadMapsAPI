using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public static class TreeInfoSearchQueryConverter
    {
        public static Model.TreeInfoSearchQuery Convert(Client.TreeInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var modelQuery = new Model.TreeInfoSearchQuery
            {
                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset,
                Title = clientQuery.Title,
                Tags = clientQuery.Tags
            };

            return modelQuery;
        }
    }
}
