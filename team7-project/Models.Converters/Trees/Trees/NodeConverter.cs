using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public static class NodeConverter
    {
        public static Client.Node Convert(Model.Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }



            var clientNode = new Client.Node
            {
                Id = node.Id,
                Title = node.Title,
                X = node.X,
                Y = node.Y,
                Color = node.Color,
                Info = NodeDescriptionInfoConverter.Convert(node.Info)
            };

            return clientNode;
        }

        public static Model.Node Convert(Client.Node clientNode)
        {
            if (clientNode == null)
            {
                throw new ArgumentNullException(nameof(clientNode));
            }


            var node = new Model.Node
            {
                Id = clientNode.Id,
                Title = clientNode.Title,
                X = clientNode.X,
                Y = clientNode.Y,
                Color = clientNode.Color,
                Info = NodeDescriptionInfoConverter.Convert(clientNode.Info)
            };

            return node;
        }
    }
}
