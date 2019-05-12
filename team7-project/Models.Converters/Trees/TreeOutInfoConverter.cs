using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public class TreeOutInfoConverter
    {

        public static Client.TreeOutInfo Convert(Model.TreeOutInfo treeOutInfo)
        {
            if (treeOutInfo == null)
            {
                throw new ArgumentNullException(nameof(treeOutInfo));
            }

            var clientTreeOutInfo = new Client.TreeOutInfo
            {
                Id = treeOutInfo.Id,
                Description = treeOutInfo.Description,
                Tags = treeOutInfo.Tags,
                Title = treeOutInfo.Title
            };

            return clientTreeOutInfo;
        }

        public static Model.TreeOutInfo Convert(Client.TreeOutInfo clientTreeOutInfo)
        {
            if (clientTreeOutInfo == null)
            {
                throw new ArgumentNullException(nameof(clientTreeOutInfo));
            }

            var treeOutInfo = new Model.TreeOutInfo
            {
                Id = clientTreeOutInfo.Id,
                Description = clientTreeOutInfo.Description,
                Tags = clientTreeOutInfo.Tags,
                Title = clientTreeOutInfo.Title
            };

            return treeOutInfo;
        }
    }
}