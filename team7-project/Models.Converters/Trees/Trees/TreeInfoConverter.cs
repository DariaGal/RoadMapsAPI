using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public class TreeInfoConverter
    {
        public static Client.TreeInfo Convert(Model.TreeInfo treeOutInfo)
        {
            if (treeOutInfo == null)
            {
                throw new ArgumentNullException(nameof(treeOutInfo));
            }

            var clientTreeOutInfo = new Client.TreeInfo
            {
                Id = treeOutInfo.Id,
                Author = treeOutInfo.Author,
                Description = treeOutInfo.Description,
                Tags = treeOutInfo.Tags,
                Title = treeOutInfo.Title,
                AddedToProfile = treeOutInfo.AddedToProfile
            };

            return clientTreeOutInfo;
        }

        public static Model.TreeInfo Convert(Client.TreeInfo clientTreeOutInfo)
        {
            if (clientTreeOutInfo == null)
            {
                throw new ArgumentNullException(nameof(clientTreeOutInfo));
            }

            var treeOutInfo = new Model.TreeInfo
            {
                Id = clientTreeOutInfo.Id,
                Author = clientTreeOutInfo.Author,
                Description = clientTreeOutInfo.Description,
                Tags = clientTreeOutInfo.Tags,
                Title = clientTreeOutInfo.Title,
                AddedToProfile = clientTreeOutInfo.AddedToProfile
            };

            return treeOutInfo;
        }
    }
}