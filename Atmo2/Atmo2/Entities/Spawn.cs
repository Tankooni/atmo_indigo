using Indigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Entities
{
	public class Spawn : Entity, Indigo.Loaders.IOgmoNodeHandler
	{


		public void NodeHandler(System.Xml.XmlNode entity)
		{
			new Player(X, Y).SetResetPointToCurrentLocation();
		}
	}
}
