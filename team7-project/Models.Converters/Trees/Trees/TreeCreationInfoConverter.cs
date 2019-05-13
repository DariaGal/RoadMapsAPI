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

            List<Model.Node> clientNodes = null;
            List<Model.Link> clientLinks = null;


            if (clientTree.Nodes != null)
            {
                clientNodes = clientTree.Nodes.Select(x => NodeConverter.Convert(x)).ToList();
            }

            if (clientTree.Links != null)
            {
                clientLinks = clientTree.Links.Select(x => LinkConverter.Convert(x)).ToList();
            }

            var tree = new Model.TreeCreationInfo
            {
                Title = clientTree.Title,
                Description = clientTree.Description,
                Tags = clientTree.Tags,
                Nodes = clientNodes,
                Links = clientLinks
            };

            return tree;
        }
    }
}
