using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees
{
    public class TreeOutInfo
    {
        /// <summary>
        /// Идентификатор дерева
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Заголовок дерева
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        public List<string> Tags { get; set; }

        public TreeOutInfo(Tree tree)
        {
            Id = tree.Id;
            Title = tree.Title;
            Description = tree.Description;
            Tags = tree.Tags;
        }

        public TreeOutInfo()
        {
        }
    }
}
