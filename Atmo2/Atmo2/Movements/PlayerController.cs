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
        IPlayerState next_state;

        public PlayerController(IPlayerState initial_state)
        {
            this.current_state = initial_state;
            initial_state.OnEnter();
        }

        public void Update(GameTime time)
        {
            var newState = current_state.Update(time);

            // Transision states if needed
            if (next_state != null)
            {
                newState = next_state;
                next_state = null;
            }

            if(newState != null)
            {
                current_state.OnExit();
                newState.OnEnter();
                current_state = newState;
            }
        }

        public void NextState(IPlayerState state)
        {
            this.next_state = state;
        }
    }
}
