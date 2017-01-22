using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo;

namespace Atmo2.Movements.PlayerStates
{
    public interface IPlayerState
    {
        void OnEnter();
        IPlayerState Update(GameTime time);
        void OnExit();
    }
}
