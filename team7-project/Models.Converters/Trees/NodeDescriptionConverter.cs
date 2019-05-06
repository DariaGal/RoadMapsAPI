using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public class NodeDescriptionConverter
    {
        public static Client.NodeDescriptionInfo Convert(Model.NodeDescriptionInfo nodeDescriptionInfo)
        {
            if (nodeDescriptionInfo == null)
            {
                throw new ArgumentNullException(nameof(nodeDescriptionInfo));
            }


            var clientNodeDescriptionInfo = new Client.NodeDescriptionInfo
            {
                Description = nodeDescriptionInfo.Description,
                Data = nodeDescriptionInfo.Data
            };

            return clientNodeDescriptionInfo;
        }

        public static Model.NodeDescriptionInfo Convert(Client.NodeDescriptionInfo clientNodeDescriptionInfo)
        {
            if (clientNodeDescriptionInfo == null)
            {
                throw new ArgumentNullException(nameof(clientNodeDescriptionInfo));
            }

            var nodeDescriptionInfo = new Model.NodeDescriptionInfo
            {
                Description = clientNodeDescriptionInfo.Description,
                Data = clientNodeDescriptionInfo.Data
            };

            return nodeDescriptionInfo;
        }
    }
}