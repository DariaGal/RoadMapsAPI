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
    }
}
