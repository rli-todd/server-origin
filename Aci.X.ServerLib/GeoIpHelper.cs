using System;
using System.Collections.Concurrent;
using MaxMind.GeoIP2;
using NLog;

namespace Aci.X.ServerLib
{
  public class GeoIpHelper
  {
    private static DatabaseReader _reader = new DatabaseReader(WebServiceConfig.MaxMindGeoIpDatabasePath);
    private static string _strPermittedCountryIsoCodes = WebServiceConfig.MaxMindGeoIpPermittedCountryIsoCodes;
    private static ConcurrentDictionary<string, int> _dictRejectedCountries = new ConcurrentDictionary<string, int>();
    private static DateTime _dtLastLogged = DateTime.MinValue;
    private static Logger _logger = LogManager.GetCurrentClassLogger();

    public static string GetCountryISO(string strIpAddress)
    {
      var country = _reader.Country(strIpAddress);
      return country == null ? null : country.Country.IsoCode;
    }

    public static bool IsCountryPermitted(string strCountryISO)
    {
      bool boolIsBlocked = !_strPermittedCountryIsoCodes.Contains(strCountryISO);
      if (boolIsBlocked)
      {
        _dictRejectedCountries[strCountryISO] = _dictRejectedCountries.ContainsKey(strCountryISO)
          ? _dictRejectedCountries[strCountryISO] + 1
          : 1;
        if (DateTime.Now.Subtract(_dtLastLogged).TotalSeconds >=
            WebServiceConfig.MaxMindGeoRejectedLoggingIntervalSecs)
        {
          _dtLastLogged = DateTime.Now;
          foreach (var countryISO in _dictRejectedCountries.Keys)
          {
            if (_dictRejectedCountries[countryISO] > 0)
            {
              _logger.Warn("GeoIP rejected {0} visits from ISO {1}", _dictRejectedCountries[countryISO], countryISO);
              _dictRejectedCountries[countryISO] = 0;
            }
          }
        }
      }
      return boolIsBlocked;
    }
  }
}
