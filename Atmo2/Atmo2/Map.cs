using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Atmo2.Worlds.Rooms;
using Indigo;
using Indigo.Inputs;
using Indigo.Loaders;

namespace Atmo2
{
    class Map : Entity
    {
        public MapRoom[] mapRooms;
        public new bool Visible {
            get { return base.Visible; }
            set {
                base.Visible = value;
                foreach (var r in mapRooms) r.Visible = value;
            }
        }
        private float scale;
        public float Scale
        {
            get { return Camera.Scale; }//return scale; }
            set {
                //scale = value;
                //foreach (var r in mapRooms) r.Scale = value;
                Camera.Scale = value;
            }
        }

        public new int RenderStep
        {
            get { return base.RenderStep; }
            set
            {
                base.RenderStep = value;
                foreach (var r in mapRooms) r.RenderStep = value;
            }
        }

        public Map(string map_filename)
        {
            OgmoLoader ogmo = new OgmoLoader();
            ogmo.RegisterClassAlias<MapRoom>("Room");
            var mapRoomEntities = ogmo.BuildLevelAsArray(
                Library
                .Get<XmlDocument>(map_filename));
            this.mapRooms = new MapRoom[mapRoomEntities.Length];
            Array.Copy(mapRoomEntities, this.mapRooms, this.mapRooms.Length);

            this.Visible = false;
            this.RenderStep = -100;

            this.Camera = new Camera();
            foreach (var r in mapRooms) r.Camera = this.Camera;
            this.Scale *= .7f;
        }

        public override void Update()
        {
            if(Keyboard.Space.Pressed)
            {
                this.Visible = !this.Visible;
            }

            base.Update();
        }

        public override void Added()
        {
            base.Added();
            this.World.AddList(this.mapRooms);
        }

        public override void Removed()
        {
            base.Removed();
            this.World.RemoveList(this.mapRooms);
        }
    }
}
