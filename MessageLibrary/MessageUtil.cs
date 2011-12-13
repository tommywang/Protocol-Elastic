using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageLibrary
{
    public class MessageUtil
    {
        public static byte[] encoder(Message message)
        {
            int location = 0;
            byte[] finalMessage = new byte[getMessageSize(message) + 4];
            byte[] totalSizeMessage = BitConverter.GetBytes(getMessageSize(message));
            totalSizeMessage.CopyTo(finalMessage, location);
            location += 4;
            MessageItem mSource = new MessageItem(message.Source);
            mSource.getItemSizeAndContent().CopyTo(finalMessage, location);
            location += mSource.getItemLength();

            MessageItem mTarget = new MessageItem(message.Target);
            mTarget.getItemSizeAndContent().CopyTo(finalMessage, location);
            location += mTarget.getItemLength();

            MessageItem mOperation = new MessageItem(message.Operation);
            mOperation.getItemSizeAndContent().CopyTo(finalMessage, location);
            location += mOperation.getItemLength();

            MessageItem mStamp = new MessageItem(message.Stamp);
            mStamp.getItemSizeAndContent().CopyTo(finalMessage, location);
            location += mStamp.getItemLength();

            BitConverter.GetBytes(message.ListParams.Count).CopyTo(finalMessage, location);
            location += 4;

            foreach (string param in message.ListParams)
            {
                MessageItem mParam = new MessageItem(param);
                mParam.getItemSizeAndContent().CopyTo(finalMessage, location);
                location += mParam.getItemLength();
            }

            return finalMessage;
        }

        public static Message decoder(byte[] restMessage)
        {
            byte[] sizeSource = readNByte(restMessage, 4);
            restMessage = removeNByte(restMessage, 4);
            byte[] contentSource = readNByte(restMessage, byteToInt(sizeSource));
            restMessage = removeNByte(restMessage, byteToInt(sizeSource));

            MessageItem source = new MessageItem(sizeSource, contentSource);

            byte[] sizeTarget = readNByte(restMessage, 4);
            restMessage = removeNByte(restMessage, 4);
            byte[] contentTarget = readNByte(restMessage, byteToInt(sizeTarget));
            restMessage = removeNByte(restMessage, byteToInt(sizeTarget));
            MessageItem target = new MessageItem(sizeTarget, contentTarget);

            byte[] sizeOperation = readNByte(restMessage, 4);
            restMessage = removeNByte(restMessage, 4);
            byte[] contentOperation = readNByte(restMessage, byteToInt(sizeOperation));
            restMessage = removeNByte(restMessage, byteToInt(sizeOperation));
            MessageItem operation = new MessageItem(sizeOperation, contentOperation);

            byte[] sizeStamp = readNByte(restMessage, 4);
            restMessage = removeNByte(restMessage, 4);
            byte[] contentStamp = readNByte(restMessage, byteToInt(sizeStamp));
            restMessage = removeNByte(restMessage, byteToInt(sizeStamp));
            MessageItem stamp = new MessageItem(sizeStamp, contentStamp);

            byte[] paramsCount = readNByte(restMessage, 4);
            restMessage = removeNByte(restMessage, 4);

            List<string> listParams = new List<string>();

            for (int i = 0; i < byteToInt(paramsCount); i++)
            {
                byte[] sizeParam = readNByte(restMessage, 4);
                restMessage = removeNByte(restMessage, 4);
                byte[] contentParam = readNByte(restMessage, byteToInt(sizeParam));
                restMessage = removeNByte(restMessage, byteToInt(sizeParam));
                listParams.Add((new MessageItem(sizeParam, contentParam)).DecoderItem());
            }


            return new Message(source.DecoderItem(), target.DecoderItem(), operation.DecoderItem()
                , stamp.DecoderItem(), listParams);
        }


        public static byte[] readNByte(byte[] messageByte, int n)
        {
            byte[] restByte = new byte[n];
            for (int i = 0; i < n; i++)
            {
                restByte[i] = messageByte[i];
            }
            return restByte;
        }

        public static byte[] removeNByte(byte[] messageByte, int n)
        {
            int size = messageByte.Length - n;
            byte[] restByte = new byte[size];
            for (int i = n; i < messageByte.Length; i++)
            {
                restByte[i - n] = messageByte[i];
            }
            return restByte;
        }

        public static int byteToInt(byte[] msg)
        {
            return BitConverter.ToInt32(msg, 0);

        }

        private static int getMessageSize(Message m)
        {

            int size = (new MessageItem(m.Source)).getItemLength() + (new MessageItem(m.Target)).getItemLength()
                + (new MessageItem(m.Operation)).getItemLength() + (new MessageItem(m.Stamp)).getItemLength()
                + 4; //4 pour le param count

            foreach (string param in m.ListParams)
            {
                size += (new MessageItem(param)).getItemLength();
            }
            return size;
        }
    }

    [Serializable()]
    public class MessageLengthExceptionBis : System.Exception
    {
        public MessageLengthExceptionBis(string message) : base(message) { }

    }
}
