using System;
using System.Threading.Tasks;

namespace Assembler
{
    internal class Cache
    {
        private readonly int blockSize;
        private readonly CacheBlock[] blocks;
        private readonly int size;
        public int hits = 0;
        public int misses = 0;

        public Cache(int size, int blocksize)
        {
            blocks = new CacheBlock[size];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new CacheBlock(blocksize, 257);
            }
            blockSize = blocksize;
            this.size = size;
        }

        public int getValueAt(int index)
        {
            int g;
            try
            {
                if (Memory.directCache)
                {
                    g = blocks[(((index)/blockSize + 1)%size)].getValueAt(index);
                }
                else
                {
                    try
                    {
                        g = blocks[(((index)/blockSize)*2 + 1)%size].getValueAt(index);
                    }
                    catch (MissException)
                    {
                        g = blocks[(((index)/blockSize)*2 + 2)%size].getValueAt(index);
                    }
                }
                hits++;
                return g;
            }
            catch (MissException e)
            {
                misses++;
                replaceBlock(index);
                return Memory.getStackAt(index);
            }

        }

        public void setValueAt(int index, int value)
        {
            try
            {
                if (Memory.directCache)
                {
                    blocks[((index)/(blockSize) + 1)%size].writeValue(index, value);
                }
                else
                {
                    try
                    {
                        blocks[((((index)/blockSize))*2 + 1)%size].writeValue(index, value);
                    }
                    catch (MissException)
                    {
                        blocks[((((index)/blockSize))*2 + 2)%size].writeValue(index, value);
                    }
                }
                hits++;

            }
            catch (MissException)
            {
                
                misses++;
                Memory.setStackAt(index, value);
                replaceBlock(index);
            }
        }

        private void replaceBlock(int index)
        {
            if (Memory.directCache)
            {
                var newblock = new int[blockSize];
                for (int i = 0; i < blockSize; i++)
                {
                    newblock[i] = Memory.getStackAt(index + i);
                }
                blocks[(index/blockSize + 1)%size].replaceBlock(newblock, index);
            }
            else
            {
                var whatever = new Random();
                int g = whatever.Next(1);
                var newblock = new int[blockSize];
                for (int i = 0; i < blockSize; i++)
                {
                    newblock[i] = Memory.getStackAt(index + i);
                }
                blocks[((((index)/blockSize))*2 + 1 + g)%size].replaceBlock(newblock, index);
            }
        }
    }
}