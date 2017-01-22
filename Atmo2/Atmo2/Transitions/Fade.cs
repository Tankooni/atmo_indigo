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
        public enum FadeMessages
        {
            StartFadeIn,
            StartFadeOut,
            OnFadeInCompleted,
            OnFadeOutCompleted
        }

        private Image texture;
        private bool isFading;
        private const int FADEBUFFER = 30;

        private float duration;
        public float Alpha
        {
            get { return texture.Alpha; }
            set { texture.Alpha = value; }
        }

        public Fade(Color color, float duration)
        {
            this.texture = new Image(
                Library.Get<Texture>("content/image/white.png"));
            texture.ScaleX = Engine.Width + FADEBUFFER;
            texture.ScaleY = Engine.Height + FADEBUFFER;
            this.RenderStep = 999999;
            texture.Color = color;
            texture.ScrollX = 0;
            texture.ScrollY = 0;

            this.duration = duration;
            this.Alpha = 0;
            this.AddComponent(texture);
        }

        public void FadeIn(Action f = null)
        {
            if (!isFading)
            {
                isFading = true;
                Tweener.Tween(this, new { Alpha = 1 }, duration)
                    .From(new { Alpha = 0 })
                    .Ease((t) => t)
                    .OnComplete(() => {
                        isFading = false;
                        f?.Invoke();
                    });
            }
        }
        public void FadeOut(Action f = null)
        {
            if (!isFading)
            {
                isFading = true;
                Tweener.Tween(this, new { Alpha = 0 }, duration)
                    .From(new { Alpha = 1 })
                    .Ease((t) => t)
                    .OnComplete(() => {
                        isFading = false;
                        f?.Invoke();
                    });
            }
        }
    }
}
