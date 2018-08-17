using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LocalCommons;
using LocalCommons.Cryptography;

namespace LocalCommons.Network
{
    /// <summary>
    /// Abstract Class For Writing Packets
    /// Author: Raphail
    /// </summary>
    public abstract class NetPacket
    {
        protected PacketWriter ns;
        private int packetId;
        private bool m_littleEndian;
        private bool m_IsArcheAge;
        private byte level;
        public static byte m_NumPck = 0;  //修复第二用户、二次登陆、大厅返回重连DD05计数器造成错误问题 BUGглобальный подсчет пакетов DD05


        public bool IsArcheAgePacket
        {
            get { return m_IsArcheAge; }
            set { m_IsArcheAge = true; }
        }

    /// <summary>
    /// Creates Instance Of Any Other Packet
    /// </summary>
    /// <param name="packetId">Packet Identifier(opcode)</param>
    /// <param name="isLittleEndian">Send Data In Little Endian Or Not.</param>
    protected NetPacket(int packetId, bool isLittleEndian)
        {
            this.packetId = packetId;
            this.m_littleEndian = isLittleEndian;
            ns = PacketWriter.CreateInstance(4092, isLittleEndian);
        }

        /// <summary>
        /// Creates Instance Of ArcheAge Game Packet.
        /// </summary>
        /// <param name="level">Packet Level</param>
        /// <param name="packetId">Packet Identifier(opcode)</param>
        protected NetPacket(byte level, int packetId)
        {
            this.packetId = packetId;
            this.level = level;
            this.m_littleEndian = true;
            this.m_IsArcheAge = true;
            ns = PacketWriter.CreateInstance(4092, true);
        }

        /// <summary>
        /// Stream Where We Writing Data.
        /// </summary>
        public PacketWriter UnderlyingStream
        {
            get { return ns; }
        }

        /// <summary>
        /// Compiles Data And Return Compiled byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] Compile()
        {
            PacketWriter temporary = PacketWriter.CreateInstance(4092 * 4, m_littleEndian);
            //temporary.Write((short)(ns.Length + (m_IsArcheAge ? 6 : 2)));
            if (m_IsArcheAge)
            {
                if (level == 5)
                {
                    //здесь будет шифрование пакета DD05
                    temporary.Write((short)(ns.Length + 6));

                    temporary.Write((byte)0xDD);
                    temporary.Write((byte)level);

                    //TODO: посмотреть, может по другому написать, через copy?
                    byte[] numPck = new byte[1];
                    numPck[0] = m_NumPck; //вставили номер пакета в массив
                    byte[] data = numPck.Concat(BitConverter.GetBytes((short)packetId)).ToArray(); //объединили с ID
                    data = data.Concat(ns.ToArray()).ToArray(); //объединили с телом пакета
                    byte crc = Encryption._CRC8_(data); //посчитали CRC пакета
                    byte[] CRC = new byte[1];
                    CRC[0] = crc; //вставили crc в массив
                    data = CRC.Concat(data).ToArray(); //добавили спереди контрольную сумму
                    byte[] encrypt = Encryption.StoCEncrypt(data); //зашифровали пакет
                    temporary.Write(encrypt, 0, encrypt.Length);
                    ++m_NumPck; //следующий номер шифрованного пакета DD05
                }
                else
                {
                    temporary.Write((short)(ns.Length + 4));

                    temporary.Write((byte)0xDD);
                    temporary.Write((byte)level);
                    temporary.Write((short)packetId);

                    byte[] redata = ns.ToArray();
                    temporary.Write(redata, 0, redata.Length);
                }
            }
            else
            {
                temporary.Write((short)(ns.Length + 2));
                temporary.Write((short)packetId);
                byte[] redata = ns.ToArray();
                temporary.Write(redata, 0, redata.Length);
            }
            PacketWriter.ReleaseInstance(ns);
            ns = null;
            byte[] compiled = temporary.ToArray();
            PacketWriter.ReleaseInstance(temporary);
            temporary = null;

            return compiled;
        }
        /// <summary>
        /// Compiles Data And Return Compiled byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] Compile0()
        {
            PacketWriter temporary = PacketWriter.CreateInstance(8192 * 4, m_littleEndian);
            temporary.Write((short)(ns.Length + (m_IsArcheAge ? 4 : 2)));
            if (m_IsArcheAge)
            {
                temporary.Write((byte)0xDD);
                temporary.Write((byte)level);
                temporary.Write((short)packetId);
            }
            else
                temporary.Write((short)packetId);
            byte[] redata = ns.ToArray();
            PacketWriter.ReleaseInstance(ns);
            ns = null;
            temporary.Write(redata, 0, redata.Length);
            byte[] compiled = temporary.ToArray();
            PacketWriter.ReleaseInstance(temporary);
            temporary = null;
            return compiled;
        }
        /// <summary>
        /// Compiles Data And Return Compiled byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] Compile2()
        {
            PacketWriter temporary = PacketWriter.CreateInstance(4092 * 4, m_littleEndian);
          
            byte[] redata = ns.ToArray();
            PacketWriter.ReleaseInstance(ns);
            ns = null;
            temporary.Write(redata, 0, redata.Length);
            byte[] compiled = temporary.ToArray();
            PacketWriter.ReleaseInstance(temporary);
            temporary = null;
            return compiled;
        }
    }
}
