using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public class NodeLinkDataConverter
    {
        public static Client.NodeLinkData Convert(Model.NodeLinkData nodeLinkData)
        {
            if (nodeLinkData == null)
            {
                throw new ArgumentNullException(nameof(nodeLinkData));
            }


            var clientNodeLinkData = new Client.NodeLinkData
            {
                Description = nodeLinkData.Description,
                Link = nodeLinkData.Link
            };

            return clientNodeLinkData;
        }

        public static Model.NodeLinkData Convert(Client.NodeLinkData clientNodeLinkData)
        {
            if (clientNodeLinkData == null)
            {
                throw new ArgumentNullException(nameof(clientNodeLinkData));
            }


            var nodeLinkData = new Model.NodeLinkData
            {
                Description = clientNodeLinkData.Description,
                Link = clientNodeLinkData.Link
            };

            return nodeLinkData;
        }
    }
}