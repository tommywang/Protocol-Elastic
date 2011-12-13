using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageLibrary
{
    class MessageItem
    {
        private byte[] size;
        public byte[] Size { get { return size; } set { size = value; } }
        private byte[] content;
        public byte[] Content { get { return content; } set { content = value; } }
        
        public MessageItem(byte[] size, byte[] content)
        {
            Size = size;
            Content = content;
        }

        public MessageItem(byte[] content)
        {
            Size =BitConverter.GetBytes(content.Length);
            Content = content;
        }

        public MessageItem(String content)
        {
            Content = Encoding.UTF8.GetBytes(content);
            Size = BitConverter.GetBytes(Content.Length);
        }

        public String DecoderItem()
        {
            return Encoding.UTF8.GetString(Content);
        }

        //
        public int getItemLength()
        {
            int length = BitConverter.ToInt32(Size, 0);
            return length + 4;
        }

        //retourner un tableau de byte contenant la concatenation de la size et le contenu
        public byte[] getItemSizeAndContent()
        {
            byte[] tmp= new byte[Size.Length+Content.Length];
            Size.CopyTo(tmp, 0);
            Content.CopyTo(tmp, Size.Length);
            return tmp;
        }
    }
}
