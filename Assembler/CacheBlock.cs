using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class CacheBlock
    {
        public int tag;
        private int[] items;
        public  bool dirty;
        private int size;
        public CacheBlock(int size, int tag)
        {
            items = new int[size];
            dirty = false;
            this.size = size;
            this.tag = tag;
        }
        public int getValueAt(int index) 
        {
            if (index-tag > size || index-tag < 0)
            {
                throw new MissException();
            }
            else
            {
                return items[index-tag];
            }
        }
        public void writeValue(int index, int value)
        {
            if (index - tag > size || index - tag < 0)
            {
                throw new MissException();
            }
            else
            {
               items[index - tag]=value;
               dirty = true;
            }
        }
        public void replaceBlock(int[] values, int tag)
        {
            if (dirty)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    Memory.setStackAt(tag + i, items[i]);
                }
            }
            dirty = false;
            this.tag = tag;
            items = values;
        }

    }
}
