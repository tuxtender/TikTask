using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace TikTask
{
    class TagStorage
    {
        private string fileName;
		public TagItem root;

        public TagStorage(string fileName)
        {
			root = new TagItem("Root", "", null);
            //For LoadXml method
            this.fileName = fileName;
        }

        public void LoadXml()
        {
            XmlDocument xdoc = new XmlDocument();

            try{
                xdoc.Load(fileName);
			} catch(FileNotFoundException e) {
				Console.WriteLine(e.Message);
				return;
			} catch(XmlException e) {
				Console.WriteLine("File {0} is an invalid format", fileName);
				Console.WriteLine(e.Message);
				return;
            }

            XmlElement rootXmlElement = xdoc.DocumentElement;
            //Record a name of a root tag to TagItem instance
			root.NodeName = rootXmlElement.LocalName;

			_InitTree(rootXmlElement, root);
        }

        private void _InitTree(XmlNode node, TagItem tagItem)
        {
            XmlNodeList childs = node.ChildNodes;
            XmlNode child;
            TagItem tI;
            //Node isn't content a content and other node at the same time
            //One node in list is empty or string including node
            if (childs.Count > 0)
            {
                for (int i = 0; i < childs.Count; i++)
                {
                    child = childs.Item(i);

                    //Checking it's child no more include tag
                    if (child.LocalName == "#text")
                    {
                        tI = new TagItem(tagItem.NodeName,
						                 child.Value,
						                 tagItem.parent);
                        try{
                            tagItem.parent.Add(tI);
                            tagItem.parent.Remove(tagItem);
                        }
                        catch (NullReferenceException){
                            Console.WriteLine("In terms of a task a root " +
                                              "node have to a \"none\" type cause " +
                                              "it doesn\'t read xml files contaning" +
                                              " numeric root");
							//System.Environment.Exit(0);
							return;
                        } 
                        continue;
                    }
                    
                    //Assign tag name for TagItem's item with empty content
                    tI = new TagItem(child.Name, "", tagItem);
                    tagItem.Add(tI);
                    
                    //go in depth
                    _InitTree(child, tI);
                }
            }
            else
            {
                //There isn't tag anymore. Assign content 
                tagItem.content = node.Value;
                return;
            }
        }
        
        //Save to human-readable text with indents
        public void SaveXml()
        {
            using (StreamWriter sw = new StreamWriter("out.xml"))
            {
                int offset = 2;
                
                sw.Write("<{0}>\n", root.NodeName);
                _MakeXml(root, sw, offset);
                sw.Write("</{0}>", root.NodeName);
            }
        }

        private void _MakeXml(TagItem tag, StreamWriter sw, int offset)
        {
            List<TagItem> list = tag.childList;
            string o;

            offset++;

            if (list.Count > 0)
            {
                foreach (TagItem tI in list)
                {
                    if (tI.content == "")
                    {
                        o = offset.ToString();

                        sw.Write("{0," + o + "}{1}{2}\n", "<", tI.NodeName, ">");
                        _MakeXml(tI, sw, offset);

                        int io1 = offset+1;
                        string so1 = io1.ToString();

                        sw.Write("{0," + so1 + "}{1}{2}\n", "</", tI.NodeName, ">");

                    }
                    else if (tI.childList.Count == 0)
                    {
                        o = offset.ToString();
                        
                        sw.Write("{0," + o + "}{1}{2}", "<", tI.NodeName, ">");
                        sw.Write("{0}", tI.content);
                        sw.Write("{0}{1}{2}\n", "</", tI.NodeName, ">");
                    }
                }
            }

        }

        

    }

    
}
