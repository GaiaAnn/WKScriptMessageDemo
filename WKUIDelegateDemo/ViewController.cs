using System;
using UIKit;
using WebKit;
using Foundation;
using System.IO;


namespace WKScriptMessageDemo
{
	public partial class ViewController : UIViewController , IWKScriptMessageHandler 
	{
		WKWebView webview_real;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
		{
			var msg = message.Body.ToString();
			System.Diagnostics.Debug.WriteLine(msg);

			//C# call JavaScript
			webview_real.EvaluateJavaScript("ChangeValue('"+ msg +"')", (result, error) =>			
			{
				if (error != null) Console.WriteLine(error);
			});
		}


		public void AppDomainInitializer()
		{
			var userController = new WKUserContentController();
			userController.AddScriptMessageHandler(this, "myapp");

			var config = new WKWebViewConfiguration
			{
				UserContentController = userController
			};

			webview_real = new WKWebView(View.Frame, config);
			View.AddSubview(webview_real);

			#region load from file
			string htmlPath = NSBundle.MainBundle.PathForResource("Alerts", "html");
			string htmlContents = File.ReadAllText(htmlPath);
			webview_real.LoadHtmlString(htmlContents, null);
			#endregion


			#region load from url sample
			//var url = "https://www.google.com.tw";
			//webview_real.LoadRequest(new NSUrlRequest(new NSUrl(url)));
			#endregion

		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			AppDomainInitializer();
		}

	}
}

