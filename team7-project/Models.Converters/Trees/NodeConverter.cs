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

            var clientNodeDescriptionInfo = node.Info.Select(x => NodeDescriptionConverter.Convert(x)).ToList();



            var clientNode = new Client.Node
            {
                Id = node.Id,
                Text = node.Text,               
                X = node.X,
                Y = node.Y,
                Color = node.Color,
                Info = clientNodeDescriptionInfo
            };

            return clientNode;
        }

        public static Model.Node Convert(Client.Node clientNode)
        {
            if (clientNode == null)
            {
                throw new ArgumentNullException(nameof(clientNode));
            }

            var nodeDescriptionInfo = clientNode.Info.Select(x => NodeDescriptionConverter.Convert(x)).ToList();

            var node = new Model.Node
            {
                Id = clientNode.Id,
                Text = clientNode.Text,
                X = clientNode.X,
                Y = clientNode.Y,
                Color = clientNode.Color,
                Info = nodeDescriptionInfo
            };

            return node;
        }
    }
}
