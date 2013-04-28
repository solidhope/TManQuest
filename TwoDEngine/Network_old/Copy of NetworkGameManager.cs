using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine.Network
{
    public interface NetworkGameManager
    {
        NetworkGame CreateGame(String name, String description);
        NetworkGame[] ListGames();
    }
}
