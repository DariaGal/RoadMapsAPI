using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Trees
{
    public class TreeInfo
    {
        /// <summary>
        /// Идентификатор дерева
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Автор дерева
        /// </summary>
        public string Author { get; set; }

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

        public TreeInfo(Tree tree)
        {
            Id = tree.Id;
            Title = tree.Title;
            Description = tree.Description;
            Tags = tree.Tags;
        }

        public TreeInfo()
        {
        }
    }
}
