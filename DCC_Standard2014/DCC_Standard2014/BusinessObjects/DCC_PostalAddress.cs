using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace DCC_Standard
{

	#region DCC_PostalAddress

	public class DCC_PostalAddress
	{

		// Properties

		public string UPRN;
		public string UDPRN;
		public string ADDRESS;
		public string BUILDING_NUMBER;
		public string BUILDING_NAME;
		public string THOROUGHFARE_NAME;
		public string POST_TOWN;
		public string POSTCODE;
		public string RPC;
		public string X_COORDINATE;
		public string Y_COORDINATE;
		public string STATUS;
		public string LOGICAL_STATUS_CODE;
		public string CLASSIFICATION_CODE;
		public string CLASSIFICATION_CODE_DESCRIPTION;
		public string LOCAL_CUSTODIAN_CODE;
		public string LOCAL_CUSTODIAN_CODE_DESCRIPTION;
		public string POSTAL_ADDRESS_CODE;
		public string POSTAL_ADDRESS_CODE_DESCRIPTION;
		public string BLPU_STATE_CODE;
		public string BLPU_STATE_CODE_DESCRIPTION;
		public string TOPOGRAPHY_LAYER_TOID;
		public string LAST_UPDATE_DATE;
		public string ENTRY_DATE;
		public string BLPU_STATE_DATE;
		public string LANGUAGE;
		public string MATCH;
		public string MATCH_DESCRIPTION;



		// Utilities

		public override string ToString() { return ADDRESS; }



		// Constructors

		public DCC_PostalAddress()
		{
		}

		public DCC_PostalAddress(string uprn)
		{
			string url = ConfigurationManager.AppSettings["OS_Proxy_URL"] ?? null;
			if (string.IsNullOrEmpty(url)) throw new NotImplementedException();
			string urlParameters = "?https://api.os.uk/search/places/v1/uprn?uprn=" + uprn;
			try
			{
				HttpClient httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(url);
				HttpResponseMessage response = httpClient.GetAsync(urlParameters).Result;
				if (response.IsSuccessStatusCode)
				{
					string result = response.Content.ReadAsStringAsync().Result;
					if (result.Contains("\"results\" : [ "))
					{
						result = result.Substring(result.IndexOf("\"results\" : [ ") +12);//strip off header
						result = result.Substring(0, result.Length -1);// strip off closing bracket of header
						var addresses = JsonConvert.DeserializeObject<List<AddressList>>(result);
						
						if (addresses[0].DPA.UPRN == uprn)
						{
							UPRN =                             addresses[0].DPA.UPRN;
							UDPRN =                            addresses[0].DPA.UDPRN;
							ADDRESS =                          addresses[0].DPA.ADDRESS;
							BUILDING_NUMBER =                  addresses[0].DPA.BUILDING_NUMBER;
							BUILDING_NAME =                    addresses[0].DPA.BUILDING_NAME;
							THOROUGHFARE_NAME =                addresses[0].DPA.THOROUGHFARE_NAME;
							POST_TOWN =                        addresses[0].DPA.POST_TOWN;
							POSTCODE =                         addresses[0].DPA.POSTCODE;
							RPC =                              addresses[0].DPA.RPC;
							X_COORDINATE =                     addresses[0].DPA.X_COORDINATE;
							Y_COORDINATE =                     addresses[0].DPA.Y_COORDINATE;
							STATUS =                           addresses[0].DPA.STATUS;
							LOGICAL_STATUS_CODE =              addresses[0].DPA.LOGICAL_STATUS_CODE;
							CLASSIFICATION_CODE =              addresses[0].DPA.CLASSIFICATION_CODE;
							CLASSIFICATION_CODE_DESCRIPTION =  addresses[0].DPA.CLASSIFICATION_CODE_DESCRIPTION;
							LOCAL_CUSTODIAN_CODE =             addresses[0].DPA.LOCAL_CUSTODIAN_CODE;
							LOCAL_CUSTODIAN_CODE_DESCRIPTION = addresses[0].DPA.LOCAL_CUSTODIAN_CODE_DESCRIPTION;
							POSTAL_ADDRESS_CODE =              addresses[0].DPA.POSTAL_ADDRESS_CODE;
							POSTAL_ADDRESS_CODE_DESCRIPTION =  addresses[0].DPA.POSTAL_ADDRESS_CODE_DESCRIPTION;
							BLPU_STATE_CODE =                  addresses[0].DPA.BLPU_STATE_CODE;
							BLPU_STATE_CODE_DESCRIPTION =      addresses[0].DPA.BLPU_STATE_CODE_DESCRIPTION;
							TOPOGRAPHY_LAYER_TOID =            addresses[0].DPA.TOPOGRAPHY_LAYER_TOID;
							LAST_UPDATE_DATE =                 addresses[0].DPA.LAST_UPDATE_DATE;
							ENTRY_DATE =                       addresses[0].DPA.ENTRY_DATE;
							BLPU_STATE_DATE =                  addresses[0].DPA.BLPU_STATE_DATE;
							LANGUAGE =                         addresses[0].DPA.LANGUAGE;
							MATCH =                            addresses[0].DPA.MATCH;
							MATCH_DESCRIPTION =                addresses[0].DPA.MATCH_DESCRIPTION;
						}
					}
				}
				httpClient.Dispose();
			}
			catch (Exception e)
			{
			}
		}

	}

	#endregion


	#region DCC_PostalAddressList

	public class DCC_PostalAddressList : IEnumerable<DCC_PostalAddress>
	{

		// Properties

		public List<DCC_PostalAddress> All = new List<DCC_PostalAddress>();



		// Utilities

		public int Count { get { return All.Count; } }
		public override string ToString() { return "DCC_PostalAddressList (" + Count + " items)"; }



		// Constructors

		public DCC_PostalAddressList()
		{
		}

		public DCC_PostalAddressList(string postcode)
		{
			string url = ConfigurationManager.AppSettings["OS_Proxy_URL"] ?? null;
			if (string.IsNullOrEmpty(url)) throw new NotImplementedException();
			string urlParameters = "?https://api.os.uk/search/places/v1/postcode?postcode=" + postcode;
			try
			{
				HttpClient httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(url);
				HttpResponseMessage response = httpClient.GetAsync(urlParameters).Result;
				if (response.IsSuccessStatusCode)
				{
					string result = response.Content.ReadAsStringAsync().Result;
					if (result.Contains("\"results\" : [ "))
					{
						result = result.Substring(result.IndexOf("\"results\" : [ ") +12);//strip off header
						result = result.Substring(0, result.Length -1);// strip off closing bracket of header
						var addresses = JsonConvert.DeserializeObject<List<AddressList>>(result);
						foreach (var address in addresses) Add(address.DPA);
					}
				}
				httpClient.Dispose();
			}
			catch (Exception e)
			{
			}
		}



		// Methods

		void Add(DCC_PostalAddress postalAddress)
		{
			All.Add(postalAddress);
		}



		// Enumerator

		public IEnumerator<DCC_PostalAddress> GetEnumerator() { foreach (object o in All) yield return (DCC_PostalAddress)o; }
		IEnumerator IEnumerable.GetEnumerator() { foreach (object o in All) yield return "dummy"; }

	}


	public class AddressList
	{
		public DCC_PostalAddress DPA;
	}

	#endregion

}
