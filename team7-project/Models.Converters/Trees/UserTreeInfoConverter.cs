using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Trees
{
    using Client = Client.Models.Trees;
    using Model = Models.Trees;

    public static class UserTreeInfoConverter
    {
        public static Client.UserTrees.UserTreeInfo Convert(Model.UserTrees.UserTreeInfo modelTreeInfo)
        {
            if(modelTreeInfo == null)
            {
                    throw new ArgumentNullException(nameof(modelTreeInfo));
            }

            var clientTreeInfo = new Client.UserTrees.UserTreeInfo
            {
                Id = modelTreeInfo.Id,
                Title = modelTreeInfo.Title,
                Author = modelTreeInfo.Author,
                Description = modelTreeInfo.Description,
                Tags = modelTreeInfo.Tags,
                EnableEdit = false
            };

            return clientTreeInfo;
        }
    }
}
