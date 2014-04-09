using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Cache
    {
        CacheBlock[] blocks;
        private int blockSize;
        public int hits =0;
        public int misses=0;
        private int Size;
        public Cache(int size, int blocksize)
        {
            blocks = new CacheBlock[size];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new CacheBlock(blocksize,257);
            }
            blockSize = blocksize;
            Size = size;
        }
        public int getValueAt(int index){
            int g;
            try{
                if (Memory.direct)
                {
                    g = blocks[(((index+1) % blockSize)%Size)].getValueAt(index);
                }
                else
                {
                    try
                    {
                        g = blocks[(((index+1) % blockSize)%Size)*2].getValueAt(index);
                    }
                    catch (MissException)
                    {
                        g = blocks[(((index+1) % blockSize)%Size)*2+1].getValueAt(index);
                    }
                }
                hits++;
                return g;
            }catch(MissException e){
                misses++;
                replaceBlock(index);
                return Memory.getStackAt(index);
            }
        }
        public void setValueAt(int index, int value)
        {
            try { 
            if (Memory.direct)
            {
                blocks[((index+1) % (blockSize))%Size].writeValue(index, value);
            }
            else
            {
                try
                {
                    blocks[(((index+1) % blockSize)%Size) * 2].writeValue(index,value);
                }
                catch (MissException)
                {
                    blocks[(((index+1) % blockSize)%Size) * 2 + 1].writeValue(index,value);
                }
            }
            }
            catch (MissException)
            {
                Memory.setStackAt(index, value);
                replaceBlock(index);
            }
        }
        private void replaceBlock(int index)
        {
            if(Memory.direct){
                int[] newblock = new int[blockSize];
                for(int i = 0;i<blockSize;i++){
                    newblock[i]=Memory.getStackAt(index+i);
                }
                blocks[(index%blockSize)%Size].replaceBlock(newblock,index);
            }
            else
            {
                Random whatever = new Random();
                int g = whatever.Next(1);
                int[] newblock = new int[blockSize];
                for (int i = 0; i < blockSize; i++)
                {
                    newblock[i] = Memory.getStackAt(index + i);
                }
                blocks[(index % blockSize)*2+g].replaceBlock(newblock, index);
            }
        }
    }
}
