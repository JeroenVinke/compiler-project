using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Common
{
    public class SymbolTable
    {
        public static int MaxId = 0;
        public SymbolTable Parent { get; set; }

        public List<SymbolTableEntry> Entries { get; set; } = new List<SymbolTableEntry>();
        public List<SymbolTable> Children { get; set; } = new List<SymbolTable>();
        public int Id { get; private set; }

        public SymbolTable()
        {
            Id = MaxId++;
        }

        public bool Get(string name, out SymbolTableEntry result)
        {
            SymbolTable cur = this;

            result = null;

            while (cur != null)
            {
                SymbolTableEntry entry = cur.Entries.FirstOrDefault(x => x.Name == name);

                if (entry != null)
                {
                    result =  entry;
                    return true;
                }

                cur = cur.Parent;
            }

            return false;
        }

        public SymbolTableEntry Create(string name, SymbolTableEntryType type)
        {
            SymbolTableEntry entry = new SymbolTableEntry
            {
                Name = name,
                Type = type
            };
            
            Entries.Add(entry);

            return entry;
        }

        public SymbolTable CreateChild()
        {
            SymbolTable st = new SymbolTable()
            {
                Parent = this
            };

            Children.Add(st);

            return st;
        }

        internal SymbolTableEntryType GetEntryType(Token n)
        {
            //if (n == Word.Integer)
            //{
            //    return SymbolTableEntryType.Integer;
            //}
            //else if (n == Word.Float)
            //{
            //    return SymbolTableEntryType.Float;
            //}
            //else if (n == Word.Void)
            //{
            //    return SymbolTableEntryType.Void;
            //}
            //else if (n == Word.Bool)
            //{
            //    return SymbolTableEntryType.Bool;
            //}

            throw new Exception("Unknown entry type" + n.ToString());
        }

        public static SymbolTableEntryType StringToSymbolTableEntryType(string type)
        {
            switch (type)
            {
                case "string":
                    return SymbolTableEntryType.String;
                case "int":
                    return SymbolTableEntryType.Integer;
                case "bool":
                    return SymbolTableEntryType.Bool;
            }

            throw new Exception("Unknown entry type " + type);
        }

        public string ToDot()
        {
            string result = Id + "1111 [shape=plaintext,label=<<table>";

            foreach (SymbolTableEntry entry in Entries)
            {
                result += "<tr><td>" + entry.Name + "</td><td>" + Enum.GetName(typeof(SymbolTableEntryType), entry.Type) + "</td></tr>";
            }

            result += "</table>>]\r\n";

            foreach(SymbolTable child in Children)
            {
                if (child.Entries.Count > 0)
                {
                    result += child.ToDot();

                    result += $"{child.Id}1111 -> {Id}1111\r\n";
                }
            }

            return result;
        }
    }
}
