using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees
{
    /// <summary>
    /// Связь узлов дерева
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Идетификатор узла источника
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// Идетификатор целевого узла
        /// </summary>
        public string TargetId { get; set; }
    }
}
