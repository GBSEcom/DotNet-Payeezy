//For TESTING PURPOSES ONLY. NOT FOR PRODUCTION USE
using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Data;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
 
public partial class PayeezyCard : System.Web.UI.Page
{
	public byte[] CalculateHMAC(string data, string secret)
		{
			HMAC hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
			byte[] dataBytes = Encoding.UTF8.GetBytes(data);
			byte[] hmac2Hex = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));
		 
			string hex = BitConverter.ToString(hmac2Hex);
			hex = hex.Replace("-","").ToLower();
			byte[] hexArray = Encoding.UTF8.GetBytes(hex);
			return hexArray;
		}
		
	protected void Button1_Click(object sender, EventArgs e)
	{                     
        var paymentCard = new
        {
            type = "visa",
            cardholder_name = "Test Name",
            card_number = "4005519200000004",
            exp_date = "1020",
            cvv = "123"
        };

        var payload = new
        {
            merchant_ref = "MVC TEST",
            transaction_type = "authorize",
            method = "credit_card",
            amount = "1299",
            currency_code = "USD",
            credit_card = paymentCard
        };

		string payloadJson = JsonConvert.SerializeObject(payload);

		Random random = new Random();
		string nonce = (random.Next(0, 1000000)).ToString();

		string time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();


		string token = Request.Form["token"];//Merchant token
		string apiKey = Request.Form["apikey"];//apikey
		string apiSecret = Request.Form["apisecret"];//API secret
		string hashData = apiKey+nonce+time+token+payloadJson;
		
		string base64Hash = Convert.ToBase64String(CalculateHMAC(hashData, apiSecret));
		
		string url = "https://api-cert.payeezy.com/v1/transactions";
	   
		//begin HttpWebRequest
		HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

		webRequest.Method = "POST";
		webRequest.Accept = "*/*";
		webRequest.Headers.Add("timestamp", time);
		webRequest.Headers.Add("nonce", nonce);
		webRequest.Headers.Add("token", token);
		webRequest.Headers.Add("apikey", apiKey);
		webRequest.Headers.Add("Authorization", base64Hash );
		webRequest.ContentLength = payloadJson.Length;
		webRequest.ContentType = "application/json";

		StreamWriter writer = null;
		writer = new StreamWriter(webRequest.GetRequestStream());
		writer.Write(payloadJson);
		writer.Close();

		string responseString;
		try
			{
				using(HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
				{
					using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
					{
						responseString = responseStream.ReadToEnd();
						request_label.Text = "<h3>Request</h3><br />" + webRequest.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(payloadJson);
						response_label.Text = "<h3>Response</h3><br />" + webResponse.Headers.ToString() + System.Web.HttpUtility.HtmlEncode(responseString);
					}
				}
			}
		catch (WebException ex)
		{
			if (ex.Response != null) 
			{
				using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response) 
				{
					using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream())) 
					{
						string remoteEx = reader.ReadToEnd();
						error.Text = remoteEx;
					}
				}
			}           
		}
	}
}