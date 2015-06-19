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
				content.AppendFormat( "rotation/up/{0} {1}\n", BoolToString( controller.ButtonUp) );
				content.AppendFormat( "rotation/right/{0} {1}\n", BoolToString( controller.ButtonRight) );
				content.AppendFormat( "rotation/left/{0} {1}\n", BoolToString( controller.ButtonLeft) );
				content.AppendFormat( "rotation/down/{0} {1}\n", BoolToString( controller.ButtonDown) );
				content.AppendFormat( "rotation/A/{0} {1}\n", BoolToString( controller.ButtonA) );
				content.AppendFormat( "rotation/B/{0} {1}\n", BoolToString( controller.ButtonB) );
				content.AppendFormat( "rotation/-/{0} {1}\n", BoolToString( controller.ButtonMin) );
				content.AppendFormat( "rotation/home/{0} {1}\n", BoolToString( controller.ButtonHome) );
				content.AppendFormat( "rotation/+/{0} {1}\n", BoolToString( controller.ButtonPlus) );
				content.AppendFormat( "rotation/1/{0} {1}\n", BoolToString( controller.Button1) );
				content.AppendFormat( "rotation/2/{0} {1}\n", BoolToString( controller.Button2) );

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
