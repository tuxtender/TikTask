using System;
using System.Collections.Generic;

namespace TikTask
{
    class TagItem
    {
		private string _nodeName;

        public string pathNode;
        public string fullName;
        public uint level;
        public string content;
        public string dataType;
        public TagItem parent;
        public List<TagItem> childList;

		public string NodeName {
            get {
                return _nodeName;
            }
            set {
                _nodeName = value;
				_Refresh();
            }
        }
        
        private void _DataTypeReview()
        {
            int intNum;
            float floatNum;
            bool boolNum;

            if (int.TryParse(content, out intNum))
            {
                dataType = "int";
            }
            else if (float.TryParse(content, out floatNum))
            {
                dataType = "float";
            }
            else if (bool.TryParse(content, out boolNum))
            {
                dataType = "bool";
            }
            else
            {
                //Uncomment to hide text
                dataType = "none";
                //content = "";
            }

        }
       
		private void _LevelCount()
        {
			if (parent != null) {
				level = parent.level + 1;
			} else {
				level = 0;
			}
        }
        
        private void _AbsolutePath()
        {
			if (parent != null) {
				pathNode = parent.fullName;
			} else {
				pathNode = "";
			}
        }

        private void _FullName()
        {
			fullName = pathNode+ '.' + _nodeName;
            fullName = fullName.TrimStart('.');
        }

        //Depth-first search
		private void _Refresh()
        {
			_LevelCount();
            _AbsolutePath();
            _FullName();

			foreach (TagItem item in childList) 
			{
				item._Refresh();
            }
            
        }

        private void _Print()
        {
            Console.WriteLine("{0} Level={1} {2} {3}",
                              fullName,
                              level,
                              dataType,
                              content);
        }


		public TagItem(string name, string number, TagItem parentNode, string type = "none")
        {
            _nodeName = name;
            content = number;
            parent = parentNode;
            childList = new List<TagItem>();
            //Content initiat at instance creating
            dataType = type;

            if (type == "none")
            {
                _DataTypeReview();
            }
            //Figure out attributes: level, pathNode, fullName
            _LevelCount();
            _AbsolutePath();
            _FullName();

        }

        public TagItem SearchChild(string expr)
        {
			foreach (TagItem item in childList)
            {
                if (item._nodeName == expr)
                {
                    return item;
                }
            }
            return null;
        }

        //Search with "" parameter to traverse tree (DFS) and to write in console
        public TagItem Search(string expr)
        {
            TagItem legacy = null;
   
            if (_nodeName == expr)
            {
                return this;
            }

			foreach (TagItem item in childList)
            {
                if (expr == "")
                {
                    item._Print();
                }

                if (item.fullName == expr)
                {
                    return item;
                }

                legacy = item.Search(expr);

                if (legacy != null)
                {
                    return legacy;
                }
            }
            
            return null;
        }
              
        public void Add(TagItem item)
        {
            childList.Add(item);
        }

        public void Remove(TagItem item)
        {
            childList.Remove(item);
        }
       

		public void Transfer(TagItem newParent)
        {
            //Checking newParent is not for child of moving node
            TagItem tag = Search(newParent.fullName);
            if (tag != null)
            {
                Console.WriteLine("Transfer aborted. {0} is parent for {1}",
                                  fullName, newParent.fullName);
                return;
            }
            
			newParent.Add(this);
			parent.Remove(this);
			parent = newParent;
   
            _Refresh();
            Console.WriteLine("Transfer done.");
        }

    }

}
