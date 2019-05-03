using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public static class LinkConverter
    {
        public static Client.Link Convert(Model.Link link)
        {
            if(link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            var clientLink = new Client.Link
            {
                SourceId = link.SourceId,
                TargetId = link.TargetId,
                Type = link.Type
            };

            return clientLink;
        }


        public static Model.Link Convert(Client.Link clientLink)
        {
            if (clientLink == null)
            {
                throw new ArgumentNullException(nameof(clientLink));
            }

            var link = new Model.Link
            {
                SourceId = clientLink.SourceId,
                TargetId = clientLink.TargetId,
                Type = clientLink.Type
            };

            return link;
        }
    }
}
