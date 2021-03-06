﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Trees.UserTrees
{
    public class UserTreeInfo
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public List<string> Tags { get; set; }

        public string Description { get; set; }

        public bool EnableEdit { get; set; }

        public List<string> CheckedNodes { get; set; }

        public int AllNodesCount { get; set; }
    }
}
