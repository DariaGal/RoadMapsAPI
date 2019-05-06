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
        /// Cgbcjr ccskjr
        /// </summary>
        public List<string> Data { get; set; }


    }

    public class NodeLinkData
    {
        /// <summary>
        /// Описание ссылки
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Текст ссылки
        /// </summary>
        public string Link { get; set; }
    }
}
