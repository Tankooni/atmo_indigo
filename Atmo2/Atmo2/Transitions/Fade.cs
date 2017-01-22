using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Glide;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;

namespace Atmo2.Transitions
{
    public class Fade : Entity
    {
        private Image texture;
        private bool isFading;
        public float Alpha
        {
            get { return texture.Alpha; }
            set { texture.Alpha = value; }
        }

        public Fade(Color color, int fadeBuffer, float fadeIncrement)
        {
            this.texture = new Image(
                Library.Get<Texture>("content/image/white.png"));
            texture.ScaleX = Engine.Width + fadeBuffer;
            texture.ScaleY = Engine.Height + fadeBuffer;
            this.RenderStep = 999999;
            texture.Color = color;
            texture.ScrollX = 0;
            texture.ScrollY = 0;

            AddResponse(Door.DoorMessages.StartChangeRoom, (args) =>
            {
                if (!isFading) {
                    Tweener.Tween(this, new { Alpha = 1 }, 1)
                        .From(new { Alpha = 0 })
                        .Ease(Ease.ToAndFro)
                        .OnComplete(() => isFading = false);
                }
            });


            this.Alpha = 0;
            this.AddComponent(texture);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            /*
            if (changeToNewRoom1)
            {
                if (!changeToNewRoom2 && (fadeToBlackImage.Alpha += fadeIncrement) >= 1)
                {
                    ActuallyChangeRoom();
                    changeToNewRoom2 = true;
                }
                else if (changeToNewRoom2 && (fadeToBlackImage.Alpha -= fadeIncrement) <= 0)
                {
                    changeToNewRoom1 = changeToNewRoom2 = false;
                    Remove(fadeToBlack);
                }
                return;
            }*/
        }
    }
}
