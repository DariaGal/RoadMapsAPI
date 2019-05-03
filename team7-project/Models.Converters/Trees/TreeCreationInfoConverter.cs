using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public class TreeCreationInfoConverter
    {
        public static Model.TreeCreationInfo Convert(Client.TreeCreationInfo clientTree)
        {
            if (clientTree == null)
            {
                throw new ArgumentNullException(nameof(clientTree));
            }

            var clientNodes = clientTree.Nodes.Select(x => NodeConverter.Convert(x)).ToList();
            var clientLinks = clientTree.Links.Select(x => LinkConverter.Convert(x)).ToList();

            var tree = new Model.TreeCreationInfo
            {
                Title = clientTree.Title,
                Nodes = clientNodes,
                Links = clientLinks
            };

            return tree;
        }
    }
}
