using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public static class TreeConverter
    {
        public static Client.Tree Convert(Model.Tree tree, string authorLogin)
        {
            if(tree == null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            var clientNodes = tree.Nodes.Select(x => NodeConverter.Convert(x)).ToList();
            var clientLinks = tree.Links.Select(x => LinkConverter.Convert(x)).ToList();

            var clientTree = new Client.Tree
            {
                Id = tree.Id,
                Author = authorLogin,
                Description = tree.Description,
                Tags = tree.Tags,
                Title = tree.Title,
                Nodes = clientNodes,
                Links = clientLinks
            };

            return clientTree;
        }
    }
}
