using System;

namespace TikTask
{    
    class Editor
    {
        public const string FILE_NAME = "in.xml";
		static public TagStorage storage;

        static public void Load()
        {
			storage = new TagStorage(FILE_NAME);
            storage.LoadXml();
        }

        static public void Save()
        {
            storage.SaveXml();
        }

        static public void Print()
        {
			TagItem tag = storage.root;
            Console.WriteLine("{0} Level={1} {2} {3}",
                              tag.fullName,
                              tag.level,
                              tag.dataType,
                              tag.content);

            tag.Search("");
        }

        static public void Remove()
        {
			TagItem root = storage.root;

            Console.WriteLine("What tag you want to remove?\n" +
                              "Name of tag with path require");
            string expr = Console.ReadLine();
            //Get wanted tag
            TagItem tag = root.Search(expr);
            if (tag != null)
            {
				if (tag.parent != null)
				{
					tag.parent.Remove(tag);
				} else {
                //Attempt to remove a structure's root
					storage = new TagStorage(FILE_NAME);
				}
				Console.WriteLine("Deleted");
                return;
			} 
            Console.WriteLine("Tag is absent. Nothing delete");
        }

        static public void Add()
        {
			TagItem root = storage.root;

            Console.WriteLine("Choose parent tag");
            string path = Console.ReadLine();
            TagItem parent = root.Search(path);

            if (parent != null)
            {
                Console.WriteLine("Suggest name of tag");
                string name = Console.ReadLine();
                Console.WriteLine("What\'s data type it will be?");
                string type = Console.ReadLine();

                TagItem tag;

                switch (type)
                {
                    case "int":
                        tag = new TagItem(name, "", parent, type);
                        break;
                    case "float":
                        tag = new TagItem(name, "", parent, type);
                        break;
                    case "bool":
                        tag = new TagItem(name, "", parent, type);
                        break;
                    default:
                        tag = new TagItem(name, "", parent, "none");
                        break;
                }

                parent.Add(tag);
                Console.WriteLine("{0} is added to tree", tag.fullName);
                return;
            }
            Console.WriteLine("Tag {0} is absent. Nothing add", path);

        }

        static public void Rename()
        {
			TagItem root = storage.root;

            Console.WriteLine("What\'s tag rename?");
            string name = Console.ReadLine();
            TagItem tag = root.Search(name);
            if (tag != null)
            {
                Console.WriteLine("What\'s new name?");
                string newName = Console.ReadLine();
                Console.WriteLine("{0} is renamed to {1}",
				                  tag.NodeName, newName);
				tag.NodeName = newName;

                return;
            }
            Console.WriteLine("Tag is absent. Nothing rename");
        }
        //Move tag along a tree
        static public void Transfer()
        {
			TagItem root = storage.root;

            Console.WriteLine("What\'s tag will be NEW parent?");
            string parent = Console.ReadLine();

            TagItem parentItem = root.Search(parent);
            //Statement of existing both nodes in tree 
            //and is moving node  isn't parent relative to other
            if (parentItem != null)
            {
                Console.WriteLine("What\'s tag is moving?");
                string tag = Console.ReadLine();

                TagItem tagItem = root.Search(tag);
                if (tagItem != null)
                {
                    tagItem.Transfer(parentItem);
                    return;

                }
                Console.WriteLine("Transfer is aborted : There is no {0}",
                                  tag);
                return;
            }
            Console.WriteLine("Transfer is aborted : There is no {0}",
                              parent);

        }

        static public void SetContent()
        {
			TagItem root = storage.root;

            Console.WriteLine("What\'s tag content changing?");
            string tag = Console.ReadLine();
            TagItem item = root.Search(tag);
            if (item != null)
            {
                Console.WriteLine("Put something");
                string val = Console.ReadLine();
                //Cause dataType figure at instance initiation time
                //swap TagItem's items with matching name but different type 
                //all childs renaming tag get lost

                TagItem newTag = new TagItem(item.NodeName, val, item.parent);
                if (item == root)
                {
                    Console.WriteLine("Root of tree is stricly \"none\" type in terms of a test exercise");
                    return;
                }
                item.childList.Clear();
                item.parent.Remove(item);
                item.parent.Add(newTag);

                Console.WriteLine("{0} is set. Data type : {1}. Value : {2}",
                                  newTag.fullName,
                                  newTag.dataType,
                                  newTag.content);
                return;
            }
            Console.WriteLine("Tag is absent.");

        }
		static void Main(string[] args)
        {
            storage = new TagStorage(FILE_NAME);

            string stars = "\n************************************";
            string menu = "\nPress a key suggested menu\n" +
                "\'r\' : Read in.xml\n" +
                "\'w\' : Write to out.xml\n" +
                "\'p\' : Print a tree\n" +
                "\'a\' : Add a tag\n" +
                "\'d\' : Delete a tag\n" +
                "\'c\' : Change a tag's name\n" +
                "\'h\' : Show menu\n" +
                "\'q\' : Quit\n" +
                "Additional:\n" +
                "\'m\' : Moving tag\n" +
                "\'s\' : Set new value to a tag\n";

            Console.WriteLine(menu);

            for (; ; )
            {
                char key = (char)Console.Read();
                switch (key)
                {
                    case 'r':
                        Console.WriteLine(stars);
                        Load();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 'w':
                        Console.WriteLine(stars);
                        Save();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 'p':
                        Console.WriteLine(stars);
                        Print();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 'a':
                        Console.WriteLine(stars);
                        Add();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 'd':
                        Console.WriteLine(stars);
                        Remove();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 'c':
                        Console.WriteLine(stars);
                        Rename();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 'h':
                        Console.WriteLine(stars);
                        Console.WriteLine(menu);
                        break;
                    case 'q':
                        Console.WriteLine(stars);
                        Console.WriteLine("Exit");
                        System.Environment.Exit(0);
                        break;
                    case 'm':
                        Console.WriteLine(stars);
                        Transfer();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;
                    case 's':
                        Console.WriteLine(stars);
                        SetContent();
                        Console.WriteLine("\n\'h\' : Show menu\n");
                        break;

                }

            }





        }



    }





}


