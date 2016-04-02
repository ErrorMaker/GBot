using Sulakore.Communication;
using Sulakore.Habbo;
using Sulakore.Protocol;
using System;
using Sulakore;
namespace giopib
{
    public class WiredMoveFurniture
    {
        private readonly HMessage _packet;
        public ushort Header { get; private set; }
        public int xPos1 { get; private set; }
        public int yPos1 { get; private set; }
        public int xPos2 { get; private set; }
        public int yPos2 { get; private set; }
        public int state { get; private set; }
        public int FurniID { get; private set; }
        public string z1 { get; private set; }
        public string z2 { get; private set; }
        public int number { get; private set; }

        public WiredMoveFurniture(HMessage packet)
        {
            _packet = packet;
            Header = _packet.Header;
            xPos1 = _packet.ReadInteger();
            yPos1 = _packet.ReadInteger();
            xPos2 = _packet.ReadInteger();
            yPos2 = _packet.ReadInteger();
            state = _packet.ReadInteger();
            FurniID = _packet.ReadInteger();
            z1 = _packet.ReadString();
            z2 = _packet.ReadString();
            number = _packet.ReadInteger();

        }

        public override string ToString()
        {
            return string.Format("Packet: {0}, Header: {1}, xPos1: {2}, yPos1: {3}, xPos2: {4}, yPos2: {5}, state: {6}, FurniId: {7}, z1: {8}, z2: {9}, number: {9}", _packet, Header, xPos1, yPos1, xPos2, yPos2, state, FurniID, z1, z2, number);
        }
    }
}