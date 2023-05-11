using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestConsole
{
    public class Player
    {

        private static Room _currentRoom;
        private List<string> _inventory;
        private static int _roomIndex;
        public Player(int startRoom)
        {
            _roomIndex = startRoom;
            _inventory = new List<string>();
            _currentRoom = Environment.Scene[_roomIndex];
        }
        public static Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        public List<string> Inventory { get { return _inventory; } }

        public static int RoomIndex { get { return _roomIndex; } set { _roomIndex = value; } }

    }
}
