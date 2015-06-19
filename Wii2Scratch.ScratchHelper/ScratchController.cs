using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Wii2Scratch.ScratchHelper
{
	public class ScratchController : ApiController
	{
		static object s_lock = new object();
		static List<string> s_pollInputs = new List<string>();

		[Route( "poll" )]
		[HttpGet]
		public HttpResponseMessage Poll()
		{
			var resp = new HttpResponseMessage( HttpStatusCode.OK );
			var content = new StringBuilder();

			foreach ( var controller in AppState.WiiControllers )
			{
				content.AppendFormat( "button/up/{0} {1}\n", controller.Index, BoolToString( controller.ButtonUp) );
				content.AppendFormat( "button/right/{0} {1}\n", controller.Index, BoolToString( controller.ButtonRight ) );
				content.AppendFormat( "button/left/{0} {1}\n", controller.Index, BoolToString( controller.ButtonLeft ) );
				content.AppendFormat( "button/down/{0} {1}\n", controller.Index, BoolToString( controller.ButtonDown ) );
				content.AppendFormat( "button/A/{0} {1}\n", controller.Index, BoolToString( controller.ButtonA ) );
				content.AppendFormat( "button/B/{0} {1}\n", controller.Index, BoolToString( controller.ButtonB ) );
				content.AppendFormat( "button/-/{0} {1}\n", controller.Index, BoolToString( controller.ButtonMin ) );
				content.AppendFormat( "button/home/{0} {1}\n", controller.Index, BoolToString( controller.ButtonHome ) );
				content.AppendFormat( "button/+/{0} {1}\n", controller.Index, BoolToString( controller.ButtonPlus ) );
				content.AppendFormat( "button/1/{0} {1}\n", controller.Index, BoolToString( controller.Button1 ) );
				content.AppendFormat( "button/2/{0} {1}\n", controller.Index, BoolToString( controller.Button2 ) );

				content.AppendFormat( "rotation/x/{0} {1}\n", controller.Index, (int)( controller.RotationX + 0.5 ) );
				content.AppendFormat( "rotation/y/{0} {1}\n", controller.Index, (int)( controller.RotationY + 0.5 )  );
				content.AppendFormat( "rotation/z/{0} {1}\n", controller.Index, (int)( controller.RotationZ + 0.5 ) );

				content.AppendFormat( "battery/{0} {1}\n", controller.Index, controller.BatteryPercent );
			}
						
			resp.Content = new StringContent( content.ToString(), Encoding.UTF8, "text/plain" );
			return resp;
		}

		private void AppendControllerButton( StringBuilder builder, WiiController controller, string buttonName, Func<WiiController, bool> buttonState )
		{
			builder.Append( "button/" );
			builder.Append( buttonName );
			builder.Append( "/" );
			builder.Append( controller.Index );
			builder.Append( " " );
			builder.Append( buttonState( controller ) ? "true" : "false" );
			builder.Append( '\n' );
		}

		[Route( "led/{ledIndex}/{controllerIndex}/{onOff}" )]
		[HttpGet]
		public HttpResponseMessage Led( string ledIndex, string controllerIndex, string onOff )
		{
			int li;
			if ( !int.TryParse( ledIndex, out li ) )
			{
				return this.OkResponse; ;
			}

			int ci;
			if ( !int.TryParse( controllerIndex, out ci ) )
			{
				return this.OkResponse; ;
			}

			var controller = AppState.WiiControllers.FirstOrDefault( c => c.Index == ci );
			if ( controller == null )
			{
				return this.OkResponse; ;
			}

			controller.SetLed( li, onOff.ToLower() == "on" );
			return this.OkResponse;
		}

		[Route( "rumble/{controllerIndex}/{onOff}" )]
		[HttpGet]
		public HttpResponseMessage Led( string controllerIndex, string onOff )
		{
			int ci;
			if ( !int.TryParse( controllerIndex, out ci ) )
			{
				return this.OkResponse; ;
			}

			var controller = AppState.WiiControllers.FirstOrDefault( c => c.Index == ci );
			if ( controller == null )
			{
				return this.OkResponse; ;
			}

			controller.SetRumble( onOff.ToLower() == "on" );
			return this.OkResponse;
		}

		private string BoolToString( bool b )
		{
			return b ? "true" : "false";
		}

		private HttpResponseMessage OkResponse
		{
			get
			{
				var resp = new HttpResponseMessage( HttpStatusCode.OK );
				resp.Content = new StringContent( string.Empty, Encoding.UTF8, "text/plain" );
				return resp;
			}
		}
	}
}
