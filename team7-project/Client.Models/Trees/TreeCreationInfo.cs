﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Client.Models.Trees
{
    public class TreeCreationInfo
    {
        /// <summary>
        /// Заголовок дерева
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        [DataMember(IsRequired = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Список узлов
        /// </summary>
        [DataMember(IsRequired = true)]
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Список связей
        /// </summary>
        [DataMember(IsRequired = true)]
        public List<Link> Links { get; set; }
    }
}
