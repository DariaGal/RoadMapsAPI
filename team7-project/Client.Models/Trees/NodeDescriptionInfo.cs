using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees
{
    public class NodeDescriptionInfo
    {
        /// <summary>
        /// Описание узла
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Список ссылок
        /// </summary>
        public List<NodeLinkData> Data { get; set; }

    }
}
