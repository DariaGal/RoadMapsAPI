using System;
using System.Collections.Generic;
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
                Text = node.Text,
                Type = node.Type,
                X = node.X,
                Y = node.Y
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
                Text = clientNode.Text,
                Type = clientNode.Type,
                X = clientNode.X,
                Y = clientNode.Y
            };

            return node;
        }
    }
}
