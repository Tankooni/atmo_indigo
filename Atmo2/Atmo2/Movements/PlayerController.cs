using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Movements.PlayerStates;
using Indigo;

namespace Atmo2.Movements
{
    public class PlayerController
    {
        IPlayerState current_state;

        public PlayerController(IPlayerState initial_state)
        {
            this.current_state = initial_state;
            initial_state.OnEnter();
        }

        public void Update(GameTime time)
        {
            var newState = current_state.Update(time);

            // Transision states if needed
            if(newState != null)
            {
                current_state.OnExit();
                newState.OnEnter();
                current_state = newState;
            }
        }
    }
}
