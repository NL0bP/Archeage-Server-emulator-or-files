using ArcheAge.ArcheAge.Structuring;
using LocalCommons.Logging;
using LocalCommons.Network;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ArcheAge.ArcheAge.Network.Connections
{

    /// <summary>
    /// Connection That Used For ArcheAge Client( Game Side )
    /// </summary>
    public class ClientConnection : IConnection
    {
        //----- Static
        private static Dictionary<int, Account> m_CurrentAccounts = new Dictionary<int, Account>();
        private byte m_Random;
        public byte m_NumPck = 0;  //修复第二用户、二次登陆、大厅返回重连DD05计数器造成错误问题 BUGглобальный подсчет пакетов DD05
        public static Dictionary<int, Account> CurrentAccounts
        {
            get { return m_CurrentAccounts; }
        }

        public Account CurrentAccount { get; set; }

        public ClientConnection(Socket socket) : base(socket) {
            Logger.Trace("Client IP: {0} connected", this);
            DisconnectedEvent += ClientConnection_DisconnectedEvent;
            m_LittleEndian = true;
        }

        public override void SendAsync(NetPacket packet)
        {
            packet.IsArcheAgePacket = true;
			NetPacket.m_NumPck = m_NumPck;//重写为当前连接的计数
            base.SendAsync(packet);
			m_NumPck = NetPacket.m_NumPck;//将计数回写
        }
        public void SendAsyncd(NetPacket packet)
        {
            packet.IsArcheAgePacket = false;
            base.SendAsync(packet);
        }
        void ClientConnection_DisconnectedEvent(object sender, EventArgs e)
        {
            Logger.Trace("Client IP: {0} disconnected", this);
            Dispose();
        }

        public override void HandleReceived(byte[] data)
        {
            PacketReader reader = new PacketReader(data, 0);
            //reader.Offset += 1; //Undefined Random Byte
            byte seq = reader.ReadByte();
            byte header = reader.ReadByte(); //Packet Level
            ushort opcode = reader.ReadLEUInt16(); //Packet Opcode
            //пока не смог дешифровать клиентские пакеты, придется делать так
            if (header == 0x05)
            {
                reader.Offset -= 2; //вернемся к hash, count
                byte hash = reader.ReadByte(); //считываем hash или CRC (он не меняется)
                byte count = reader.ReadByte(); //считываем count (шифрован, меняется)
                switch (hash)
                {
                    case 0x33:
                        opcode = 0x008E; //вход в игру5
                        break;
                    case 0x34:
                        //opcode = 0x0088; //пакет на релогин
                        break;
                    case 0x36:
                        opcode = 0x008F; //вход в игру6
                        break;
                    case 0x37:
                        opcode = 0x008B; //вход в игру2
                        break;
                    case 0x38:
                        opcode = 0x008A; //вход в игру1
                        break;
                    case 0x39:
                        opcode = 0x008C; //вход в игру3
                        break;
                    case 0x3F:
                        opcode = 0x008D; //вход в игру4
                        break;
                        //default:
                        //    msg = "";
                        //    break;
                }
                //reader.Offset -= 2; //Undefined Random Byte
            }

            if (!DelegateList.ClientHandlers.ContainsKey(header))
            {
                Logger.Trace("Received undefined packet - seq: {0}, header: {1}, opcode: 0x{2:X2}", seq, header, opcode);
                return;
            }
            try { 
            PacketHandler<ClientConnection> handler = DelegateList.ClientHandlers[header][opcode];
                if (handler != null)
                    handler.OnReceive(this, reader);
                else
                    Logger.Trace("Received undefined packet - seq: {0}, header: {1}, opcode: 0x{2:X2}", seq, header, opcode);
            }
            catch (Exception)
            {
                Logger.Trace("Received undefined packet - seq: {0}, header: {1}, opcode: 0x{2:X2}", seq, header, opcode);
                throw;
            }
        }
    }
}
