using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto2015
{
	public interface IElement
	{
		int X { get; set; }
		int Y { get; set; }
		void Update(bool colision);
		void Draw();
	}
}
