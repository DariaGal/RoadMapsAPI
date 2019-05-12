using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public class NodeDescriptionInfoConverter
    {
        public static Client.NodeDescriptionInfo Convert(Model.NodeDescriptionInfo nodeDescriptionInfo)
        {
            if (nodeDescriptionInfo == null)
            {
                throw new ArgumentNullException(nameof(nodeDescriptionInfo));
            }

            var clientNodeData = nodeDescriptionInfo.Data.Select(x => NodeLinkDataConverter.Convert(x)).ToList();

            var clientNodeDescriptionInfo = new Client.NodeDescriptionInfo
            {
                Description = nodeDescriptionInfo.Description,
                Data = clientNodeData
            };

            return clientNodeDescriptionInfo;
        }

        public static Model.NodeDescriptionInfo Convert(Client.NodeDescriptionInfo clientNodeDescriptionInfo)
        {
            if (clientNodeDescriptionInfo == null)
            {
                throw new ArgumentNullException(nameof(clientNodeDescriptionInfo));
            }

            var nodeData = clientNodeDescriptionInfo.Data.Select(x => NodeLinkDataConverter.Convert(x)).ToList();

            var nodeDescriptionInfo = new Model.NodeDescriptionInfo
            {
                Description = clientNodeDescriptionInfo.Description,
                Data = nodeData
            };

            return nodeDescriptionInfo;
        }
    }
}